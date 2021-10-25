#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED
float dot2( in float2 v ) { return dot(v,v); }
void SDF_float (float2 uv,
             out float Out){ 
                float Circle_out = length((0.5, 0.5)- uv)- 0.2;
float2 d = abs((0.5, 0.5)-uv) -(0.2, 0.4);
        float Rectangle_out = length(max(d, 0)) + min(max(d.x, d.y), 0);
float c_out= min(Circle_out,Rectangle_out);
                Out =c_out; }
#endif
