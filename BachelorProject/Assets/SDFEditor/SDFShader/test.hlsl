#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 tex434_position, sampler2D tex434_tex, float tex434_scale, float tex434_rotation){ 
        float tex434_out = tex2D(tex434_tex, transform(tex434_position, tex434_rotation, tex434_scale, uv) + tex434_position + float2(0.5, 0.5)).r;

        return tex434_out;
    }
        
#endif
