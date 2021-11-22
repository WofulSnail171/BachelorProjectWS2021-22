using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Smooth Blend")]
public class SDFSBLend : SDFFunction
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
            this.OnInputChange?.Invoke();
        }
    }
    
    public SDFNode InputB {
        get => this._inputB;
        set {
            if (this._inputB == value) return;
            this._inputB = value;
            this.OnInputChange?.Invoke();
        }
    }
    
    public float K {
        get => this._k;
        set {
            if (this._k == value) return;
            this._k = value;
            this.OnValueChange?.Invoke(this);
        }
    }

    private void OnValidate() {
        this.InputA = this.inputA;
        this.InputB = this.inputB;
        this.K = this.k;
    }

    private void Awake() {
        this.nodeType = NodeType.Lerp;
        
        this.index = (uint)Random.Range(0, 1000);
       
        this.sdfName = "sblend" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.OnInputChange += this.GenerateVariables;
        if (this.inputA != null || this._inputB != null) {
            this.GenerateVariables();
        }
    }
    public override string GenerateHlslFunction() {

        string hlslString = @"
    float h = max( " + this.variables[0] +" - abs(" + this.inputA + " - " + this.inputB + @"), 0.0 )/" + this.variables[0] + @";
    float this.o =  min( " + this.inputA + ", " + this.inputB + ") - h*h*" + this.variables[0] +"*(1.0/4.0);";
        
        return hlslString;
    }
    
    public override void GetActiveNodes(List<SDFNode> nodes) {
        nodes.Add(this);
        
        if (this.inputA is SDFFunction) {
            SDFFunction i = (SDFFunction) this.inputA;
            i.GetActiveNodes(nodes);
        }
        else {
            nodes.Add(this.inputA);
        }
        if (this.inputB is SDFFunction) {
            SDFFunction i = (SDFFunction) this.inputB;
            i.GetActiveNodes(nodes);
        }
        else {
            nodes.Add(this.inputB);
        }
    }
    
    public void GenerateVariables() {
        
        if (this.variables != null) {
            this.variables.Clear();
            this.types.Clear();
        }
        
        if (this._inputA == null || this.inputB == null) {
            Debug.LogWarning("cant generate shader. missing assigned node in " + this.name);
            return;}

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
    }

    private void OnDisable() {
        this.OnInputChange -= GenerateVariables;
    }
}
