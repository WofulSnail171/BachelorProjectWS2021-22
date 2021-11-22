#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float lerp45_t, float2 circle199_position, float circle199_radius, float2 rect773_position, float2 rect773_box, float rect773_scale, float4 rect773_roundness){ 
        float circle199_out = length(circle199_position- uv)- circle199_radius;
    rect773_roundness.xy = (rect773_position.x - uv.x > 0.0) ? rect773_roundness.xy : rect773_roundness.zw;
        rect773_roundness.x  = (rect773_position.y - uv.y > 0.0) ? rect773_roundness.x  : rect773_roundness.y;
        float2 q = abs(rect773_position - uv) - rect773_box + rect773_roundness.x;
        float rect773_out = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - rect773_roundness.x;
    float lerp45_out = lerp(circle199_out,rect773_out, lerp45_t);

         return lerp45_out;
        }
        
#endif
