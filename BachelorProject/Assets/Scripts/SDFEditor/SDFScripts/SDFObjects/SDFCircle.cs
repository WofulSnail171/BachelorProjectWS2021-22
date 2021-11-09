using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SDF/Circle")]
public class SDFCircle : SDFScriptableObject {
    
    private float radius = 0f;
    public float Radius {
        get => this.radius;
        set {
            if (this.radius == value) return;
            this.radius = value;
            //TODO: call OnChange event
        }
    }
    
    
    
    private void Awake() {
        this.index = (uint)Random.Range(0, 1000);
        Debug.Log("changed index from " + this.sdfName);
        
        this.sdfName = "circle" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_radius");
        this.types.Add("float2");
    }
    
    public override string SdfFunction() {
        
        this.sdfName = "circle" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_radius");
        this.types.Add("float2");
        
        string hlslString = "float " + this.o + " = length(" + this.variables[0] + "- uv)- " + this.variables[1] + ";" ;
        return hlslString;
    }
    
}
