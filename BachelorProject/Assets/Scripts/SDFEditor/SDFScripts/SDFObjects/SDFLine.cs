using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Line")]
public class SDFLine : SDFObject {
    public Vector2 a;
    public Vector2 b;
    public float roundness;

    public Vector2 A {
        get => this.a;
        set {
            if (this.a == value) return;
            this.a = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    public Vector2 B {
        get => this.b;
        set {
            if (this.b == value) return;
            this.b = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    public float Roundness {
        get => this.roundness;
        set {
            if (this.roundness == value) return;
            this.roundness = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    private void OnValidate() {
        if (this.A != this.a) this.A = this.a;
        if (this.B != this.b) this.B = this.b;
        if (this.Roundness != this.roundness) this.Roundness = this.roundness;
    }
    
    private void Awake() {
        this.nodeType = NodeType.Line;
        
        this.index = (uint)Random.Range(0, 1000);
 
        this.sdfName = "line" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_a");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_b");
        this.types.Add(this.sdfName + "_roundness");
        this.types.Add("float2");
    }
    
    public override string SdfFunction() {

        string hlslString = @"
        float2 pa = uv - " + this.variables[0] + " - " + this.variables[1] + @";
        float2 ba = " + this.variables[2] + " - " + this.variables[1] + @";
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float "+ this.o + " = length(pa - ba*h) - " + this.variables[3]+ ";";
        
        return hlslString;
    }
}
