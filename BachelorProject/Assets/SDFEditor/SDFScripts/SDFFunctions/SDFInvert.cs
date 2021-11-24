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
    
    public override void GetActiveNodes(List<SDFNode> nodes) {
        nodes.Add(this);
        
        if (this.input is SDFFunction) {
            SDFFunction i = (SDFFunction) this.input;
            i.GetActiveNodes(nodes);
        }
        else {
            bool d;
            foreach (SDFNode s in nodes) {
                if (s.sdfName == this.input.sdfName) {
                    Debug.Log("found double in input");
                    return;
                }
            }
            nodes.Add(this.input);
            Debug.Log("found no double in input");
        }
    }
}
