using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Function/Smooth Blend")]
public class SDFSBLend : SDFNode
{
    [SerializeField] private SDFScriptableObject inputA;
    [SerializeField] private SDFScriptableObject inputB;
    public override string SDFFunction() {
        this.o = this.SDFName +"_out";
        string hlslString = @"
    float h = max( k-abs(float2" + this.inputA + " - float2" + this.inputB + @"), 0.0 )/k;
    float this.o =  min( float2" + this.inputA + ", float2" + this.inputB + ") - h*h*k*(1.0/4.0);";
        return hlslString;
    }
}
