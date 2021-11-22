#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float sblend860_k, float2 line109_position, float2 line109_a, float2 line109_b, float line109_roundness, float2 circle199_position, float circle199_radius){ 
        
        float2 pa = uv - line109_position - line109_a;
        float2 ba = line109_b - line109_a;
        float h1 = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float line109_out = length(pa - ba*h1) - line109_roundness;
    float circle199_out = length(circle199_position- uv)- circle199_radius;
    float h = max( sblend860_k - abs(line109_out - circle199_out), 0.0 )/sblend860_k;
    float sblend860_out =  min( line109_out, circle199_out) - h*h*sblend860_k*(1.0/4.0);

         return sblend860_out;
        }
        
#endif
