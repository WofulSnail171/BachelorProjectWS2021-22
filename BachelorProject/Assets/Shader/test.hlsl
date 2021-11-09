#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 rect636_position, float2 rect636_box, float rect636_scale, float4 rect636_roundness){ 
        rect636_roundness.xy = (rect636_position.x - uv.x > 0.0) ? rect636_roundness.xy : rect636_roundness.zw;
        rect636_roundness.x  = (rect636_position.y - uv.y > 0.0) ? rect636_roundness.x  : rect636_roundness.y;
        float2 q = abs(rect636_position - uv) - rect636_box + rect636_roundness.x;
        float rect636_out = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - rect636_roundness.x;

         return rect636_out;
        }
        
#endif
