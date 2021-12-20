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
               float2 rect55_position, float2 rect55_box, float rect55_scale, float4 rect55_roundness, float rect55_rotation){ 
        
        uv = transform(positionSDF, rotationSDF, scaleSDF, uv);
        
        float2 t_rect55 = transform(rect55_position, rect55_rotation, rect55_scale, uv);
        rect55_roundness.xy = (t_rect55.x > 0.0) ? rect55_roundness.xy : rect55_roundness.zw;
        rect55_roundness.x  = (t_rect55.y  > 0.0) ? rect55_roundness.x  : rect55_roundness.y;
        float2 q_rect55 = abs(t_rect55) - rect55_box + rect55_roundness.x;
        float rect55_out = (min(max(q_rect55.x,q_rect55.y),0.0) + length(max(q_rect55,0.0)) - rect55_roundness.x) * rect55_scale;


        return rect55_out*scaleSDF;
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
