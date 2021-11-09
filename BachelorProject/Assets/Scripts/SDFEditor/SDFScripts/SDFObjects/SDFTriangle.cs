using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Triangle")]
public class SDFTriangle : SDFScriptableObject {
    private Vector2 a;
    private Vector2 b;
    private Vector2 c;
    private float scale;
    
    public Vector2 A {
        get => this.a;
        set {
            if (this.a == value) return;
            this.a = value;
            //TODO: call OnChange event
        }
    }
    
    public Vector2 B {
        get => this.b;
        set {
            if (this.b == value) return;
            this.b = value;
            //TODO: call OnChange event
        }
    }
    
    public Vector2 C {
        get => this.c;
        set {
            if (this.c == value) return;
            this.c = value;
            //TODO: call OnChange event
        }
    }

    public float Scale {
        get => this.scale;
        set {
            if (this.scale == value) return;
            this.scale = value;
            //TODO: call OnChange event
        }
    }

    private void Awake() {
        this.index = (uint)Random.Range(0, 1000);
        Debug.Log("changed index from " + this.sdfName);
        
        this.sdfName = "triangle" + this.index;
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
        this.variables.Add(this.sdfName + "_scale");
        this.types.Add("float");
    }
    
    public override string SdfFunction() {

        string hlslString ="float2 e0 = " + this.variables[2] + " - " + this.variables[1] + @";
    float2 e1 = " + this.variables[3] + " - " + this.variables[2] + @";
    float2 e2 = " + this.variables[1] + " - " + this.variables[3] + @";
    
    float2 uv_" + this.sdfName + " = 1/" + this.variables[4] + " * uv - " + this.variables[0] + @";
    
    float2 v0 = uv_" + this.sdfName + " - " + this.variables[1] + @";
    float2 v1 = uv_" + this.sdfName + " - " + this.variables[2] + @";
    float2 v2 = uv_" + this.sdfName + " - " + this.variables[3] + @";
    
    float2 pq0 = v0 - e0 * clamp( dot(v0,e0)/dot(e0,e0), 0.0, 1.0 );
    float2 pq1 = v1 - e1 * clamp( dot(v1,e1)/dot(e1,e1), 0.0, 1.0 );
    float2 pq2 = v2 - e2 * clamp( dot(v2,e2)/dot(e2,e2), 0.0, 1.0 );
    
    float s = sign( e0.x*e2.y - e0.y*e2.x ) ;
    float2 d = min(min(float2(dot(pq0,pq0), s*(v0.x*e0.y-v0.y*e0.x)),
                       float2(dot(pq1,pq1), s*(v1.x*e1.y-v1.y*e1.x))),
                       float2(dot(pq2,pq2), s*(v2.x*e2.y-v2.y*e2.x)));
    float "+ this.o + "= -sqrt(d.x) * sign(d.y) * " + this.scale +";";
        
        return hlslString;
    }
}
