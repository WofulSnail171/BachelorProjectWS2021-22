#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 circle892_position, float circle892_radius){ 
        float circle892_out = length(circle892_position- uv)- circle892_radius;

         return circle892_out;
        }
        
#endif
