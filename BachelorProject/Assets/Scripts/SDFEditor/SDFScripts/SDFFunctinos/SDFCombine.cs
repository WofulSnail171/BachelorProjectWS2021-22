using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SDF Function/Combine")]
public class SDFCombine : SDFNode {
    [SerializeField] private SDFNode inputA;
    [SerializeField] private SDFNode inputB;
    
    private void Awake() {
        this.index = (uint)Random.Range(0, 1000);
        Debug.Log("changed index from " + this.sdfName);
        
        this.sdfName = "comb";
        this.o = this.sdfName +"_out";
    }

    public override string SdfFunction() {

        string a = this.inputA.SdfFunction();
        string b = this.inputB.SdfFunction();
        
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
