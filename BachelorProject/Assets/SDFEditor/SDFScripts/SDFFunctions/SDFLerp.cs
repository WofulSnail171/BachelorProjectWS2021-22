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
        this.T = this.t;
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
    
    public override string GenerateHlslFunction() {

        string hlslString = @"

        " + "float " + this.o + " = lerp(" + this.inputA.o + "," + this.inputB.o + ", " +  this.variables[0] + ");";
        return hlslString;
    }
    
    public override void GetActiveNodes(List<SDFNode> activeNodes) {
        activeNodes.Add(this);
        
        this.GetActiveNodes(activeNodes, this.InputA);
        this.GetActiveNodes(activeNodes, this.InputB);
        
    }
}
