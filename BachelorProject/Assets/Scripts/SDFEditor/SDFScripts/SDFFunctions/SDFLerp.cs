using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Lerp")]
public class SDFLerp : SDFFunction
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
    
    public float T {
        get => this._t;
        set {
            if (this._t == value) return;
            this._t = value;
            this.OnValueChange?.Invoke(this);
        }
    }

    private void OnValidate() {
        this.InputA = this.inputA;
        this.InputB = this.inputB;
        this.T = this.t;
    }
    
    private void Awake() {
        this.nodeType = NodeType.Lerp;
        
        this.index = (uint)Random.Range(0, 1000);
        
        this.sdfName = "lerp" + this.index;
        this.o = this.sdfName + "_out";
        
        this.OnInputChange += this.GenerateVariables;
        GenerateVariables();
        
    }
    
    public override string GenerateHlslFunction() {

        string a = this.inputA.GenerateHlslFunction();
        string b = this.inputB.GenerateHlslFunction();

        string hlslString = a +@"
    " + b + @"
    " + "float " + this.o + " = lerp(" + this.inputA.o + "," + this.inputB.o + ", " +  this.variables[0] + ");";
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
        
        this.variables.Clear();
        this.types.Clear();
        
        if (this._inputA == null || this.inputB == null) {
            Debug.LogWarning("cant generate shader. missing assigned node in " + this.name);
            return;}

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
    }

    private void OnDisable() {
        this.OnInputChange -= GenerateVariables;
    }
}
