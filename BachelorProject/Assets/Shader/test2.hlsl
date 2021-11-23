#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 tex829_position, sampler2D tex829_tex){ 
        float tex829_out = tex2D(tex829_tex, uv + tex829_position + float2(0.5, 0.5)).r;

         return tex829_out;
        }
        
#endif
