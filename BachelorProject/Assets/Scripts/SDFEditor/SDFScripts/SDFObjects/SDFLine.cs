using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Line")]
public class SDFLine : SDFObject {
    [SerializeField] private Vector2 a;
    private Vector2 _a;
    
    [SerializeField] private Vector2 b;
    private Vector2 _b;
    
    [SerializeField] private float roundness;
    private float _roundness;
    
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
    
    public float Roundness {
        get => this._roundness;
        set {
            if (this._roundness == value) return;
            this._roundness = value;
            this.isDirty = true;
        }
    }
    
    private void OnValidate() {
        this.Position = this.position;
        this.A = this.a;
        this.B = this.b;
        this.Roundness = this.roundness;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }
    
    private void Awake() {
        this.nodeType = NodeType.Line;
        
        this.index = (uint)Random.Range(0, 1000);
 
        this.sdfName = "line" + this.index;
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
        this.variables.Add(this.sdfName + "_roundness");
        this.types.Add("float");
    }
    
    public override string GenerateHlslFunction() {

        string hlslString = @"
        float2 pa = uv - " + this.variables[0] + " - " + this.variables[1] + @";
        float2 ba = " + this.variables[2] + " - " + this.variables[1] + @";
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float "+ this.o + " = length(pa - ba*h) - " + this.variables[3]+ ";";
        
        return hlslString;
    }
}
