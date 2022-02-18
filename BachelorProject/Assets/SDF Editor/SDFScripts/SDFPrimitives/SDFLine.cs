
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Primitives/Line")]
public class SDFLine : SDFPrimitive {
    [SerializeField] private float scale;
    private float _scale = 1;
    
    [SerializeField] private float rotation;
    private float _rotation;
    
    [SerializeField] private Vector2 a;
    private Vector2 _a;
    
    [SerializeField] private Vector2 b;
    private Vector2 _b;
    
    [SerializeField] private float roundness;
    private float _roundness = 1;
    
    public Vector2 A {
        get => this._a;
        set {
            if (this._a == value) return;
            this._a = value;
            this.isDirty = true;
        }
    }
    
    public Vector2 B {
        get => this._b;
        set {
            if (this._b == value) return;
            this._b = value;
            this.isDirty = true;
        }
    }
    
    public float Roundness {
        get => this._roundness;
        set {
            if (this._roundness == value) return;
            this._roundness = value;
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
    
    private void OnValidate() {
        this.Position = this.position;
        this.A = this.a;
        this.B = this.b;
        this.Roundness = this.roundness;
        this.Scale = this.scale;
        this.Rotation = this.rotation;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }
    
    private void Awake() {
        this.nodeType = NodeType.Line;
        
        this.index = (uint)Random.Range(0, 1000);
 
        this.sdfName = "line" + this.index;
        this.o = this.sdfName + "_out";

        Debug.Log("changed index from line to: " + this.index);
        
        this.variables.Clear();
        this.types.Clear();

        this.variables.Add(this.sdfName + "_position");       //0
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_a");              //1
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_b");              //2
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_roundness");      //3
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_scale");          //4
        this.types.Add("float");
        this.variables.Add(this.sdfName + "_rotation");       //5
        this.types.Add("float");
    }
    
    public override string GenerateHlslFunction() {
        
        string hlslString = @"
        
        float2 pa_" + this.sdfName + " = transform("+ this.variables[0] + ", "+ this.variables[5] + ", "+ this.variables[4] + ", uv) - " + this.variables[1] + @";
        float2 ba_" + this.sdfName + " = " + this.variables[2] + " - " + this.variables[1] + @";
        float h_" + this.sdfName + " = clamp(dot(pa_" + this.sdfName + ", ba_" + this.sdfName + ")/dot(ba_" + this.sdfName + ", ba_" + this.sdfName + @"), 0, 1);
        float "+ this.o + " = (length(pa_" + this.sdfName + " - ba_" + this.sdfName + "*h_" + this.sdfName + ") - (0.01 * " + this.variables[3]+ ")) * " + this.variables[4] +";";
        
        return hlslString;
    }
}
