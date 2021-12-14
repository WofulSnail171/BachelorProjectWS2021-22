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
               float2 triangle600_position, float2 triangle600_a, float2 triangle600_b, float2 triangle600_c, float triangle600_scale, float triangle600_rotation, float2 circle662_position, float circle662_radius, float sSubtract899_k){ 
        
        uv = transform(positionSDF, rotationSDF, scaleSDF, uv);
        
    float2 e0_triangle600 = triangle600_b - triangle600_a;
    float2 e1_triangle600 = triangle600_c - triangle600_b;
    float2 e2_triangle600 = triangle600_a - triangle600_c;
    
    float2 t_triangle600 = transform(triangle600_position, triangle600_rotation, triangle600_scale, uv);
    
    float2 v0_triangle600 = t_triangle600 - triangle600_a;
    float2 v1_triangle600 = t_triangle600 - triangle600_b;
    float2 v2_triangle600 = t_triangle600 - triangle600_c;
    
    float2 pq0_triangle600 = v0_triangle600 - e0_triangle600 * clamp( dot(v0_triangle600,e0_triangle600)/dot(e0_triangle600,e0_triangle600), 0.0, 1.0 );
    float2 pq1_triangle600 = v1_triangle600 - e1_triangle600 * clamp( dot(v1_triangle600,e1_triangle600)/dot(e1_triangle600,e1_triangle600), 0.0, 1.0 );
    float2 pq2_triangle600 = v2_triangle600 - e2_triangle600 * clamp( dot(v2_triangle600,e2_triangle600)/dot(e2_triangle600,e2_triangle600), 0.0, 1.0 );
    
    float s_triangle600 = sign( e0_triangle600.x*e2_triangle600.y - e0_triangle600.y*e2_triangle600.x ) ;
    float2 d_triangle600 = min(min(float2(dot(pq0_triangle600,pq0_triangle600), s_triangle600*(v0_triangle600.x*e0_triangle600.y-v0_triangle600.y*e0_triangle600.x)),
                       float2(dot(pq1_triangle600,pq1_triangle600), s_triangle600*(v1_triangle600.x*e1_triangle600.y-v1_triangle600.y*e1_triangle600.x))),
                       float2(dot(pq2_triangle600,pq2_triangle600), s_triangle600*(v2_triangle600.x*e2_triangle600.y-v2_triangle600.y*e2_triangle600.x)));
    float triangle600_out= -sqrt(d_triangle600.x) * sign(d_triangle600.y) * triangle600_scale;
        float circle662_out = length(circle662_position- uv)- circle662_radius;

    float h_sSubtract899 = clamp( 0.5 - 0.5*(circle662_out+triangle600_out)/sSubtract899_k, 0.0, 1.0 );
    float sSubtract899_out = lerp( circle662_out, -triangle600_out, h_sSubtract899 ) + sSubtract899_k*h_sSubtract899*(1.0-h_sSubtract899);


        return sSubtract899_out*scaleSDF;
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
