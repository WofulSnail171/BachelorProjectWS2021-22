using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Texture")]
public class SDFTexture : SDFObject {
    [SerializeField] private Texture sdfTexture;
    private Texture _sdfTexture;
    
    public Texture SdfTexture {
        get => this._sdfTexture;
        set {
            if (this._sdfTexture == value) return;
            this._sdfTexture = value;
            this.isDirty = true;
        }
    }

    private void OnValidate() {
        this.SdfTexture = this.sdfTexture;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }
    
    private void Awake() {
        this.nodeType = NodeType.Texture;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "tex" + this.index;
        this.o = this.sdfName + "_out";
        
        if (this.variables != null) {
            this.variables.Clear();
            this.types.Clear();
        }
        
        this.variables.Add(this.sdfName + "_tex");
        this.types.Add("UnityTexture2D");
        this.variables.Add(this.sdfName + "_sampleState");
        this.types.Add("UnitySamplerState");
    }
    
    public override string GenerateHlslFunction() {

        string hlslString =  "float " + this.o + " = SAMPLE_TEXTURE2D(" + this.variables[0] + ", " + this.variables[1] + ", uv).r;";
        return hlslString;
    }
}
