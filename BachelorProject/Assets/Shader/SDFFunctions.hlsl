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

void SDFBezier_float(float2 A, float2 B, float2 C, float2 UV, out float Out){
    float2 pos = UV;
    float2 a = B - A;
    float2 b = A - 2.0*B + C;
    float2 c = a * 2.0;
    float2 d = A - pos;
    float kk = 1.0/dot(b,b);
    float kx = kk * dot(a,b);
    float ky = kk * (2.0*dot(a,a)+dot(d,b)) / 3.0;
    float kz = kk * dot(d,a);      
    float res = 0.0;
    float p = ky - kx*kx;
    float p3 = p*p*p;
    float q = kx*(2.0*kx*kx-3.0*ky) + kz;
    float h = q*q + 4.0*p3;
    if( h >= 0.0) 
    { 
        h = sqrt(h);
        float2 x = (float2(h,-h)-q)/2.0;
        float2 uv = sign(x)*pow(abs(x), float2(1.0/3.0,0));
        float t = clamp( uv.x+uv.y-kx, 0.0, 1.0 );
        float2 sum = d + (c + b*t)*t;
        res = dot(sum, sum);
    }
    else
    {
        float z = sqrt(-p);
        float v = acos( q/(p*z*2.0) ) / 3.0;
        float m = cos(v);
        float n = sin(v)*1.732050808;
        float3  t = clamp(float3(m+m,-n-m,n-m)*z-kx,0.0,1.0);
        
        float2 sum1 = d+(c+b*t.x)*t.x;
        float2 sum2 = d+(c+b*t.y)*t.y;
        
        res = min( dot(sum1, sum1),
                   dot(sum2, sum2) );
        // the third root cannot be the closest
        // res = min(res,dot2(d+(c+b*t.z)*t.z));
    }
    Out = sqrt(res);
}
    
#endif