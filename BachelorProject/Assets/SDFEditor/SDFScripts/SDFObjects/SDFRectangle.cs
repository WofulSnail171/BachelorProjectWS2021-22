
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF/Rectangle")]
public class SDFRectangle : SDFObject
{
    [SerializeField] private float rotation;
    private float _rotation;
    
    [SerializeField] private float scale = 1;
    private float _scale = 1;
    
    [SerializeField] private Vector2 box = new Vector2(0,0);
    private Vector2 _box = new Vector2(0,0);

    [SerializeField] private Vector4 roundness = new Vector4(0, 0, 0, 0);
    private Vector4 _roundness = new Vector4(0, 0, 0, 0);

    public Vector2 Box {
        get => this._box;
        set {
            if (this._box == value) return;
            this._box = value;
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
    
    public Vector4 Roundness {
        get => this._roundness;
        set {
            if (this._roundness == value) return;
            this._roundness = value;
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

    private void OnValidate() {
        this.Position = this.position;
        this.Box = this.box;
        this.Scale = this.scale;
        this.Roundness = this.roundness;
        this.Rotation = this.rotation;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }

    private void Awake() {
        this.nodeType = NodeType.Rect;
        
        this.index = (uint)Random.Range(0, 1000);
   
        this.sdfName = "rect" + this.index;
        this.o = this.sdfName + "_out";
        
        if (this.variables != null) {
            this.variables.Clear();
            this.types.Clear();
        }
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_box");
        this.types.Add("float2");
        this.variables.Add( this.sdfName + "_scale");
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_roundness");
        this.types.Add("float4");
        this.variables.Add(this.sdfName + "_rotation");       
        this.types.Add("float");
    }
    public override string GenerateHlslFunction() {

        string hlslString = @"
        float2 t_" + this.sdfName + @" = transform(" + this.variables[0] + ", " + this.variables[4] + ", " + this.variables[2] + @", uv);
        "+ this.variables[3] + ".xy = (t_" + this.sdfName + ".x > 0.0) ? " + this.variables[3] + ".xy : " + this.variables[3] + @".zw;
        " + this.variables[3] + ".x  = (t_" + this.sdfName + @".y  > 0.0) ? " + this.variables[3] + ".x  : " + this.variables[3] + @".y;
        float2 q_" + this.sdfName + " = abs(t_" + this.sdfName + ") - " + this.variables[1] + " + " + this.variables[3] + @".x;
        float " + this.o + " = (min(max(q_" + this.sdfName + ".x,q_" + this.sdfName + ".y),0.0) + length(max(q_" + this.sdfName + ",0.0)) - " + this.variables[3] + ".x) * " + this.variables[2] + ";";
        
        return hlslString;
    }
}