using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Smooth Blend")]
public class SDFSBLend : SDFNode
{
    public SDFNode inputA;
    public SDFNode inputB;
    public float k;

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
    
    public float K {
        get => this.k;
        set {
            if (this.k == value) return;
            this.k = value;
            this.OnValueChange?.Invoke(this);
        }
    }

    private void OnValidate() {
        if (this.InputA == this.inputA) this.InputA = this.inputA;
        if (this.InputB == this.inputB) this.InputB = this.inputB;
        if (this.K == this.k) this.K = this.k;
    }

    private void Awake() {
        this.nodeType = NodeType.Lerp;
        
        this.index = (uint)Random.Range(0, 1000);
       
        this.sdfName = "sblend" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_k");
        this.types.Add("float");
    }
    public override string SdfFunction() {

        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_k");
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

        string hlslString = @"
    float h = max( " + this.variables[0] +" - abs(" + this.inputA + " - " + this.inputB + @"), 0.0 )/" + this.variables[0] + @";
    float this.o =  min( " + this.inputA + ", " + this.inputB + ") - h*h*" + this.variables[0] +"*(1.0/4.0);";
        
        return hlslString;
    }
}
