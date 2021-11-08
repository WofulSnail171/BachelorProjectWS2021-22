using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Invert")]
public class SDFInvert : SDFNode
{
    [SerializeField] private SDFScriptableObject inputA;
    
    private void Awake() {
        this.index = (uint)Random.Range(0, 1000);
        Debug.Log("changed index from " + this.sdfName);
        
        this.sdfName = "invert";
        this.o = this.sdfName +"_out";
    }
    public override string SDFFunction() {
        
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
