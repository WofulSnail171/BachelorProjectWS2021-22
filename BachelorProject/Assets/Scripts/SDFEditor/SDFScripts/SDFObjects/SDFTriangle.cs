using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF/Triangle")]
public class SDFTriangle : SDFObject {
    [SerializeField] private Vector2 a;
    private Vector2 _a;
    
    [SerializeField] private Vector2 b;
    private Vector2 _b;
    
    [SerializeField] private Vector2 c;
    private Vector2 _c;
    
    [SerializeField] private float scale;
    private float _scale;
    
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

    public float Scale {
        get => this._scale;
        set {
            if (this._scale == value) return;
            this._scale = value;
            this.isDirty = true;
        }
    }

    private void OnValidate() {
        this.Position = this.position;
        this.A = this.a;
        this.B = this.b;
        this.C = this.c;
        this.Scale = this.scale;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }

    private void Awake() {
        this.nodeType = NodeType.Triangle;
        
        this.index = (uint)Random.Range(0, 1000);
     
        this.sdfName = "triangle" + this.index;
        this.o = this.sdfName + "_out";
        
        if (this.variables != null) {
            this.variables.Clear();
            this.types.Clear();
        }
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_a");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_b");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_c");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_scale");
        this.types.Add("float");
    }
    
    public override string GenerateHlslFunction() {

        string hlslString =@"
    float2 e0_" + this.sdfName + " = " + this.variables[2] + " - " + this.variables[1] + @";
    float2 e1_" + this.sdfName + " = " + this.variables[3] + " - " + this.variables[2] + @";
    float2 e2_" + this.sdfName + " = " + this.variables[1] + " - " + this.variables[3] + @";
    
    float2 uv_" + this.sdfName + " = 1/" + this.variables[4] + " * uv - " + this.variables[0] + @";
    
    float2 v0_" + this.sdfName + " = uv_" + this.sdfName + " - " + this.variables[1] + @";
    float2 v1_" + this.sdfName + " = uv_" + this.sdfName + " - " + this.variables[2] + @";
    float2 v2_" + this.sdfName + " = uv_" + this.sdfName + " - " + this.variables[3] + @";
    
    float2 pq0_" + this.sdfName + " = v0_" + this.sdfName + " - e0_" + this.sdfName + " * clamp( dot(v0_" + this.sdfName + ",e0_" + this.sdfName + ")/dot(e0_" + this.sdfName + ",e0_" + this.sdfName + @"), 0.0, 1.0 );
    float2 pq1_" + this.sdfName + " = v1_" + this.sdfName + " - e1_" + this.sdfName + " * clamp( dot(v1_" + this.sdfName + ",e1_" + this.sdfName + ")/dot(e1_" + this.sdfName + ",e1_" + this.sdfName + @"), 0.0, 1.0 );
    float2 pq2_" + this.sdfName + " = v2_" + this.sdfName + " - e2_" + this.sdfName + " * clamp( dot(v2_" + this.sdfName + ",e2_" + this.sdfName + ")/dot(e2_" + this.sdfName + ",e2_" + this.sdfName + @"), 0.0, 1.0 );
    
    float s_" + this.sdfName + " = sign( e0_" + this.sdfName + ".x*e2_" + this.sdfName + ".y - e0_" + this.sdfName + ".y*e2_" + this.sdfName + @".x ) ;
    float2 d_" + this.sdfName + " = min(min(float2(dot(pq0_" + this.sdfName + ",pq0_" + this.sdfName + "), s_" + this.sdfName + "*(v0_" + this.sdfName + ".x*e0_" + this.sdfName + ".y-v0_" + this.sdfName + ".y*e0_" + this.sdfName + @".x)),
                       float2(dot(pq1_" + this.sdfName + ",pq1_" + this.sdfName + "), s_" + this.sdfName + "*(v1_" + this.sdfName + ".x*e1_" + this.sdfName + ".y-v1_" + this.sdfName + ".y*e1_" + this.sdfName + @".x))),
                       float2(dot(pq2_" + this.sdfName + ",pq2_" + this.sdfName + "), s_" + this.sdfName + "*(v2_" + this.sdfName + ".x*e2_" + this.sdfName + ".y-v2_" + this.sdfName + ".y*e2_" + this.sdfName + @".x)));
    float "+ this.o + "= -sqrt(d_" + this.sdfName + ".x) * sign(d_" + this.sdfName + ".y) * " + this.variables[4] +";";
        
        return hlslString;
    }
}
