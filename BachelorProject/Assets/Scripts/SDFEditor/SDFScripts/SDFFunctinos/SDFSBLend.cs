using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Smooth Blend")]
public class SDFSBLend : SDFNode
{
    [SerializeField] private SDFScriptableObject inputA;
    [SerializeField] private SDFScriptableObject inputB;
    [SerializeField] private float k;
    
    private void Awake() {
        this.index = (uint)Random.Range(0, 1000);
        Debug.Log("changed index from " + this.sdfName);
        
        this.sdfName = "sblend" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_k");
        this.types.Add("float");
    }
    public override string SdfFunction() {

        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_k");
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

        string hlslString = @"
    float h = max( " + this.variables[0] +" - abs(" + this.inputA + " - " + this.inputB + @"), 0.0 )/" + this.variables[0] + @";
    float this.o =  min( " + this.inputA + ", " + this.inputB + ") - h*h*" + this.variables[0] +"*(1.0/4.0);";
        
        return hlslString;
    }
}
