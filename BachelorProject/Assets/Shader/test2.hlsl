#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 line538_position, float2 line538_a, float2 line538_b, float line538_roundness, float2 circle187_position, float circle187_radius){ 
        
        float2 pa = uv - line914_position - line914_a;
        float2 ba = line914_b - line914_a;
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float line914_out = length(pa - ba*h) - line914_roundness;
    float circle187_out = length(circle187_position- uv)- circle187_radius;
    float comb_out = min(line914_out,circle187_out);


         return comb_out;
        }
        
#endif
