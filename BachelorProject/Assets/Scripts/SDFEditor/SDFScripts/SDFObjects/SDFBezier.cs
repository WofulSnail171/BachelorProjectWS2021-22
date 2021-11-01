using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Bezier")]
public class SDFBezier : SDFScriptableObject
{
    [SerializeField] private Vector2 a;
    [SerializeField] private Vector2 b;
    [SerializeField] private Vector2 c;
    
    public override string SDFFunction() {
        
        this.o = this.SDFName + "_out";
        
        string hlslString = @"
    float2 pos = uv - float2" + this.Position + @";
    float2 A = float2" + this.b + " - float2" + this.a + @";
    float2 B = float2" + this.a + " - 2.0 * float2" + this.b + " + float2" + this.c + @";
    float2 C =  A * 2.0;
    float2 D =  float2" + this.a + @" - pos;
    float kk = 1.0/dot(B, B);
    float kx = kk * dot(A, B);
    float ky = kk * (2.0*dot(A, A)+dot(D, B)) / 3.0;
    float kz = kk * dot(D, A);      
    float res = 0.0;
    float p = ky - kx * kx;
    float p3 = p * p * p;
    float q = kx * (2.0 * kx * kx - 3.0 * ky) + kz;
    float h = q * q + 4.0 * p3;
    if( h >= 0.0) 
    { 
        h = sqrt(h);
        float2 x = (float2(h,-h)-q)/2.0;
        float2 uv = sign(x) * pow(abs(x), float2(1.0/3.0,1.0/3.0));
        float t = clamp( uv.x+uv.y-kx, 0.0, 1.0 );
        res = dot2(D + (C + B * t) * t);
    }
    else
    {
        float z = sqrt(-p);
        float v = acos( q/(p * z * 2.0) ) / 3.0;
        float m = cos(v);
        float n = sin(v) * 1.732050808;
        float3  t = clamp(float3(m + m,-n - m, n - m) * z - kx,0.0,1.0);
    
        res = min( dot2(D + (C + B * t.x) * t.x),
            dot2(D + (C + B * t.y) * t.y) );
    }
    float " + this.o + " = sqrt(res);";

        return hlslString;
    }
}
