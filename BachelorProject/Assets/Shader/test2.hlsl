#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 circle199_position, float circle199_radius, float2 line109_position, float2 line109_a, float2 line109_b, float line109_roundness){ 
        float circle199_out = length(circle199_position- uv)- circle199_radius;
    
        float2 pa = uv - line109_position - line109_a;
        float2 ba = line109_b - line109_a;
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float line109_out = length(pa - ba*h) - line109_roundness;
    float comb_out = min(circle199_out,line109_out);


         return comb_out;
        }
        
#endif
