using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDFIntersect : SDFFunction
{
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
        if (this.inputA != null && this.sdfName == this.inputA.sdfName) {
            Debug.LogWarning("invalid node in Input A");
            this.inputA = null;
            return;
        }

        if (this.inputB != null && this.sdfName == this.inputB.sdfName) {
            Debug.LogWarning("invalid node in Input B");
            this.inputB = null;
            return;
        }
        this.InputA = this.inputA;
        this.InputB = this.inputB;
    }

    private void Awake() {
        
        //Debug.Log("started awake for " + this.sdfName);
        this.nodeType = NodeType.Subtract;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "subtract" + this.index;
        this.o = this.sdfName +"_out";

    }

    public override string GenerateHlslFunction() {

        string hlslString = @"

        " + "float " + this.o + " = max(" + this.inputA.o + "," + this.inputB.o + @");";
        
        return hlslString;
    }

    public override void GetActiveNodes(List<SDFNode> activeNodes) {
        activeNodes.Add(this);
        
        this.GetActiveNodes(activeNodes, this.InputA);
        this.GetActiveNodes(activeNodes, this.InputB);
        
    }
}
