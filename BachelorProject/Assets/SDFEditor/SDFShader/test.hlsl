#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 line59_position, float2 line59_a, float2 line59_b, float line59_roundness, float line59_scale, float line59_rotation){ 
        
        
        float2 pa_line59 = transform(line59_position, line59_rotation, line59_scale, uv) - line59_a;
        float2 ba_line59 = line59_b - line59_a;
        float h_line59 = clamp(dot(pa_line59, ba_line59)/dot(ba_line59, ba_line59), 0, 1);
        float line59_out = (length(pa_line59 - ba_line59*h_line59) - (0.01 * line59_roundness)) * line59_scale;

        return line59_out;
    }
        
#endif
