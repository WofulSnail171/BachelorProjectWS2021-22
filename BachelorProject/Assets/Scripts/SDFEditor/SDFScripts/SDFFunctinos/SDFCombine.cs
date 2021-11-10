using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Combine")]
public class SDFCombine : SDFNode {
    public SDFNode inputA;
    public SDFNode inputB;

    public SDFNode InputA {
        get => this.inputA;
        set {
            if (this.InputA == value) return;
            this.InputA = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    public SDFNode InputB {
        get => this.inputB;
        set {
            if (this.InputB == value) return;
            this.InputB = value;
            this.OnValueChange?.Invoke(this);
        }
    }

    private void OnValidate() {
        if (this.InputA != this.inputA) this.InputA = this.inputA;
        if (this.InputB != this.inputB) this.InputB = this.inputB;
    }

    private void Awake() {
        this.nodeType = NodeType.Comb;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "comb";
        this.o = this.sdfName +"_out";
    }

    public override string SdfFunction() {

        string a = this.inputA.SdfFunction();
        string b = this.inputB.SdfFunction();
        
        this.variables.Clear();
        this.types.Clear();

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
    " + "float " + this.o + " = min(" + this.inputA.o + "," + this.inputB.o + ");";
        
        return hlslString;
    }
}
