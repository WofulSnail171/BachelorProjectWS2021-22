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
        this.Position = this.position;
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
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_tex");
        this.types.Add("sampler2D");
    }
    
    public override string GenerateHlslFunction() {

        string hlslString =  "float " + this.o + " = tex2D(" + this.variables[1] + ", uv + " + this.variables[0] + " + float2(0.5, 0.5)).r;";
        return hlslString;
    }
}
