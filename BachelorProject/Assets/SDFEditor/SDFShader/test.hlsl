#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 rect2_position, float2 rect2_box, float rect2_scale, float4 rect2_roundness, float2 circle283_position, float circle283_radius, float lerp929_t){ 
        
        rect2_roundness.xy = (rect2_position.x - uv.x > 0.0) ? rect2_roundness.xy : rect2_roundness.zw;
        rect2_roundness.x  = (rect2_position.y - uv.y > 0.0) ? rect2_roundness.x  : rect2_roundness.y;
        float2 q_rect2 = abs((rect2_position - uv)*1/rect2_scale) - rect2_box + rect2_roundness.x;
        float rect2_out = (min(max(q_rect2.x,q_rect2.y),0.0) + length(max(q_rect2,0.0)) - rect2_roundness.x) * rect2_scale;
        float circle283_out = length(circle283_position- uv)- circle283_radius;

        float comb_out = min(circle283_out,rect2_out);

        float lerp929_out = lerp(comb_out,circle283_out, lerp929_t);

        return lerp929_out;
    }
        
#endif
