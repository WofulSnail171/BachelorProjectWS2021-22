using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SDFCombine : ScriptableObject {
    [SerializeField] private SDFScriptableObject inputA;
    [SerializeField] private SDFScriptableObject inputB;
    [SerializeField] private new string name = "newName";

    [HideInInspector]public string o;

    public string Combine() {
        this.o = this.name +"_out";
        string a = this.inputA.SDFFunction();
        string b = this.inputB.SDFFunction();
        
        string hlslString = a + System.Environment.NewLine + b + System.Environment.NewLine + "float " + this.o + "= min(" + this.inputA.o + "," + this.inputB.o + ");";
        return hlslString;
    }
}
