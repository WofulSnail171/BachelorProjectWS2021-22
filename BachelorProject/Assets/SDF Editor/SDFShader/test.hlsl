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
               float2 rect465_position, float2 rect465_box, float rect465_scale, float4 rect465_roundness, float rect465_rotation, float2 circle129_position, float circle129_radius, float sSubtract310_k){ 
        
        uv = transform(positionSDF, rotationSDF, scaleSDF, uv);
         uv = uv - distance * clamp(round(uv/distance), -finiteClamp, finiteClamp);
        
        float2 t_rect465 = transform(rect465_position, rect465_rotation, rect465_scale, uv);
        rect465_roundness.xy = (t_rect465.x > 0.0) ? rect465_roundness.xy : rect465_roundness.zw;
        rect465_roundness.x  = (t_rect465.y  > 0.0) ? rect465_roundness.x  : rect465_roundness.y;
        float2 q_rect465 = abs(t_rect465) - rect465_box + rect465_roundness.x;
        float rect465_out = (min(max(q_rect465.x,q_rect465.y),0.0) + length(max(q_rect465,0.0)) - rect465_roundness.x) * rect465_scale;
        float circle129_out = length(circle129_position- uv)- circle129_radius;

    float h_sSubtract310 = clamp( 0.5 - 0.5*(circle129_out+rect465_out)/sSubtract310_k, 0.0, 1.0 );
    float sSubtract310_out = lerp( circle129_out, -rect465_out, h_sSubtract310 ) + sSubtract310_k*h_sSubtract310*(1.0-h_sSubtract310);


        return sSubtract310_out*scaleSDF;
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
