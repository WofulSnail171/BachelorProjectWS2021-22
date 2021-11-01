using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Lerp")]
public class SDFLerp : SDFNode
{
    [SerializeField] private SDFNode inputA;
    [SerializeField] private SDFNode inputB;
    [SerializeField] private float t;
    public override string SDFFunction() {
        this.o = this.SDFName +"_out";
        
        string a = this.inputA.SDFFunction();
        string b = this.inputB.SDFFunction();
        
        string hlslString = a +@"
    " + b + @"
    " + "float " + this.o + " = lerp(" + this.inputA.o + "," + this.inputB.o + ", " +  t + ");";
        return hlslString;
    }
}
