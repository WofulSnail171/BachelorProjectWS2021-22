using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Invert")]
public class SDFInvert : SDFNode
{
    [SerializeField] private SDFScriptableObject inputA;
    public override string SDFFunction() {
        
        this.sdfName = "invert" + this.index;
        this.o = this.sdfName + "_out";
        
        string a = this.inputA.SDFFunction();
        
        this.variables.Clear();
        this.types.Clear();

        foreach (string s in this.inputA.variables) {
            this.variables.Add(s);
        }
        foreach (string s in this.inputA.types) {
            this.types.Add(s);
        }

        string hlslString = a +@"
    " + "float " + this.o + " = -" + this.inputA.o + ";";
        
        return hlslString;
    }
}
