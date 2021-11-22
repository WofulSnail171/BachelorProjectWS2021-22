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
    
    public float Roundness {
        get => this._roundness;
        set {
            if (this._roundness == value) return;
            this._roundness = value;
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
        this.Roundness = this.roundness;
        this.Scale = this.scale;
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

        Debug.Log("changed index from line to: " + this.index);

        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_a");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_b");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_roundness");
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_scale");
        this.types.Add("float");
    }
    
    public override string GenerateHlslFunction() {
        
        string hlslString = @"
        float2 pa_" + this.sdfName + " = uv * 1/ " + this.variables[4] + " - " + this.variables[0] + " - " + this.variables[1] + @";
        float2 ba_" + this.sdfName + " = " + this.variables[2] + " - " + this.variables[1] + @";
        float h_" + this.sdfName + " = clamp(dot(pa_" + this.sdfName + ", ba_" + this.sdfName + ")/dot(ba_" + this.sdfName + ", ba_" + this.sdfName + @"), 0, 1);
        float "+ this.o + " = (length(pa_" + this.sdfName + " - ba_" + this.sdfName + "*h_" + this.sdfName + ") - " + this.variables[3]+ ") * " + this.variables[4] +";";
        
        return hlslString;
    }
}
