using System;
using System.Collections.Generic;
using UnityEngine;

public class SDFColorOutput : SDFColorNode
{
    [SerializeField] private SDFColorInput insideColor;
    private SDFColorInput _insideColor;
    
    [SerializeField] private SDFColorInput outsideColor;
    private SDFColorInput _outsideColor;
    
    [Header ("Outline")]
    [SerializeField] private SDFColorInput outlineColor;
    private SDFColorInput _outlineColor;

    [SerializeField] private float thickness;
    private float _thickness;

    [SerializeField] private int repetition;
    private int _repetition;

    [SerializeField] private float lineDistance;
    private float _lineDistance;

    public SDFColorInput InsideColor {
        get => this._insideColor;
        set {
            if (this._insideColor == value) return;
            this._insideColor = value;
            this.OnInputChange?.Invoke();
        }
    }
    
    public SDFColorInput OutsideColor {
        get => this._outsideColor;
        set {
            if (this._outsideColor == value) return;
            this._outsideColor = value;
            this.OnInputChange?.Invoke();
        }
    }
    
    public SDFColorInput OutlineColor {
        get => this._outlineColor;
        set {
            if (this._outlineColor == value) return;
            this._outlineColor = value;
            this.OnInputChange?.Invoke();
        }
    }
    
    public float Thickness {
        get => this._thickness;
        set {
            if (this._thickness == value) return;
            this._thickness = value;
            this.isDirty = true;
        }
    }
    
    public int Repetition {
        get => this._repetition;
        set {
            if (this._repetition == value) return;
            this._repetition = value;
            this.isDirty = true;
        }
    }
    
    public float LineDistance {
        get => this._lineDistance;
        set {
            if (this._lineDistance == value) return;
            this._lineDistance = value;
            this.isDirty = true;
        }
    }
    
    private void OnValidate() {
        this.InsideColor = this.insideColor;
        this.OutsideColor = this.outsideColor;
        this.OutlineColor = this.outlineColor;
        this.Thickness = this.thickness;
        this.Repetition = this.repetition;
        this.LineDistance = this.lineDistance;
        
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }

    private void Awake() {
        this.nodeType = NodeType.ColorOutput;
        
        this.index = (uint)UnityEngine.Random.Range(0, 1000);
        
        this.sdfName = "colorOutput" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_thickness");
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_repetition");
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_lineDistance");
        this.types.Add("float");

    }

    public override string GenerateHlslFunction() {
        string iColor = "";
        string oColor = "";
        string olColor = "";
        
        if (this.InsideColor == null) {
            iColor = "float4(1,1,1,1)";
        }
        else {
            iColor = this.InsideColor.o;
        }
        if (this.OutsideColor == null) {
            oColor = "float4(0.5,0.5,0.5,0)";
        }
        else {
            oColor = this.OutsideColor.o;
        }
        
        if (this.OutlineColor == null) {
            olColor = "float4(0,0,0,1)";
        }
        else {
            olColor = this.OutlineColor.o;
        }

        string hlslString = @"
        
        float sdf = smoothstep(0,thickness*0.01 - thickness*0.005 ,sdfOut);
            float4 col = lerp(insideColor, outsideColor, sdf);
            float outline = 1-smoothstep(0,thickness*0.01 ,abs(frac(sdfOut / (lineDistance*0.1) + 0.5) - 0.5) * (lineDistance*0.1));
            outline *= step(sdfOut-repetition *0.01, 0);
            col = lerp(col, outlineColor, outline);

        " + "float " + this.o + " = ;";
        return hlslString;

    }

    public void GetActiveNodes(List<SDFColorNode> activeNodes) {
        activeNodes.Add(this);
        
        if(!this.InsideColor)
            this.GetActiveNodes(activeNodes, this.InsideColor);
        if(!this.OutsideColor)
            this.GetActiveNodes(activeNodes, this.OutsideColor);
        if(!this.OutlineColor)
            this.GetActiveNodes(activeNodes, this.OutlineColor);
        
    }
    
}
