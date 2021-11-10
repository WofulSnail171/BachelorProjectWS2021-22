using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Lerp")]
public class SDFLerp : SDFNode
{
    public SDFNode inputA;
    public SDFNode inputB;
    public float t;
    
    public SDFNode InputA {
        get => this.inputA;
        set {
            if (this.inputA == value) return;
            this.inputA = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    public SDFNode InputB {
        get => this.inputB;
        set {
            if (this.inputB == value) return;
            this.inputB = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    public float T {
        get => this.t;
        set {
            if (this.t == value) return;
            this.t = value;
            this.OnValueChange?.Invoke(this);
        }
    }

    private void OnValidate() {
        if (this.InputA != this.inputA) this.InputA = this.inputA;
        if (this.InputB != this.inputB) this.InputB = this.inputB;
        if (this.T != this.t) this.T = this.t;
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
