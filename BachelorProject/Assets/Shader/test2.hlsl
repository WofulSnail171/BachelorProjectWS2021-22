#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 circle343_position, float circle343_radius, float2 rect474_position, float2 rect474_box, float rect474_scale, float4 rect474_roundness){ 
        float circle343_out = length(circle343_position- uv)- circle343_radius;
    
        float2 pa = uv - line996_position - line996_a;
        float2 ba = line996_b - line996_a;
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float line996_out = length(pa - ba*h) - line996_roundness;
    float comb_out = min(circle343_out,line996_out);


         return comb_out;
        }
        
#endif
