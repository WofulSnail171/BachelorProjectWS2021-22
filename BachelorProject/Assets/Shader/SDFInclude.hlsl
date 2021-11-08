#ifndef SDFFUNCTIONS_INCLUDED
#define SDFFUNCTIONS_INCLUDED
float dot2( in float2 v ) { return dot(v,v); }

void SDF_float (float2 uv, float2 rect884_position, float2 rect884_box, float rect884_scale, float4 rect884_roundness, out float Out){ 
    rect884_roundness.xy = (rect884_position.x - uv.x > 0.0) ? rect884_roundness.xy : rect884_roundness.zw;
    rect884_roundness.x  = (rect884_position.y - uv.y > 0.0) ? rect884_roundness.x  : rect884_roundness.y;
    float2 q = abs(rect884_position - uv) - rect884_box + rect884_roundness.x;
    float rect884_out = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - rect884_roundness.x;

    Out = rect884_out;
}
#endif