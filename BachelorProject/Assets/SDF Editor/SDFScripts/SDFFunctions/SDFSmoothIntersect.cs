using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Smooth Intersect")]
public class SDFSmoothIntersect : SDFFunction
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
        this.nodeType = NodeType.SmoothIntersect;
        
        this.index = (uint)Random.Range(0, 1000);
       
        this.sdfName = "sIntersect" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_k");
        this.types.Add("float");
    }
    public override string GenerateHlslFunction() {

        string hlslString = @"

    float h_" + this.sdfName + " = clamp( 0.5 - 0.5*(" + this.inputA.o + "-" + this.inputB.o + ")/" + this.variables[0] + @", 0.0, 1.0 );
    float " + this.o + " = lerp( " + this.inputA.o + ", " + this.inputB.o + ", h_" + this.sdfName + " ) + " + this.variables[0] + "*h_" + this.sdfName + "*(1.0-h_" + this.sdfName + ");";
  
        return hlslString;
    }
    
    public override void GetActiveNodes(List<SDFNode> activeNodes) {
        activeNodes.Add(this);
        
        this.GetActiveNodes(activeNodes, this.InputA);
        this.GetActiveNodes(activeNodes, this.InputB);
        
    }
}
