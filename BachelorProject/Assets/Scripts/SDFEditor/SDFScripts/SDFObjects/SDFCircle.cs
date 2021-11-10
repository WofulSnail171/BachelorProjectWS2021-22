using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SDF/Circle")]
public class SDFCircle : SDFObject {
    
    public float radius;
    
    public float Radius {
        get => this.radius;
        set {
            if (this.Radius == value) return;
            this.Radius = value;
            this.OnValueChange?.Invoke(this);
        }
    }
    
    private void OnValidate() {
        if (this.Radius != this.radius) this.Radius = this.radius;
    }
    
    private void Awake() {
        nodeType = NodeType.Circle;
        
        this.index = (uint)Random.Range(0, 1000);

        this.sdfName = "circle" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_radius");
        this.types.Add("float2");
    }
    
    public override string SdfFunction() {
        
        this.sdfName = "circle" + this.index;
        this.o = this.sdfName + "_out";
        
        this.variables.Clear();
        this.types.Clear();
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_radius");
        this.types.Add("float2");
        
        string hlslString = "float " + this.o + " = length(" + this.variables[0] + "- uv)- " + this.variables[1] + ";" ;
        return hlslString;
    }
    
}
