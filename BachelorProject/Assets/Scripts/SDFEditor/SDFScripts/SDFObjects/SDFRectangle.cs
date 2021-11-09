using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SDF/Rectangle")]
public class SDFRectangle : SDFScriptableObject
{
    [SerializeField] Vector2 box = new Vector2(0,0);
    [SerializeField] private float scale = 1;
    [SerializeField] private Vector4 roundness = new Vector4(0, 0, 0, 0);

    private void Awake() {
        this.index = (uint)Random.Range(0, 1000);
        Debug.Log("changed index from " + this.sdfName);
        
        this.sdfName = "rect" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_box");
        this.types.Add("float2");
        this.variables.Add( this.sdfName + "_scale");
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_roundness");
        this.types.Add("float4");
    }
    public override string SdfFunction() {

        Vector2 b = this.box * this.scale;

        string hlslString = this.variables[3] + ".xy = (" + this.variables[0] + ".x - uv.x > 0.0) ? " + this.variables[3] + ".xy : " + this.variables[3] + @".zw;
        " + this.variables[3] + ".x  = (" + this.variables[0] + ".y - uv.y > 0.0) ? " + this.variables[3] + ".x  : " + this.variables[3] + @".y;
        float2 q = abs(" + this.variables[0] + " - uv) - " + this.variables[1] + " + " + this.variables[3] + @".x;
        float " + this.o + " = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - " + this.variables[3] + ".x;";
        
        return hlslString;
    }
}