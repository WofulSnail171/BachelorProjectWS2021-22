using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SDFRectangle : SDFScriptableObject
{
    [SerializeField] Vector2 box = new Vector2(0,0);
    [SerializeField] private float scale = 1;

    private Vector2 Box => this.box;
    private float Scale => this.scale;
    public override string SDFFunction() {
        this.o = this.SDFName + "_out";
        Vector2 b = this.Box * this.Scale;

        string hlslString = "float2 d = abs("+ this.Position + " - uv) - float2" + b + @";
    float "+ this.o + " = length(max(d, 0)) + min(max(d.x, d.y), 0);";
        return hlslString;
    }
}
