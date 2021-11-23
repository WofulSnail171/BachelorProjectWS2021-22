#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float lerp305_t, float2 tex829_position, sampler2D tex829_tex, float2 circle429_position, float circle429_radius){ 
        float tex829_out = tex2D(tex829_tex, uv + tex829_position + float2(0.5, 0.5)).r;
        
        float circle429_out = length(circle429_position- uv)- circle429_radius;

        float lerp305_out = lerp(tex829_out,circle429_out, lerp305_t);

         return lerp305_out;
        }
        
#endif
