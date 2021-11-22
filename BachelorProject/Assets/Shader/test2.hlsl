#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 rect773_position, float2 rect773_box, float rect773_scale, float4 rect773_roundness){ 
        rect773_roundness.xy = (rect773_position.x - uv.x > 0.0) ? rect773_roundness.xy : rect773_roundness.zw;
        rect773_roundness.x  = (rect773_position.y - uv.y > 0.0) ? rect773_roundness.x  : rect773_roundness.y;
        float2 q_rect773 = abs((rect773_position - uv)*1/rect773_scale) - rect773_box + rect773_roundness.x;
        float rect773_out = (min(max(q_rect773.x,q_rect773.y),0.0) + length(max(q_rect773,0.0)) - rect773_roundness.x) * rect773_scale;

         return rect773_out;
        }
        
#endif
