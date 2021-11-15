using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Bezier")]
public class SDFBezier : SDFObject
{
    [SerializeField] private Vector2 a;
    private Vector2 _a;
    
    [SerializeField] private Vector2 b;
    private Vector2 _b;
    
    [SerializeField] private Vector2 c;
    private Vector2 _c;

    public Vector2 A {
        get => this._a;
        set {
            if (this._a == value) return;
            this._a = value;
            this.isDirty = true;
        }
    }
    
    public Vector2 B {
        get => this._b;
        set {
            if (this._b == value) return;
            this._b = value;
            this.isDirty = true;
        }
    }
    
    public Vector2 C {
        get => this._c;
        set {
            if (this._c == value) return;
            this._c = value;
            this.isDirty = true;
        }
    }
    
    void OnValidate() {
        this.A = this.a;
        this.B = this.b;
        this.C = this.c;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }
    
    private void Awake() {
        this.nodeType = NodeType.BezierCurve;
        
        this.index = (uint)Random.Range(0, 1000);
       
        this.sdfName = "bezier" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_a");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_b");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_c");
        this.types.Add("float2");
    }
    
    public override string SdfFunction() {
        
        string hlslString = @"
    float2 pos = uv - " + this.variables[0] + @";
    float2 A = " + this.variables[2] + " - " + this.variables[1] + @";
    float2 B = " + this.variables[1] + " - 2.0 * " + this.variables[2] + " + " + this.variables[3] + @";
    float2 C =  A * 2.0;
    float2 D =  float2" + this.variables[1] + @" - pos;
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
