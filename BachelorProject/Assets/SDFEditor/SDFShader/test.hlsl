#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 triangle490_position, float2 triangle490_a, float2 triangle490_b, float2 triangle490_c, float triangle490_scale, float triangle490_rotation, float2 circle646_position, float circle646_radius){ 
        
    float2 e0_triangle490 = triangle490_b - triangle490_a;
    float2 e1_triangle490 = triangle490_c - triangle490_b;
    float2 e2_triangle490 = triangle490_a - triangle490_c;
    
    float2 t_triangle490 = transform(triangle490_position, triangle490_rotation, triangle490_scale, uv);
    
    float2 v0_triangle490 = t_triangle490 - triangle490_a;
    float2 v1_triangle490 = t_triangle490 - triangle490_b;
    float2 v2_triangle490 = t_triangle490 - triangle490_c;
    
    float2 pq0_triangle490 = v0_triangle490 - e0_triangle490 * clamp( dot(v0_triangle490,e0_triangle490)/dot(e0_triangle490,e0_triangle490), 0.0, 1.0 );
    float2 pq1_triangle490 = v1_triangle490 - e1_triangle490 * clamp( dot(v1_triangle490,e1_triangle490)/dot(e1_triangle490,e1_triangle490), 0.0, 1.0 );
    float2 pq2_triangle490 = v2_triangle490 - e2_triangle490 * clamp( dot(v2_triangle490,e2_triangle490)/dot(e2_triangle490,e2_triangle490), 0.0, 1.0 );
    
    float s_triangle490 = sign( e0_triangle490.x*e2_triangle490.y - e0_triangle490.y*e2_triangle490.x ) ;
    float2 d_triangle490 = min(min(float2(dot(pq0_triangle490,pq0_triangle490), s_triangle490*(v0_triangle490.x*e0_triangle490.y-v0_triangle490.y*e0_triangle490.x)),
                       float2(dot(pq1_triangle490,pq1_triangle490), s_triangle490*(v1_triangle490.x*e1_triangle490.y-v1_triangle490.y*e1_triangle490.x))),
                       float2(dot(pq2_triangle490,pq2_triangle490), s_triangle490*(v2_triangle490.x*e2_triangle490.y-v2_triangle490.y*e2_triangle490.x)));
    float triangle490_out= -sqrt(d_triangle490.x) * sign(d_triangle490.y) * triangle490_scale;
        float circle646_out = length(circle646_position- uv)- circle646_radius;

        float subtract69_out = max(circle646_out,triangle490_out);

        return subtract69_out;
    }
        

    float4 sdfColor (float2 uv, float sdfOut, float4 color855, float4 color646, float2 tex705_position, sampler2D tex705_tex, float tex705_scale, float tex705_rotation, float4 tex705_color, float colorOutput839_thickness, float colorOutput839_repetition, float colorOutput839_lineDistance){
        float4  tex705_out = tex2D(tex705_tex, transform(tex705_position, tex705_rotation, tex705_scale, uv) + tex705_position + float2(0.5, 0.5)) * tex705_color;
        float sdf_colorOutput839 = smoothstep(0,colorOutput839_thickness*0.01 - colorOutput839_thickness*0.005 ,sdfOut);
        float4 col_colorOutput839 = lerp(tex705_out , color646, sdf_colorOutput839);
        float outline_colorOutput839 = 1-smoothstep(0,colorOutput839_thickness*0.01 ,abs(frac(sdfOut / (colorOutput839_lineDistance*0.1) + 0.5) - 0.5) * (colorOutput839_lineDistance*0.1));
        outline_colorOutput839 *= step(sdfOut-colorOutput839_repetition *0.01, 0);
        col_colorOutput839 = lerp(col_colorOutput839, color855, outline_colorOutput839);

        float4 colorOutput839_out = col_colorOutput839;

        return colorOutput839_out;
    }


#endif
