using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF Function/Invert")]
public class SDFInvert : SDFNode
{
    public SDFNode input;

    public SDFNode Input {
        get => this.input;
        set {
            if (this.Input == value) return;
            this.input = value;
            this.OnValueChange?.Invoke(this);
        }
    }

    private void OnValidate() {
        if (this.Input != this.input) this.Input = this.Input;
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
