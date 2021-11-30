#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 rect121_position, float2 rect121_box, float rect121_scale, float4 rect121_roundness, float rect121_rotation, float2 circle125_position, float circle125_radius, float lerp979_t){ 
        
        float2 t_rect121 = transform(rect121_position, rect121_rotation, rect121_scale, uv);
        rect121_roundness.xy = (t_rect121.x > 0.0) ? rect121_roundness.xy : rect121_roundness.zw;
        rect121_roundness.x  = (t_rect121.y  > 0.0) ? rect121_roundness.x  : rect121_roundness.y;
        float2 q_rect121 = abs(t_rect121) - rect121_box + rect121_roundness.x;
        float rect121_out = (min(max(q_rect121.x,q_rect121.y),0.0) + length(max(q_rect121,0.0)) - rect121_roundness.x) * rect121_scale;
        float circle125_out = length(circle125_position- uv)- circle125_radius;

        float comb0_out = min(circle125_out,rect121_out);

        float lerp979_out = lerp(comb0_out,circle125_out, lerp979_t);

        return lerp979_out;
    }
        
#endif
