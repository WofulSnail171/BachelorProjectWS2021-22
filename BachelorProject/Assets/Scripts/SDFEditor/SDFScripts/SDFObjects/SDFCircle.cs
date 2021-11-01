using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SDF/Circle")]
public class SDFCircle : SDFScriptableObject {
    [SerializeField]private float radius = 0f;
    public float Radius => this.radius;
    
    public override string SDFFunction() {
        this.o = this.SDFName + "_out";
        string hlslString = "float " + this.o + " = length(" + this.Position + "- uv)- " + this.Radius + ";" ;
        return hlslString;
    }
    
}
