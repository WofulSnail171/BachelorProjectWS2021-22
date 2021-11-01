using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SDF Function/Combine")]
public class SDFCombine : SDFNode {
    [SerializeField] private SDFNode inputA;
    [SerializeField] private SDFNode inputB;

    public override string SDFFunction() {
        this.o = this.SDFName +"_out";
        string a = this.inputA.SDFFunction();
        string b = this.inputB.SDFFunction();
        
        string hlslString = a +@"
    " + b + @"
    " + "float " + this.o + " = min(" + this.inputA.o + "," + this.inputB.o + ");";
        return hlslString;
    }
}
