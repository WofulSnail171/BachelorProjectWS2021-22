using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Combine")]
public class SDFCombine : SDFNode {
    
    [SerializeField] private SDFNode inputA;
    private SDFNode _inputA;
    
    [SerializeField] private SDFNode inputB;
    private SDFNode _inputB;

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

    private void OnValidate() {
        this.InputA = this.inputA;
        this.InputB = this.inputB;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
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
