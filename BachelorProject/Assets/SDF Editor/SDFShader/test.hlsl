#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 positionSDF, float rotationSDF, float scaleSDF, float2 distance, float2 finiteClamp,
               float2 rect365_position, float2 rect365_box, float rect365_scale, float4 rect365_roundness, float rect365_rotation, float2 circle442_position, float circle442_radius, float sSubtract133_k){ 
        
        uv = transform(positionSDF, rotationSDF, scaleSDF, uv);
         uv = uv - distance * clamp(round(uv/distance), -finiteClamp, finiteClamp);
        
        float2 t_rect365 = transform(rect365_position, rect365_rotation, rect365_scale, uv);
        rect365_roundness.xy = (t_rect365.x > 0.0) ? rect365_roundness.xy : rect365_roundness.zw;
        rect365_roundness.x  = (t_rect365.y  > 0.0) ? rect365_roundness.x  : rect365_roundness.y;
        float2 q_rect365 = abs(t_rect365) - rect365_box + rect365_roundness.x;
        float rect365_out = (min(max(q_rect365.x,q_rect365.y),0.0) + length(max(q_rect365,0.0)) - rect365_roundness.x) * rect365_scale;
        float circle442_out = length(circle442_position- uv)- circle442_radius;

    float h_sSubtract133 = clamp( 0.5 - 0.5*(circle442_out+rect365_out)/sSubtract133_k, 0.0, 1.0 );
    float sSubtract133_out = lerp( circle442_out, -rect365_out, h_sSubtract133 ) + sSubtract133_k*h_sSubtract133*(1.0-h_sSubtract133);


        return sSubtract133_out*scaleSDF;
    }
        

    float4 sdfColor (float2 uv, float sdfOut, 
                     float4 insideColor, sampler2D insideTex, float2 insideTexPosition, float insideTexScale, float insideTexRotation, 
                     float4 outsideColor, sampler2D outsideTex, float2 outsideTexPosition, float outsideTexScale, float outsideTexRotation, 
                     float4 outlineColor, sampler2D outlineTex, float2 outlineTexPosition, float outlineTexScale, float outlineTexRotation, 
                     float outlineThickness,float outlineSmoothness, float outlineInRepetition, float outlineOutRepetition, float outlineLineDistance){

        float4 iColor = tex2D(insideTex, transform(insideTexPosition, insideTexRotation, insideTexScale, uv) + float2(0.5, 0.5)) * insideColor;
        float4 oColor = tex2D(outsideTex, transform(outsideTexPosition, outsideTexRotation, outsideTexScale, uv) + float2(0.5, 0.5)) * outsideColor;
        float4 olColor = tex2D(outlineTex, transform(outlineTexPosition, outlineTexRotation, outlineTexScale, uv) + float2(0.5, 0.5)) * outlineColor;

        oColor.rgb = oColor.a == 0 ? olColor.rgb : oColor.rgb;
        outlineInRepetition += outlineThickness;
        outlineOutRepetition += outlineThickness;

        float sdf = smoothstep(-0.0025, 0.0025  ,sdfOut);
        float4 col = lerp(iColor ,oColor, sdf);
        float outline = 1-smoothstep(0, outlineThickness*0.01 ,abs(frac(sdfOut / (outlineLineDistance*0.1) + 0.5) - 0.5) * (outlineLineDistance*0.1));
        outline = smoothstep(0, outlineSmoothness*0.1, outline);
        outline *= step(sdfOut - max(outlineOutRepetition *0.01, 0), 0);
        outline = min(step(1-sdfOut - max((outlineInRepetition+100)*0.01, 0), 0), outline);
        col = lerp(col, olColor, outline);

        return col;
    }
#endif
