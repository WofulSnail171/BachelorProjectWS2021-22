#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED
float dot2( in float2 v ) { return dot(v,v); }

void SDF_float (float2 uv, out float Out){ 
    float2 d = abs((0.5, 0.5) - uv) - float2(0.1, 0.4);
    float rect714_out = length(max(d, 0)) + min(max(d.x, d.y), 0);

    Out = rect714_out;
}

#endif
