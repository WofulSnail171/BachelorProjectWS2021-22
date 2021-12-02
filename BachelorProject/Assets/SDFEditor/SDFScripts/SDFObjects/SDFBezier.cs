using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Bezier")]
public class SDFBezier : SDFObject
{
    [SerializeField] private Vector2 a;
    private Vector2 _a;
    
    [SerializeField] private Vector2 b;
    private Vector2 _b;
    
    [SerializeField] private Vector2 c;
    private Vector2 _c;

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
    
    public Vector2 C {
        get => this._c;
        set {
            if (this._c == value) return;
            this._c = value;
            this.isDirty = true;
        }
    }
    
    void OnValidate() {
        this.Position = this.position;
        this.A = this.a;
        this.B = this.b;
        this.C = this.c;
        if (this.isDirty) {
            this.OnValueChange?.Invoke(this);
            this.isDirty = false;
        }
    }
    
    private void Awake() {
        this.nodeType = NodeType.BezierCurve;
        
        this.index = (uint)Random.Range(0, 1000);
       
        this.sdfName = "bezier" + this.index;
        this.o = this.sdfName + "_out";
        
        if (this.variables != null) {
            this.variables.Clear();
            this.types.Clear();
        }
        
        this.variables.Add(this.sdfName + "_position");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_a");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_b");
        this.types.Add("float2");
        this.variables.Add(this.sdfName + "_c");
        this.types.Add("float2");
    }
    
    public override string GenerateHlslFunction() {
        
        string hlslString = @"
        float2 pos_" + this.sdfName + " = uv - " + this.variables[0] + @";
        float2 A_" + this.sdfName +" = " + this.variables[2] + " - " + this.variables[1] + @";
        float2 B_" + this.sdfName + " = " + this.variables[1] + " - 2.0 * " + this.variables[2] + " + " + this.variables[3] + @";
        float2 C_" + this.sdfName + " =  A_" + this.sdfName + @" * 2.0;
        float2 D_" + this.sdfName + " =  " + this.variables[1] + @" - pos_" + this.sdfName + @";
        float kk_" + this.sdfName + " = 1.0/dot(B_" + this.sdfName + ", B_" + this.sdfName + @");
        float kx_" + this.sdfName + " = kk_" + this.sdfName + " * dot(A_" + this.sdfName + @", B_" + this.sdfName + @");
        float ky_" + this.sdfName + " = kk_" + this.sdfName + " * (2.0*dot(A_" + this.sdfName + ", A_" + this.sdfName + @")+dot(D_" + this.sdfName + ", B_" + this.sdfName + @")) / 3.0;
        float kz_" + this.sdfName + " = kk_" + this.sdfName + " * dot(D_" + this.sdfName + ", A_" + this.sdfName + @");      
        float res_" + this.sdfName + @" = 0.0;
        float p_" + this.sdfName + " = ky_" + this.sdfName + " - kx_" + this.sdfName + " * kx_" + this.sdfName + @";
        float p3_" + this.sdfName + " = p_" + this.sdfName + " * p_" + this.sdfName + " * p_" + this.sdfName + @";
        float q_" + this.sdfName + " = kx_" + this.sdfName + " * (2.0 * kx_" + this.sdfName + " * kx_" + this.sdfName + " - 3.0 * ky_" + this.sdfName + ") + kz_" + this.sdfName + @";
        float h_" + this.sdfName + " = q_" + this.sdfName + " * q_" + this.sdfName + " + 4.0 * p3_" + this.sdfName + @";
        if( h_" + this.sdfName + @" >= 0.0) 
        { 
            h_" + this.sdfName + " = sqrt(h_" + this.sdfName + @");
            float2 x_" + this.sdfName + " = (float2(h_" + this.sdfName + ",-h_" + this.sdfName + ")-q_" + this.sdfName + @")/2.0;
            float2 uv_" + this.sdfName + " = sign(x_" + this.sdfName + ") * pow(abs(x_" + this.sdfName + @"), float2(1.0/3.0,1.0/3.0));
            float t_" + this.sdfName + " = clamp( uv_" + this.sdfName + ".x+uv_" + this.sdfName + ".y-kx_" + this.sdfName + @", 0.0, 1.0 );
            res_" + this.sdfName + " = dot2(D_" + this.sdfName + " + (C_" + this.sdfName + " + B_" + this.sdfName + " * t_" + this.sdfName + ") * t_" + this.sdfName + @");
        }
        else
        {
            float z_" + this.sdfName + " = sqrt(-p_" + this.sdfName + @");
            float v_" + this.sdfName + " = acos( q_" + this.sdfName + "/(p_" + this.sdfName + " * z_" + this.sdfName + @" * 2.0) ) / 3.0;
            float m_" + this.sdfName + " = cos(v_" + this.sdfName + @");
            float n_" + this.sdfName + " = sin(v_" + this.sdfName + @") * 1.732050808;
            float3  t_" + this.sdfName + " = clamp(float3(m_" + this.sdfName + " + m_" + this.sdfName + ",-n_" + this.sdfName + " - m_" + this.sdfName + ", n_" + this.sdfName + " - m_" + this.sdfName + ") * z_" + this.sdfName + " - kx_" + this.sdfName + @",0.0,1.0);
        
            res_" + this.sdfName + " = min( dot2(D_" + this.sdfName + " + (C_" + this.sdfName + " + B_" + this.sdfName + " * t_" + this.sdfName + ".x) * t_" + this.sdfName + @".x),
                dot2(D_" + this.sdfName + " + (C_" + this.sdfName + " + B_" + this.sdfName + " * t_" + this.sdfName + ".y) * t_" + this.sdfName + @".y) );
        }
        float " + this.o + " = sqrt(res_" + this.sdfName + ");";
    
        return hlslString;
    }
}
