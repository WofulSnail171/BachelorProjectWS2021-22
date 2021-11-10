using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF/Rectangle")]
public class SDFRectangle : SDFObject
{
    public Vector2 box = new Vector2(0,0);
    public float scale = 1;
    public Vector4 roundness = new Vector4(0, 0, 0, 0);
    
    public Vector2 Box {
        get => this.box;
        set {
            if (this.box == value) return;
            this.box = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    public float Scale {
        get => this.scale;
        set {
            if (this.scale == value) return;
            this.scale = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    public Vector4 Roundness {
        get => this.roundness;
        set {
            if (this.roundness == value) return;
            this.roundness = value;
            this.OnValueChange?.Invoke(this);
        }
    }

    private void OnValidate() {
        if (this.Box == this.box) this.Box = this.box;
        if (this.Scale == this.scale) this.Scale = this.scale;
        if (this.Roundness == this.roundness) this.Roundness = this.roundness;
    }

    private void Awake() {
        this.nodeType = NodeType.Rect;
        
        this.index = (uint)Random.Range(0, 1000);
   
        this.sdfName = "rect" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_box");
        this.types.Add("float2");
        this.variables.Add( this.sdfName + "_scale");
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_roundness");
        this.types.Add("float4");
    }
    public override string SdfFunction() {

        Vector2 b = this.box * this.scale;

        string hlslString = this.variables[3] + ".xy = (" + this.variables[0] + ".x - uv.x > 0.0) ? " + this.variables[3] + ".xy : " + this.variables[3] + @".zw;
        " + this.variables[3] + ".x  = (" + this.variables[0] + ".y - uv.y > 0.0) ? " + this.variables[3] + ".x  : " + this.variables[3] + @".y;
        float2 q = abs(" + this.variables[0] + " - uv) - " + this.variables[1] + " + " + this.variables[3] + @".x;
        float " + this.o + " = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - " + this.variables[3] + ".x;";
        
        return hlslString;
    }
}