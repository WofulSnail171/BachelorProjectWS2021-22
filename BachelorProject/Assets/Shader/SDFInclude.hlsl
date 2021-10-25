#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED
float dot2( in float2 v ) { return dot(v,v); }
void SDF_float (float2 uv,
             out float Out)
            { Out = length((-0.5, -0.5)+ uv)- 0.5;}
#endif
