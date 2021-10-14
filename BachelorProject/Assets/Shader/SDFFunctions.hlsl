#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED

void SDFCircle_float(float r, float2 p, float2 uv,
    out float Out){
    
    Out = length(uv + p) - r;
}

void SDFRect_float(float2 b, float2 p, float2 uv, 
    out float Out){
    
    float2 d = abs(uv+ p) - b;
    Out = length(max(d, 0)) + min(max(d.x, d.y), 0);
}

void SDFLine_float(float2 a, float2 b, float2 uv, 
    out float Out){
    
    float2 pa = uv-a;
    float2 ba = b-a;
    float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
    Out = length(pa - ba*h);
}

//void SDFRect_float(out float Out){
//    
//}
    
#endif