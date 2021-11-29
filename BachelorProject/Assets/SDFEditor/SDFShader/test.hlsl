#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 rect494_position, float2 rect494_box, float rect494_scale, float4 rect494_roundness, float2 circle959_position, float circle959_radius, float lerp385_t){ 
        
        rect494_roundness.xy = (rect494_position.x - uv.x > 0.0) ? rect494_roundness.xy : rect494_roundness.zw;
        rect494_roundness.x  = (rect494_position.y - uv.y > 0.0) ? rect494_roundness.x  : rect494_roundness.y;
        float2 q_rect494 = abs((rect494_position - uv)*1/rect494_scale) - rect494_box + rect494_roundness.x;
        float rect494_out = (min(max(q_rect494.x,q_rect494.y),0.0) + length(max(q_rect494,0.0)) - rect494_roundness.x) * rect494_scale;
        float circle959_out = length(circle959_position- uv)- circle959_radius;

        float comb_out = min(circle959_out,rect494_out);

        float lerp385_out = lerp(comb_out,circle959_out, lerp385_t);

        return lerp385_out;
    }
        
#endif
