using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Color/Texture")]
public class SDFTextureInput : SDFColorInput {

    [SerializeField] private Texture sdfTexture;
    private Texture _sdfTexture;
    
    [SerializeField] private Vector2 position;
    private Vector2 _position;
    
    [SerializeField] private float scale;
    private float _scale = 1;
    
    [SerializeField] private float rotation;
    private float _rotation;

    [SerializeField] private Color color;
    private Color _color;
    
    public Texture SdfTexture {
        get => this._sdfTexture;
        set {
            if (this._sdfTexture == value) return;
            this._sdfTexture = value;
            this.isDirty = true;
        }
    }
    public float Scale {
        get => this._scale;
        set {
            if (this._scale == value) return;
            this._scale = value;
            this.isDirty = true;
        }
    }
    
    public float Rotation {
        get => this._rotation;
        set {
            if (this._rotation == value) return;
            this._rotation = value;
            this.isDirty = true;
        }
    }

    public Vector2 Position {
        get => this._position;
        set {
            if (this._position == value) return;
            this._position = value;
            this.isDirty = true;
        }
    }

    public Color Color {
        get => this._color;
        set {
            if (this._color == value) return;
            this._color = value;
            this.isDirty = true;
        }
    }

    private void OnValidate() {
        this.SdfTexture = this.sdfTexture;
        this.Position = this.position;
        this.Scale = this.scale;
        this.Rotation = this.rotation;
        this.Color = this.color;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }
    
    private void Awake() {
        this.colorNodeType = ColorNodeType.Texture;
        
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
        this.variables.Add(this.sdfName + "_scale");          
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_rotation");       
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_color");
        this.types.Add("float4");
    }
    
    public override string GenerateHlslFunction() {
        string hlsl = "float4  "+ this.o + " = tex2D(" + this.variables[1] + ", transform(" + this.variables[0] + ", " + this.variables[3] + ", " + this.variables[2] + ", uv) + " + this.variables[0] + " + float2(0.5, 0.5)) * " + this.variables[4] + ";";;
        
        return hlsl;
    }
}
