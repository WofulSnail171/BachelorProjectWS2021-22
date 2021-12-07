#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 rect138_position, float2 rect138_box, float rect138_scale, float4 rect138_roundness, float rect138_rotation, float2 circle673_position, float circle673_radius, float2 line265_position, float2 line265_a, float2 line265_b, float line265_roundness, float line265_scale, float line265_rotation){ 
        
        float2 t_rect138 = transform(rect138_position, rect138_rotation, rect138_scale, uv);
        rect138_roundness.xy = (t_rect138.x > 0.0) ? rect138_roundness.xy : rect138_roundness.zw;
        rect138_roundness.x  = (t_rect138.y  > 0.0) ? rect138_roundness.x  : rect138_roundness.y;
        float2 q_rect138 = abs(t_rect138) - rect138_box + rect138_roundness.x;
        float rect138_out = (min(max(q_rect138.x,q_rect138.y),0.0) + length(max(q_rect138,0.0)) - rect138_roundness.x) * rect138_scale;
        float circle673_out = length(circle673_position- uv)- circle673_radius;

        float comb777_out = min(circle673_out,rect138_out);
        
        float2 pa_line265 = transform(line265_position, line265_rotation, line265_scale, uv) - line265_a;
        float2 ba_line265 = line265_b - line265_a;
        float h_line265 = clamp(dot(pa_line265, ba_line265)/dot(ba_line265, ba_line265), 0, 1);
        float line265_out = (length(pa_line265 - ba_line265*h_line265) - (0.01 * line265_roundness)) * line265_scale;

        float comb802_out = min(line265_out,comb777_out);

        return comb802_out;
    }
        

    float4 sdfColor (float2 uv, float sdfOut, float4 color566, float4 color665, float colorOutput51_thickness, float colorOutput51_repetition, float colorOutput51_lineDistance){
        
        float sdf_colorOutput51 = smoothstep(0,colorOutput51_thickness*0.01 - colorOutput51_thickness*0.005 ,sdfOut);
        float4 col_colorOutput51 = lerp(color665 , color665, sdf_colorOutput51);
        float outline_colorOutput51 = 1-smoothstep(0,colorOutput51_thickness*0.01 ,abs(frac(sdfOut / (colorOutput51_lineDistance*0.1) + 0.5) - 0.5) * (colorOutput51_lineDistance*0.1));
        outline_colorOutput51 *= step(sdfOut-colorOutput51_repetition *0.01, 0);
        col_colorOutput51 = lerp(col_colorOutput51, color566, outline_colorOutput51);

        float4 colorOutput51_out = col_colorOutput51;

        return colorOutput51_out;
    }


#endif
