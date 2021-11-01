using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Line")]
public class SDFLine : SDFScriptableObject {
    [SerializeField] private Vector2 a;
    [SerializeField] private Vector2 b;
    
    public override string SDFFunction() {
        this.o = this.SDFName + "_out";
        string hlslString = @"
        float2 pa = uv - float2" + this.Position + " - float2" + this.a + @";
        float2 ba = float2" + this.b + " - float2" + this.a + @";
        float h = clamp(dot(pa, ba)/dot(ba, ba), 0, 1);
        float "+ this.o + " = length(pa - ba*h);";
        return hlslString;
    }
}
