using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Invert")]
public class SDFInvert : SDFNode
{
    [SerializeField] private SDFScriptableObject inputA;
    public override string SDFFunction() {
        this.o = this.SDFName +"_out";
        string a = this.inputA.SDFFunction();
        string hlslString = a +@"
    " + "float " + this.o + " = -" + this.inputA.o + ";";
        return hlslString;
    }
}
