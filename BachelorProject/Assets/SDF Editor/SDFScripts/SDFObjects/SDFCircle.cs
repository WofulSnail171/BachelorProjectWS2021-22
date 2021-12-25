using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF/Circle")]
public class SDFCircle : SDFObject {

    private float _radius;
    [SerializeField] private float radius;
    
    public float Radius {
        get => this._radius;
        set {
            if (this._radius == value) return;
            this._radius = value;
            this.isDirty = true;
        }
    }
    
    private void OnValidate() {
        this.Radius = this.radius;
        this.Position = this.position;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }  
    
    private void Awake() {
        nodeType = NodeType.Circle;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "circle" + this.index;
        
        this.o = this.sdfName + "_out";
        
        if (this.variables != null) {
            this.variables.Clear();
            this.types.Clear();
        }
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_radius");
        this.types.Add("float");
    }
    
    public override string GenerateHlslFunction() {
        
        string hlslString = @"
        float " + this.o + " = length(" + this.variables[0] + "- uv)- " + this.variables[1] + ";" ;
        return hlslString;
    }
    
}
