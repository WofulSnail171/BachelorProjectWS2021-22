#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float lerp45_t, float2 circle199_position, float circle199_radius, float2 rect773_position, float2 rect773_box, float rect773_scale, float4 rect773_roundness, float lerp771_t, float2 circle429_position, float circle429_radius, float2 rect983_position, float2 rect983_box, float rect983_scale, float4 rect983_roundness){ 
        
        float circle429_out = length(circle429_position- uv)- circle429_radius;
        
        rect983_roundness.xy = (rect983_position.x - uv.x > 0.0) ? rect983_roundness.xy : rect983_roundness.zw;
        rect983_roundness.x  = (rect983_position.y - uv.y > 0.0) ? rect983_roundness.x  : rect983_roundness.y;
        float2 q_rect983 = abs((rect983_position - uv)*1/rect983_scale) - rect983_box + rect983_roundness.x;
        float rect983_out = (min(max(q_rect983.x,q_rect983.y),0.0) + length(max(q_rect983,0.0)) - rect983_roundness.x) * rect983_scale;

        float lerp771_out = lerp(circle429_out,rect983_out, lerp45_t);

         return lerp771_out;
        }
        
#endif
