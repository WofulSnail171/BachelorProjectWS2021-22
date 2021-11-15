using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Invert")]
public class SDFInvert : SDFNode
{
    [SerializeField]private SDFNode input;
    private SDFNode _input;

    public SDFNode Input {
        get => this._input;
        set {
            if (this._input == value) return;
            this._input = value;
            this.isDirty = true;
        }
    }

    private void OnValidate() {
        this.Input = this.input;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }

    private void Awake() {
        this.nodeType = NodeType.Invert;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "invert";
        this.o = this.sdfName +"_out";
    }
    public override string SdfFunction() {
        
        string a = this.input.SdfFunction();
        
        this.variables.Clear();
        this.types.Clear();

        foreach (string s in this.input.variables) {
            this.variables.Add(s);
        }
        foreach (string s in this.input.types) {
            this.types.Add(s);
        }

        string hlslString = a +@"
    " + "float " + this.o + " = -" + this.input.o + ";";
        
        return hlslString;
    }
}
