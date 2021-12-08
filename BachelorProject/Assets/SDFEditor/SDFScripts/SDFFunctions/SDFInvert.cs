using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Invert")]
public class SDFInvert : SDFFunction
{
    [SerializeField]private SDFNode input;
    private SDFNode _input;

    public SDFNode Input {
        get => this._input;
        set {
            if (this._input == value) return;
            this._input = value;
            this.OnInputChange?.Invoke();
        }
    }

    private void OnValidate() {
        if (this.input != null && this.sdfName == this.input.sdfName) {
            Debug.LogWarning("invalid node in Input");
            this.input = null;
            return;
        }
        
        this.Input = this.input;
    }

    private void Awake() {
        
        this.nodeType = NodeType.Invert;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "invert";
        this.o = this.sdfName +"_out";
    }
    public override string GenerateHlslFunction() {
        
        string hlslString = @"

    " + "float " + this.o + " = -" + this.input.o + ";";
        
        return hlslString;
    }
    
    public override void GetActiveNodes(List<SDFNode> activeNodes) {
        activeNodes.Add(this);
        
        this.GetActiveNodes(activeNodes, this.Input);

    }
}
