using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SDF Function/Combine")]
public class SDFCombine : SDFNode {
    [SerializeField] private SDFNode inputA;
    [SerializeField] private SDFNode inputB;

    public override string SDFFunction() {
        
        this.sdfName = "comb";
        this.o = this.sdfName +"_out";
        
        string a = this.inputA.SDFFunction();
        string b = this.inputB.SDFFunction();
        
        this.variables.Clear();
        this.types.Clear();

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
            
        string hlslString = a +@"
    " + b + @"
    " + "float " + this.o + " = min(" + this.inputA.o + "," + this.inputB.o + ");";
        
        return hlslString;
    }
}
