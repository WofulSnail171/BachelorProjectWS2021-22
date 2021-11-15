#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 rect817_position, float2 rect817_box, float rect817_scale, float4 rect817_roundness){ 
        rect817_roundness.xy = (rect817_position.x - uv.x > 0.0) ? rect817_roundness.xy : rect817_roundness.zw;
        rect817_roundness.x  = (rect817_position.y - uv.y > 0.0) ? rect817_roundness.x  : rect817_roundness.y;
        float2 q = abs(rect817_position - uv) - rect817_box + rect817_roundness.x;
        float rect817_out = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - rect817_roundness.x;

         return rect817_out;
        }
        
#endif
