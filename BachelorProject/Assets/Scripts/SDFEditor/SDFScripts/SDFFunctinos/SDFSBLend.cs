using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Smooth Blend")]
public class SDFSBLend : SDFNode
{
    [SerializeField] private SDFNode inputA;
    private SDFNode _inputA;
    
    [SerializeField] private SDFNode inputB;
    private SDFNode _inputB;
    
    [SerializeField] private float k;
    private float _k;


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
    
    public float K {
        get => this._k;
        set {
            if (this._k == value) return;
            this._k = value;
            this.isDirty = true;
        }
    }

    private void OnValidate() {
        this.InputA = this.inputA;
        this.InputB = this.inputB;
        this.K = this.k;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
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
