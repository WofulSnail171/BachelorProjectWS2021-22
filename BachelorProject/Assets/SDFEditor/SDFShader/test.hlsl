#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 positionSDF, float rotationSDF, float scaleSDF,
               float2 rect962_position, float2 rect962_box, float rect962_scale, float4 rect962_roundness, float rect962_rotation, float2 line92_position, float2 line92_a, float2 line92_b, float line92_roundness, float line92_scale, float line92_rotation){ 
        
        uv = transform(positionSDF, rotationSDF, scaleSDF, uv);
        
        float2 t_rect962 = transform(rect962_position, rect962_rotation, rect962_scale, uv);
        rect962_roundness.xy = (t_rect962.x > 0.0) ? rect962_roundness.xy : rect962_roundness.zw;
        rect962_roundness.x  = (t_rect962.y  > 0.0) ? rect962_roundness.x  : rect962_roundness.y;
        float2 q_rect962 = abs(t_rect962) - rect962_box + rect962_roundness.x;
        float rect962_out = (min(max(q_rect962.x,q_rect962.y),0.0) + length(max(q_rect962,0.0)) - rect962_roundness.x) * rect962_scale;
        
        float2 pa_line92 = transform(line92_position, line92_rotation, line92_scale, uv) - line92_a;
        float2 ba_line92 = line92_b - line92_a;
        float h_line92 = clamp(dot(pa_line92, ba_line92)/dot(ba_line92, ba_line92), 0, 1);
        float line92_out = (length(pa_line92 - ba_line92*h_line92) - (0.01 * line92_roundness)) * line92_scale;

        float comb572_out = min(line92_out,rect962_out);


        return comb572_out*scaleSDF;
    }
        

    float4 sdfColor (float2 uv, float sdfOut, 
                     float4 insideColor, sampler2D insideTex, float2 insideTexPosition, float insideTexScale, float insideTexRotation, 
                     float4 outsideColor, sampler2D outsideTex, float2 outsideTexPosition, float outsideTexScale, float outsideTexRotation, 
                     float4 outlineColor, sampler2D outlineTex, float2 outlineTexPosition, float outlineTexScale, float outlineTexRotation, 
                     float outlineThickness, float outlineRepetition, float outlineLineDistance){

        float4 iColor = tex2D(insideTex, transform(insideTexPosition, insideTexRotation, insideTexScale, uv) + float2(0.5, 0.5)) * insideColor;
        float4 oColor = tex2D(outsideTex, transform(outsideTexPosition, outsideTexRotation, outsideTexScale, uv) + float2(0.5, 0.5)) * outsideColor;
        float4 olColor = tex2D(outlineTex, transform(outlineTexPosition, outlineTexRotation, outlineTexScale, uv) + float2(0.5, 0.5)) * outlineColor;

        float sdf = smoothstep(0, outlineThickness *0.01 - outlineThickness*0.005 ,sdfOut);
        float4 col = lerp(iColor ,oColor, sdf);
        float outline = 1-smoothstep(0, outlineThickness*0.01 ,abs(frac(sdfOut / (outlineLineDistance*0.1) + 0.5) - 0.5) * (outlineLineDistance*0.1));
        outline *= step(sdfOut - outlineRepetition *0.01, 0);
        col = lerp(col, olColor, outline);

        return col;
    }
#endif
