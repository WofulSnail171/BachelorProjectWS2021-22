
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Smooth Combine")]
public class SDFSmoothCombine : SDFFunction
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
        this.K = this.k;
    }

    private void Awake() {
        this.nodeType = NodeType.SmoothCombine;
        
        this.index = (uint)Random.Range(0, 1000);
       
        this.sdfName = "sCombine" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_k");
        this.types.Add("float");
    }
    public override string GenerateHlslFunction() {
        
        string hlslString = @"

    float h_" + this.sdfName + " = max( " + this.variables[0] +" - abs(" + this.inputA.o + " - " + this.inputB.o + @"), 0.0 )/" + this.variables[0] + @";
    float " + this.o + " =  min( " + this.inputA.o + ", " + this.inputB.o + ") - h_" + this.sdfName + "*h_" + this.sdfName + "*" + this.variables[0] +"*(1.0/4.0);";
        
        return hlslString;
    }
    
    public override void GetActiveNodes(List<SDFNode> activeNodes) {
        activeNodes.Add(this);
        
        this.GetActiveNodes(activeNodes, this.InputA);
        this.GetActiveNodes(activeNodes, this.InputB);
        
    }
}
