#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 rect766_position, float2 rect766_box, float rect766_scale, float4 rect766_roundness, float2 circle125_position, float circle125_radius, float lerp979_t){ 
        
        rect766_roundness.xy = (rect766_position.x - uv.x > 0.0) ? rect766_roundness.xy : rect766_roundness.zw;
        rect766_roundness.x  = (rect766_position.y - uv.y > 0.0) ? rect766_roundness.x  : rect766_roundness.y;
        float2 q_rect766 = abs((rect766_position - uv)*1/rect766_scale) - rect766_box + rect766_roundness.x;
        float rect766_out = (min(max(q_rect766.x,q_rect766.y),0.0) + length(max(q_rect766,0.0)) - rect766_roundness.x) * rect766_scale;
        float circle125_out = length(circle125_position- uv)- circle125_radius;

        float comb0_out = min(circle125_out,rect766_out);

        float lerp979_out = lerp(comb0_out,circle125_out, lerp979_t);

        return lerp979_out;
    }
        
#endif
