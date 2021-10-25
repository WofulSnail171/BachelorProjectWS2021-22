using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SDFCircle : SDFScriptableObject {
    [SerializeField]private float radius = 0f;
    public float Radius => this.radius;
    
    public override string SDFFunction() {
        this.o = this.Name + "_out";
        string hlslString = "float " + this.o + " = length(" + Position + "- uv)- " + Radius + ";" ;
        return hlslString;
    }
    
}
