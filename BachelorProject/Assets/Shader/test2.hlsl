#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float lerp96_t, float2 rect550_position, float2 rect550_box, float rect550_scale, float4 rect550_roundness, float2 circle686_position, float circle686_radius){ 
        
        rect550_roundness.xy = (rect550_position.x - uv.x > 0.0) ? rect550_roundness.xy : rect550_roundness.zw;
        rect550_roundness.x  = (rect550_position.y - uv.y > 0.0) ? rect550_roundness.x  : rect550_roundness.y;
        float2 q_rect550 = abs((rect550_position - uv)*1/rect550_scale) - rect550_box + rect550_roundness.x;
        float rect550_out = (min(max(q_rect550.x,q_rect550.y),0.0) + length(max(q_rect550,0.0)) - rect550_roundness.x) * rect550_scale;
        
        float circle686_out = length(circle686_position- uv)- circle686_radius;

        float lerp96_out = lerp(rect550_out,circle686_out, lerp96_t);

         return lerp96_out;
        }
        
#endif
