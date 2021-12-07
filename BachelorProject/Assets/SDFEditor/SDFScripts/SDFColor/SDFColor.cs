using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF Color/Color")]
public class SDFColor : SDFColorInput {
    [SerializeField]private Color color;
    private Color _color;

    public Color Color{
        get => this._color;
        set {
            if (this._color == value) return;
            this._color = value;
            this.OnInputChange?.Invoke();
        }
    }

    private void OnValidate() {
        this.Color = this.color;
    }

    private void Awake() {
        this.colorNodeType = ColorNodeType.Color;
        
        this.index = (uint)UnityEngine.Random.Range(0, 1000);
        
        this.sdfName = "color" + this.index;
        this.o = this.sdfName;
        
        this.variables.Clear();
        this.types.Clear();
        
        this.variables.Add(this.sdfName);
        this.types.Add("float4");

    }

    public override string GenerateHlslFunction() {

        string hlslString = "";
        
        return hlslString;
    }
}
