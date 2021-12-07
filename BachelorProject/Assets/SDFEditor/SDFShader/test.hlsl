#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, float2 circle673_position, float circle673_radius){ 
        
        float circle673_out = length(circle673_position- uv)- circle673_radius;

        return circle673_out;
    }
        

    float4 sdfColor (float2 uv, float sdfOut, float4 color566, float4 color665, float colorOutput51_thickness, float colorOutput51_repetition, float colorOutput51_lineDistance){
        
        float sdf_colorOutput51 = smoothstep(0,colorOutput51_thickness*0.01 - colorOutput51_thickness*0.005 ,sdfOut);
        float4 col_colorOutput51 = lerp(color665 , color665, sdf_colorOutput51);
        float outline_colorOutput51 = 1-smoothstep(0,colorOutput51_thickness*0.01 ,abs(frac(sdfOut / (colorOutput51_lineDistance*0.1) + 0.5) - 0.5) * (colorOutput51_lineDistance*0.1));
        outline_colorOutput51 *= step(sdfOut-colorOutput51_repetition *0.01, 0);
        col_colorOutput51 = lerp(col_colorOutput51, color566, outline_colorOutput51);

        float4 colorOutput51_out = col_colorOutput51;

        return colorOutput51_out;
    }


#endif
