using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Texture")]
public class SDFTexture : SDFScriptableObject {
    [SerializeField] private Texture sdfTexture;
    
    private void Awake() {
        this.index = (uint)Random.Range(0, 1000);
        Debug.Log("changed index from " + this.sdfName);
        
        this.sdfName = "tex" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        this.variables.Add(this.sdfName + "_tex");
        this.types.Add("UnityTexture2D");
        this.variables.Add(this.sdfName + "_sampleState");
        this.types.Add("UnitySamplerState");
    }
    
    public override string SDFFunction() {

        string hlslString =  "float " + this.o + " = SAMPLE_TEXTURE2D(" + this.variables[0] + ", " + this.variables[1] + ", uv).r;";

        return hlslString;

    }
}
