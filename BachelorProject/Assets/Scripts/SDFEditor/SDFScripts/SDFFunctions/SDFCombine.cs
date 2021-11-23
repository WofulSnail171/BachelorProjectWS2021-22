using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Combine")]
public class SDFCombine : SDFFunction {
    
    [SerializeField] private SDFNode inputA;
    private SDFNode _inputA;
    
    [SerializeField] private SDFNode inputB;
    private SDFNode _inputB;

    public SDFNode InputA {
        get => this._inputA;
        set {
            if (this._inputA == value) return;
            this._inputA = value;
            this.OnInputChange?.Invoke();
            Debug.Log("input A has changed");
        }
    }
    
    public SDFNode InputB {
        get => this._inputB;
        set {
            if (this._inputB == value) return;
            this._inputB = value;
            this.OnInputChange?.Invoke();
            Debug.Log("input B has changed");
        }
    }

    private void OnValidate() {
        this.InputA = this.inputA;
        this.InputB = this.inputB;
    }

    private void Awake() {
        
        //Debug.Log("started awake for " + this.sdfName);
        this.nodeType = NodeType.Comb;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "comb";
        this.o = this.sdfName +"_out";
        
        this.OnInputChange += this.GenerateVariables;
        if (this._inputA != null || this._inputB != null) {
            this.GenerateVariables();
        }
        else {
            Debug.LogWarning("cant generate shader. missing assigned node in " + this.sdfName);
        }
       // Debug.Log("awake done for " + this.sdfName);
        
    }

    public override string GenerateHlslFunction() {

        string a = this.inputA.GenerateHlslFunction();
        string b = this.inputB.GenerateHlslFunction();

        string hlslString = a +@"
    " + b + @"

    " + "float " + this.o + " = min(" + this.inputA.o + "," + this.inputB.o + @");
";
        
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


        //string debugNodes = "";
        //foreach (var s in nodes) {
        //    if (s != null) {
        //        debugNodes += s.sdfName + " - ";
        //    }
        //    else {
        //        Debug.Log("node is null");
        //    }
        //}
        //
        //Debug.Log(debugNodes);
    }
    
    public void GenerateVariables() {
        
        //Debug.Log("generating new variables");

        if (this._inputA == null || this._inputB == null) {
            Debug.LogWarning("cant generate shader. missing assigned node in " + this.sdfName);
            return;
        }

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
        
        // Debug.Log("comb variables: " + this.variables);
    }

    private void OnDisable() {
        this.OnInputChange -= GenerateVariables;
    }
}
