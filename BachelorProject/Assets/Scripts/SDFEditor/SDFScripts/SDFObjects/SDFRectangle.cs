using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SDF/Rectangle")]
public class SDFRectangle : SDFScriptableObject
{
    [SerializeField] Vector2 box = new Vector2(0,0);
    [SerializeField] private float scale = 1;
    
    public override string SDFFunction() {
        this.o = this.SDFName + "_out";
        Vector2 b = this.box * this.scale;

        string hlslString = "float2 d = abs(" + this.Position + " - uv) - float2" + b + @";
    float "+ this.o + " = length(max(d, 0)) + min(max(d.x, d.y), 0) * 0.5 * " + this.scale +";";
        return hlslString;
    }
}
