#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED
float dot2( in float2 v ) { return dot(v,v); }

void SDF_float (float2 uv, float2 rect359_position, float2 rect359_box, float rect359_scale, float4 rect359_roundness, out float Out){ 
    rect359_roundness.xy = (rect359_position.x - uv.x > 0.0) ? rect359_roundness.xy : rect359_roundness.zw;
    rect359_roundness.x  = (rect359_position.y - uv.y > 0.0) ? rect359_roundness.x  : rect359_roundness.y;
    float2 q = abs(rect359_position - uv) - rect359_box + rect359_roundness.x;
    float rect359_out = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - rect359_roundness.x;

    Out = rect359_out;
}

#endif
