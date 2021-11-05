using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Line")]
public class SDFLine : SDFScriptableObject {
    [SerializeField] private Vector2 a;
    [SerializeField] private Vector2 b;
    [SerializeField] private float roundness;
    
    public override string SDFFunction() {
        
        this.sdfName = "line" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_a");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_b");
        this.types.Add(this.sdfName + "_roundness");
        this.types.Add("float2");
        
        string hlslString = @"
        float2 pa = uv - " + this.variables[0] + " - " + this.variables[1] + @";
        float2 ba = " + this.variables[2] + " - " + this.variables[1] + @";
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float "+ this.o + " = length(pa - ba*h) - " + this.variables[3]+ ";";
        
        return hlslString;
    }
}
