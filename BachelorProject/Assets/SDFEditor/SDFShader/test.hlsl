#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 circle167_position, float circle167_radius){ 
        
        float circle167_out = length(circle167_position- uv)- circle167_radius;

         return circle167_out;
        }
        
#endif
