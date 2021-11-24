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

        this.OnInputChange += this.GenerateVariables;
        GenerateVariables();

    }
    public override string GenerateHlslFunction() {
        
        string a = this.input.GenerateHlslFunction();

        string hlslString = a +@"
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
            nodes.Add(this.input);
        }
    }

    public override void GenerateVariables() {
        
        if (this.variables != null) {
            this.variables.Clear();
            this.types.Clear();
        }
        
        if (this.input == null) {
            Debug.LogWarning("cant generate shader. missing assigned node in " + this.name);
            return;
        }

        foreach (string s in this.input.variables) {
            this.variables.Add(s);
        }
        foreach (string s in this.input.types) {
            this.types.Add(s);
        }
    }

    private void OnDisable() {
        this.OnInputChange -= GenerateVariables;
    }
}
