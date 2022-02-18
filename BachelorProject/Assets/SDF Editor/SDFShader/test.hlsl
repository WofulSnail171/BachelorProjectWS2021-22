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
               float2 triangle332_position, float2 triangle332_a, float2 triangle332_b, float2 triangle332_c, float triangle332_scale, float triangle332_rotation, float2 circle470_position, float circle470_radius, float2 circle941_position, float circle941_radius, float sCombine489_k, float sCombine148_k, float2 triangle152_position, float2 triangle152_a, float2 triangle152_b, float2 triangle152_c, float triangle152_scale, float triangle152_rotation, float2 circle295_position, float circle295_radius, float2 circle606_position, float circle606_radius, float sCombine226_k, float sCombine917_k){ 
        
        uv = transform(positionSDF, rotationSDF, scaleSDF, uv);
        
        
    float2 e0_triangle332 = triangle332_b - triangle332_a;
    float2 e1_triangle332 = triangle332_c - triangle332_b;
    float2 e2_triangle332 = triangle332_a - triangle332_c;
    
    float2 t_triangle332 = transform(triangle332_position, triangle332_rotation, triangle332_scale, uv);
    
    float2 v0_triangle332 = t_triangle332 - triangle332_a;
    float2 v1_triangle332 = t_triangle332 - triangle332_b;
    float2 v2_triangle332 = t_triangle332 - triangle332_c;
    
    float2 pq0_triangle332 = v0_triangle332 - e0_triangle332 * clamp( dot(v0_triangle332,e0_triangle332)/dot(e0_triangle332,e0_triangle332), 0.0, 1.0 );
    float2 pq1_triangle332 = v1_triangle332 - e1_triangle332 * clamp( dot(v1_triangle332,e1_triangle332)/dot(e1_triangle332,e1_triangle332), 0.0, 1.0 );
    float2 pq2_triangle332 = v2_triangle332 - e2_triangle332 * clamp( dot(v2_triangle332,e2_triangle332)/dot(e2_triangle332,e2_triangle332), 0.0, 1.0 );
    
    float s_triangle332 = sign( e0_triangle332.x*e2_triangle332.y - e0_triangle332.y*e2_triangle332.x ) ;
    float2 d_triangle332 = min(min(float2(dot(pq0_triangle332,pq0_triangle332), s_triangle332*(v0_triangle332.x*e0_triangle332.y-v0_triangle332.y*e0_triangle332.x)),
                       float2(dot(pq1_triangle332,pq1_triangle332), s_triangle332*(v1_triangle332.x*e1_triangle332.y-v1_triangle332.y*e1_triangle332.x))),
                       float2(dot(pq2_triangle332,pq2_triangle332), s_triangle332*(v2_triangle332.x*e2_triangle332.y-v2_triangle332.y*e2_triangle332.x)));
    float triangle332_out= -sqrt(d_triangle332.x) * sign(d_triangle332.y) * triangle332_scale;
        float circle470_out = length(circle470_position- uv)- circle470_radius;
        float circle941_out = length(circle941_position- uv)- circle941_radius;

    float h_sCombine489 = max( sCombine489_k - abs(circle941_out - circle470_out), 0.0 )/sCombine489_k;
    float sCombine489_out =  min( circle941_out, circle470_out) - h_sCombine489*h_sCombine489*sCombine489_k*(1.0/4.0);

    float h_sCombine148 = max( sCombine148_k - abs(sCombine489_out - triangle332_out), 0.0 )/sCombine148_k;
    float sCombine148_out =  min( sCombine489_out, triangle332_out) - h_sCombine148*h_sCombine148*sCombine148_k*(1.0/4.0);
    float2 e0_triangle152 = triangle152_b - triangle152_a;
    float2 e1_triangle152 = triangle152_c - triangle152_b;
    float2 e2_triangle152 = triangle152_a - triangle152_c;
    
    float2 t_triangle152 = transform(triangle152_position, triangle152_rotation, triangle152_scale, uv);
    
    float2 v0_triangle152 = t_triangle152 - triangle152_a;
    float2 v1_triangle152 = t_triangle152 - triangle152_b;
    float2 v2_triangle152 = t_triangle152 - triangle152_c;
    
    float2 pq0_triangle152 = v0_triangle152 - e0_triangle152 * clamp( dot(v0_triangle152,e0_triangle152)/dot(e0_triangle152,e0_triangle152), 0.0, 1.0 );
    float2 pq1_triangle152 = v1_triangle152 - e1_triangle152 * clamp( dot(v1_triangle152,e1_triangle152)/dot(e1_triangle152,e1_triangle152), 0.0, 1.0 );
    float2 pq2_triangle152 = v2_triangle152 - e2_triangle152 * clamp( dot(v2_triangle152,e2_triangle152)/dot(e2_triangle152,e2_triangle152), 0.0, 1.0 );
    
    float s_triangle152 = sign( e0_triangle152.x*e2_triangle152.y - e0_triangle152.y*e2_triangle152.x ) ;
    float2 d_triangle152 = min(min(float2(dot(pq0_triangle152,pq0_triangle152), s_triangle152*(v0_triangle152.x*e0_triangle152.y-v0_triangle152.y*e0_triangle152.x)),
                       float2(dot(pq1_triangle152,pq1_triangle152), s_triangle152*(v1_triangle152.x*e1_triangle152.y-v1_triangle152.y*e1_triangle152.x))),
                       float2(dot(pq2_triangle152,pq2_triangle152), s_triangle152*(v2_triangle152.x*e2_triangle152.y-v2_triangle152.y*e2_triangle152.x)));
    float triangle152_out= -sqrt(d_triangle152.x) * sign(d_triangle152.y) * triangle152_scale;
        float circle295_out = length(circle295_position- uv)- circle295_radius;
        float circle606_out = length(circle606_position- uv)- circle606_radius;

    float h_sCombine226 = max( sCombine226_k - abs(circle606_out - circle295_out), 0.0 )/sCombine226_k;
    float sCombine226_out =  min( circle606_out, circle295_out) - h_sCombine226*h_sCombine226*sCombine226_k*(1.0/4.0);

    float h_sCombine917 = max( sCombine917_k - abs(sCombine226_out - triangle152_out), 0.0 )/sCombine917_k;
    float sCombine917_out =  min( sCombine226_out, triangle152_out) - h_sCombine917*h_sCombine917*sCombine917_k*(1.0/4.0);

        float subtract520_out = max(-sCombine917_out,sCombine148_out);


        return subtract520_out*scaleSDF;
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
