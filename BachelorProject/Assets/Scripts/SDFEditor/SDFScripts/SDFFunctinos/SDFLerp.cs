using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Lerp")]
public class SDFLerp : SDFNode
{
    [SerializeField] private SDFNode inputA;
    private SDFNode _inputA;
    
    [SerializeField] private SDFNode inputB;
    private SDFNode _inputB;
    
    [SerializeField] private float t;
    private float _t;
    
    
    public SDFNode InputA {
        get => this._inputA;
        set {
            if (this._inputA == value) return;
            this._inputA = value;
            this.isDirty = true;
        }
    }
    
    public SDFNode InputB {
        get => this._inputB;
        set {
            if (this._inputB == value) return;
            this._inputB = value;
            this.isDirty = true;
        }
    }
    
    public float T {
        get => this._t;
        set {
            if (this._t == value) return;
            this._t = value;
            this.isDirty = true;
        }
    }

    private void OnValidate() {
        this.InputA = this.inputA;
        this.InputB = this.inputB;
        this.T = this.t;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }
    
    private void Awake() {
        this.nodeType = NodeType.Lerp;
        
        this.index = (uint)Random.Range(0, 1000);
        
        this.sdfName = "lerp" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_t");
        this.types.Add("float");
    }
    
    public override string SdfFunction() {

        string a = this.inputA.SdfFunction();
        string b = this.inputB.SdfFunction();
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_t");
        this.types.Add("float");

        foreach (string s in this.inputA.variables) {
            this.variables.Add(s);
        }
        foreach (string s in this.inputB.variables) {
            this.variables.Add(s);
        }
        foreach (string s in this.inputA.types) {
            this.types.Add(s);
        }

        foreach (string s in this.inputB.types) {
            this.types.Add(s);
        }

        string hlslString = a +@"
    " + b + @"
    " + "float " + this.o + " = lerp(" + this.inputA.o + "," + this.inputB.o + ", " +  this.variables[0] + ");";
        return hlslString;
    }
}
