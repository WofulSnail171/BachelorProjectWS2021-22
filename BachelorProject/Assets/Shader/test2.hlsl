#ifndef TEST2_INCLUDE
#define TEST2_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float sblend571_k, float lerp305_t, float2 rect983_position, float2 rect983_box, float rect983_scale, float4 rect983_roundness, float2 bezier796_position, float2 bezier796_a, float2 bezier796_b, float2 bezier796_c, float2 circle429_position, float circle429_radius){ 
        
        rect983_roundness.xy = (rect983_position.x - uv.x > 0.0) ? rect983_roundness.xy : rect983_roundness.zw;
        rect983_roundness.x  = (rect983_position.y - uv.y > 0.0) ? rect983_roundness.x  : rect983_roundness.y;
        float2 q_rect983 = abs((rect983_position - uv)*1/rect983_scale) - rect983_box + rect983_roundness.x;
        float rect983_out = (min(max(q_rect983.x,q_rect983.y),0.0) + length(max(q_rect983,0.0)) - rect983_roundness.x) * rect983_scale;
        
        float2 pos_bezier796 = uv - bezier796_position;
        float2 A_bezier796 = bezier796_b - bezier796_a;
        float2 B_bezier796 = bezier796_a - 2.0 * bezier796_b + bezier796_c;
        float2 C_bezier796 =  A_bezier796 * 2.0;
        float2 D_bezier796 =  bezier796_a - pos_bezier796;
        float kk_bezier796 = 1.0/dot(B_bezier796, B_bezier796);
        float kx_bezier796 = kk_bezier796 * dot(A_bezier796, B_bezier796);
        float ky_bezier796 = kk_bezier796 * (2.0*dot(A_bezier796, A_bezier796)+dot(D_bezier796, B_bezier796)) / 3.0;
        float kz_bezier796 = kk_bezier796 * dot(D_bezier796, A_bezier796);      
        float res_bezier796 = 0.0;
        float p_bezier796 = ky_bezier796 - kx_bezier796 * kx_bezier796;
        float p3_bezier796 = p_bezier796 * p_bezier796 * p_bezier796;
        float q_bezier796 = kx_bezier796 * (2.0 * kx_bezier796 * kx_bezier796 - 3.0 * ky_bezier796) + kz_bezier796;
        float h_bezier796 = q_bezier796 * q_bezier796 + 4.0 * p3_bezier796;
        if( h_bezier796 >= 0.0) 
        { 
            h_bezier796 = sqrt(h_bezier796);
            float2 x_bezier796 = (float2(h_bezier796,-h_bezier796)-q_bezier796)/2.0;
            float2 uv_bezier796 = sign(x_bezier796) * pow(abs(x_bezier796), float2(1.0/3.0,1.0/3.0));
            float t_bezier796 = clamp( uv_bezier796.x+uv_bezier796.y-kx_bezier796, 0.0, 1.0 );
            res_bezier796 = dot2(D_bezier796 + (C_bezier796 + B_bezier796 * t_bezier796) * t_bezier796);
        }
        else
        {
            float z_bezier796 = sqrt(-p_bezier796);
            float v_bezier796 = acos( q_bezier796/(p_bezier796 * z_bezier796 * 2.0) ) / 3.0;
            float m_bezier796 = cos(v_bezier796);
            float n_bezier796 = sin(v_bezier796) * 1.732050808;
            float3  t_bezier796 = clamp(float3(m_bezier796 + m_bezier796,-n_bezier796 - m_bezier796, n_bezier796 - m_bezier796) * z_bezier796 - kx_bezier796,0.0,1.0);
        
            res_bezier796 = min( dot2(D_bezier796 + (C_bezier796 + B_bezier796 * t_bezier796.x) * t_bezier796.x),
                dot2(D_bezier796 + (C_bezier796 + B_bezier796 * t_bezier796.y) * t_bezier796.y) );
        }
        float bezier796_out = sqrt(res_bezier796);

        float lerp305_out = lerp(rect983_out,bezier796_out, lerp305_t);
    
        float circle429_out = length(circle429_position- uv)- circle429_radius;

    float h_sblend571 = max( sblend571_k - abs(lerp305_out - circle429_out), 0.0 )/sblend571_k;
    float sblend571_out =  min( lerp305_out, circle429_out) - h_sblend571*h_sblend571*sblend571_k*(1.0/4.0);

         return sblend571_out;
        }
        
#endif
