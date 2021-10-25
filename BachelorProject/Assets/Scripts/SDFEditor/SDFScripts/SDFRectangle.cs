using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDFRectangle : SDFScriptableObject
{
    [SerializeField] Vector2 box = new Vector2(0,0);
    [SerializeField] private ushort scale = 1;

    private Vector2 Box => this.box;
    private ushort Scale => this.scale;
    public override string SDFFunction() {
        Vector2 b = Box * Scale;

        string hlslString = "float2 d = abs(uv"+ Position + ") -" + b + @";
        Out = length(max(d, 0)) + min(max(d.x, d.y), 0);";
        return hlslString;
    }
}
