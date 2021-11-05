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
        
        this.sdfName = "lerp" + this.index;
        this.o = this.sdfName + "_out";
        
        string a = this.inputA.SDFFunction();
        string b = this.inputB.SDFFunction();
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_t");
        this.types.Add("float");

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
    " + "float " + this.o + " = lerp(" + this.inputA.o + "," + this.inputB.o + ", " +  this.variables[0] + ");";
        return hlslString;
    }
}
