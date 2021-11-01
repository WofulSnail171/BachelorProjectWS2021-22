#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED
float dot2( in float2 v ) { return dot(v,v); }

void SDF_float (float2 uv, out float Out){ 
    
        float2 pa = uv - float2(0.0, 0.0) - float2(0.5, 0.2);
        float2 ba = float2(0.5, 0.8) - float2(0.5, 0.2);
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float newSDF655_out = length(pa - ba*h) - 0.2;

    Out = newSDF655_out;
}

#endif
