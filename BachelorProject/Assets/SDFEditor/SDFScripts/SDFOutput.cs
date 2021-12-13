using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteAlways][CreateAssetMenu(menuName = "SDF Output/Output")]
public class SDFOutput : SDFNode{

    [SerializeField] private string shaderName; 
        
    private string pathShaderFile;
    private string pathIncludeFile;

    [SerializeField]private SDFNode input;
    private SDFNode _input;
    
    public SDFNode Input {
        get => this._input;
        set {
            if (this._input == value) return;
            this._input = value;
            this.OnInputChange?.Invoke();
        }
    }

    public Material sdfMaterial;
    private Shader sdfShader;
    
    public bool applyMaterial;
    
    private List<string> shaderStrings = new List<string>();
    private List<string> includeStrings = new List<string>();
    private List <SDFNode>  SDFNodes = new List<SDFNode>();

    //Color variables
    #region Color

    private enum ColorChange {
        Inside,
        Outside,
        Outline
    }

    private ColorChange colorChange;
    private Action<ColorChange> OnColorChange;

    private bool isInsideDirty;
    private bool isOutsideDirty;
    private bool isOutlineDirty;
    

    [Space] [Header("Inside")]
    [SerializeField] private Texture insideTex;
    private Texture _insideTex;
    
    [SerializeField] private Color insideColor;
    private Color _insideColor;

    [SerializeField] private Vector2 insideTexPosition;
    private Vector2 _insideTexPosition;
    
    [SerializeField] private float insideTexScale;
    private float _insideTexScale = 1;
    
    [SerializeField] private float insideTexRotation;
    private float _insideTexRotation;
    
    [Header("Outside")] 
    [SerializeField] private Texture outsideTex;
    private Texture _outsideTex;
    
    [SerializeField] private Color outsideColor;
    private Color _outsideColor;
    
    [SerializeField] private Vector2 outsideTexPosition;
    private Vector2 _outsideTexPosition;
    
    [SerializeField] private float outsideTexScale;
    private float _outsideTexScale = 1;
    
    [SerializeField] private float outsideTexRotation;
    private float _outsideTexRotation;
    
    [Header ("Outline")]
    [SerializeField] private Texture outlineTex;
    private Texture _outlineTex;
    
    [SerializeField] private Color outlineColor;
    private Color _outlineColor;
    
    [SerializeField] private Vector2 outlineTexPosition;
    private Vector2 _outlineTexPosition;
    
    [SerializeField] private float outlineTexScale;
    private float _outlineTexScale = 1;
    
    [SerializeField] private float outlineTexRotation;
    private float _outlineTexRotation;

    [SerializeField] private float thickness;
    private float _thickness = 0.2f;

    [SerializeField] private int repetition;
    private int _repetition = 1;

    [SerializeField] private float lineDistance;
    private float _lineDistance = 1f;
    
    
    //Color variable Setter & Getter
    //Inside
    public Texture InsideTex {
        get => this._insideTex;
        set {
            if (this._insideTex == value) return;
            this._insideTex = value;
            this.isInsideDirty = true;
        }
    }
    
    public Color InsideColor {
        get => this._insideColor;
        set {
            if (this._insideColor == value) return;
            this._insideColor = value;
            this.isInsideDirty = true;
        }
    }
    
    public Vector2 InsideTexPosition {
        get => this._insideTexPosition;
        set {
            if (this._insideTexPosition == value) return;
            this._insideTexPosition = value;
            this.isInsideDirty = true;
        }
    }
    
    public float InsideTexScale {
        get => this._insideTexScale;
        set {
            if (this._insideTexScale == value) return;
            this._insideTexScale = value;
            this.isInsideDirty = true;
        }
    }
    
    public float InsideTexRotation {
        get => this._insideTexRotation;
        set {
            if (this._insideTexRotation == value) return;
            this._insideTexRotation = value;
            this.isInsideDirty = true;
        }
    }
    
    //Outside
    public Texture OutsideTex {
        get => this._outsideTex;
        set {
            if (this._outsideTex == value) return;
            this._outsideTex = value;
            this.isOutsideDirty = true;
        }
    }

    public Color OutsideColor {
        get => this._outsideColor;
        set {
            if (this._outsideColor == value) return;
            this._outsideColor = value;
            this.isOutsideDirty = true;
        }
    }
    
    public Vector2 OutsideTexPosition {
        get => this._outsideTexPosition;
        set {
            if (this._outsideTexPosition == value) return;
            this._outsideTexPosition = value;
            this.isOutsideDirty = true;
        }
    }
    
    public float OutsideTexScale {
        get => this._outsideTexScale;
        set {
            if (this._outsideTexScale == value) return;
            this._outsideTexScale = value;
            this.isOutsideDirty = true;
        }
    }
    
    public float OutsideTexRotation {
        get => this._outsideTexRotation;
        set {
            if (this._outsideTexRotation == value) return;
            this._outsideTexRotation = value;
            this.isOutsideDirty = true;
        }
    }
    
    //Outline
    public Texture OutlineTex {
        get => this._outlineTex;
        set {
            if (this._outlineTex == value) return;
            this._outlineTex = value;
            this.isOutlineDirty = true;
        }
    }

    public Color OutlineColor {
        get => this._outlineColor;
        set {
            if (this._outlineColor == value) return;
            this._outlineColor = value;
            this.isOutlineDirty = true;
        }
    }
    
    public Vector2 OutlineTexPosition {
        get => this._outlineTexPosition;
        set {
            if (this._outlineTexPosition == value) return;
            this._outlineTexPosition = value;
            this.isOutlineDirty = true;
        }
    }
    
    public float OutlineTexScale {
        get => this._outlineTexScale;
        set {
            if (this._outlineTexScale == value) return;
            this._outlineTexScale = value;
            this.isOutlineDirty = true;
        }
    }
    
    public float OutlineTexRotation {
        get => this._outlineTexRotation;
        set {
            if (this._outlineTexRotation == value) return;
            this._outlineTexRotation = value;
            this.isOutlineDirty = true;
        }
    }
    
    public float Thickness {
        get => this._thickness;
        set {
            if (this._thickness == value) return;
            this._thickness = value;
            this.isOutlineDirty = true;
        }
    }
    
    public int Repetition {
        get => this._repetition;
        set {
            if (this._repetition == value) return;
            this._repetition = value;
            this.isOutlineDirty = true;
        }
    }
    
    public float LineDistance {
        get => this._lineDistance;
        set {
            if (this._lineDistance == value) return;
            this._lineDistance = value;
            this.isOutlineDirty = true;
        }
    }

    #endregion

    private void OnValidate() {
        this.Input = this.input;

        this.InsideTex = this.insideTex;
        this.InsideColor = this.insideColor;
        this.InsideTexPosition = this.insideTexPosition;
        this.InsideTexScale = this.insideTexScale;
        this.InsideTexRotation = this.insideTexRotation;
        
        this.OutsideTex = this.outsideTex;
        this.OutsideColor = this.outsideColor;
        this.OutsideTexPosition = this.outsideTexPosition;
        this.OutsideTexScale = this.outsideTexScale;
        this.OutsideTexRotation = this.outsideTexRotation;
        
        this.OutlineTex = this.outlineTex;
        this.OutlineColor = this.outlineColor;
        this.OutlineTexPosition = this.outlineTexPosition;
        this.OutlineTexScale = this.outlineTexScale;
        this.OutlineTexRotation = this.outlineTexRotation;
        
        this.Thickness = this.thickness;
        this.Repetition = this.repetition;
        this.LineDistance = this.lineDistance;
        
        if (this.isInsideDirty) {
            this.colorChange = ColorChange.Inside;
            this.OnColorChange?.Invoke(this.colorChange);
            this.isInsideDirty = false;
        }
        if (this.isOutsideDirty) {
            this.colorChange = ColorChange.Outside;
            this.OnColorChange?.Invoke(this.colorChange);
            this.isOutsideDirty = false;
        }
        if (this.isOutlineDirty) {
            this.colorChange = ColorChange.Outline;
            this.OnColorChange?.Invoke(this.colorChange);
            this.isOutlineDirty = false;
        }

        if (this.applyMaterial) {
            
            this.ApplyMaterial();
            this.applyMaterial = false;
        }
    }

    private void Awake() {
        this.OnInputChange += this.UpdateShader;
        this.OnColorChange += this.UpdateColor;
    }

    private void ApplyMaterial() {

        this.UpdateShader();

        if (this.sdfMaterial == null){
            this.sdfShader = Shader.Find("SDF/" + this.shaderName);
            if (this.sdfShader != null) {
                this.sdfMaterial = new Material("SDFMaterial");
                this.sdfMaterial.shader = this.sdfShader;
            }
            else{
                Debug.LogWarning("shader could not be found");
            }
        }
        else if(this.sdfMaterial.shader != this.sdfShader) {
            this.sdfShader = Shader.Find("SDF/" + this.shaderName);
            this.sdfMaterial.shader = this.sdfShader;
        }
    }

    private void UpdateShader() {
      
        if (!this.Input) {
            Debug.LogWarning("forgot to assign Input in Output Node");
            return;
        }
        if (!this.sdfMaterial) {
            Debug.LogWarning("forgot to assign Material in Output Node");
            return; 
        }

        this.UpdateActiveNodes();
        
        this.AddHlslString();
        foreach (SDFNode s in this.SDFNodes) {
            this.ChangeShaderValues(s);
        }
        
    }

    private void AddHlslString() {
        
        //////////// Generate Shader File //////////////
        this.shaderStrings.Clear();
       
        this.shaderStrings.Add(this.GenerateShaderName());
        this.shaderStrings.Add(this.GenerateShaderProperties());
        this.shaderStrings.Add(this.GenerateShaderTags());
        this.shaderStrings.Add(this.GenerateShaderPass());
        this.shaderStrings.Add(this.GenerateShaderVert());
        this.shaderStrings.Add(this.GenerateShaderVariables());
        this.shaderStrings.Add(this.GenerateShaderFrag());
        this.shaderStrings.Add(this.GenerateShaderEnd());
        
        //////////// Generate Include File /////////////
        this.includeStrings.Clear();
        this.includeStrings.Add(this.GenerateIncludeIfdef());
        this.includeStrings.Add(this.GenerateShaderSdfFunction(this.Input));
        this.includeStrings.Add(this.GenerateIncludeSdfColor());
        
        this.WriteHlslToText();
    }

    string GenerateShaderName() {
        return @"Shader ""SDF/" + this.shaderName + @"""
        {";
    }
    
    string GenerateShaderProperties() {

        string properties = "";

        foreach (SDFNode node in this.SDFNodes) {
            if (node == null) {
                Debug.Log(node.sdfName + " is null");
                break;
            }
            switch (node.nodeType) {
                case NodeType.Circle: {
                    var n = (SDFCircle) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_radius (""" + n.sdfName + @"_radius"", Float) = 0.2
                ";
                    break;
                }
                case NodeType.Rect: {
                    var n = (SDFRectangle) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_box (""" + n.sdfName + @"_box"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_scale (""" + n.sdfName + @"_scale"", Float) = 1
                [HideInInspector] " + n.sdfName + @"_roundness (""" + n.sdfName + @"_roundness"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_rotation (""" + n.sdfName + @"_rotation"", Float) = 0
                ";
                    break;
                }
                case NodeType.Triangle: {
                    var n = (SDFTriangle) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_a (""" + n.sdfName + @"_a"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_b (""" + n.sdfName + @"_b"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_c (""" + n.sdfName + @"_c"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_scale (""" + n.sdfName + @"_scale"", Float) = 1
                [HideInInspector] " + n.sdfName + @"_rotation (""" + n.sdfName + @"_rotation"", Float) = 0
                ";
                    break;
                }
                case NodeType.Line: {
                    var n = (SDFLine) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_a (""" + n.sdfName + @"_a"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_b (""" + n.sdfName + @"_b"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_scale (""" + n.sdfName + @"_scale"", Float) = 1
                [HideInInspector] " + n.sdfName + @"_roundness (""" + n.sdfName + @"_roundness"", Float) = 1
                [HideInInspector] " + n.sdfName + @"_rotation (""" + n.sdfName + @"_rotation"", Float) = 0
                ";
                    break;
                }
                case NodeType.BezierCurve: {

                    var n = (SDFBezier) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_a (""" + n.sdfName + @"_a"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_b (""" + n.sdfName + @"_b"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_c (""" + n.sdfName + @"_c"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_scale (""" + n.sdfName + @"_scale"", Float) = 1
                [HideInInspector] " + n.sdfName + @"_rotation (""" + n.sdfName + @"_rotation"", Float) = 0
                [HideInInspector] " + n.sdfName + @"_roundness (""" + n.sdfName + @"_roundness"", Float) = 1
                ";

                    break;
                }
                case NodeType.Texture: {
                    var n = (SDFTexture) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_scale (""" + n.sdfName + @"_scale"", Float) = 1
                [HideInInspector] " + n.sdfName + @"_rotation (""" + n.sdfName + @"_rotation"", Float) = 0
                [HideInInspector] " + n.sdfName + @"_tex (""" + n.sdfName + @"_tex"", 2D) = ""white""{}
                ";
                    break;
                }
                case NodeType.SBlend: {
                    var n = (SDFSBLend) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_k (""" + n.sdfName + @"_k"", Float) = 0
                ";
                    break;
                }
                case NodeType.Lerp: {
                    var n = (SDFLerp) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_t (""" + n.sdfName + @"_t"", Float) = 0
                ";
                    break;
                }
                default: {
                    break;
                }
            }
        }
        
        return @"
        Properties
        {
            " + properties + @"

            [HideInInspector] insideTex (""inside Texture"", 2D) = ""white""{}
            [HideInInspector] insideColor (""inside Color"", Color) = (1,1,1,1)
            [HideInInspector] insideTexPosition (""inside Texture Position"", Vector) = (0,0,0,0)
            [HideInInspector] insideTexScale (""inside Texture Scale"", Float) = 1
            [HideInInspector] insideTexRotation (""inside Texture Rotation"", Float) = 0

            [HideInInspector] outsideTex (""outside Texture"", 2D) = ""white""{}
            [HideInInspector] outsideColor (""outside Color"", Color) = (1,1,1,1)
            [HideInInspector] outsideTexPosition (""outside Texture Position"", Vector) = (0,0,0,0)
            [HideInInspector] outsideTexScale (""outside Texture Scale"", Float) = 1
            [HideInInspector] outsideTexRotation (""outside Texture Rotation"", Float) = 0

            [HideInInspector] outlineTex (""outline Texture"", 2D) = ""white""{}
            [HideInInspector] outlineColor (""outline Color"", Color) = (1,1,1,1)
            [HideInInspector] outlineTexPosition (""outline Texture Position"", Vector) = (0,0,0,0)
            [HideInInspector] outlineTexScale (""outline Texture Scale"", Float) = 1
            [HideInInspector] outlineTexRotation (""outline Texture Rotation"", Float) = 0

            [HideInInspector] outlineThickness (""outline Thickness"", Float) = 0.2
            [HideInInspector] outlineRepetition (""outline Repetition"", Float) = 1
            [HideInInspector] outlineLineDistance (""outline LineDistance"", Float) = 1

            [Enum(Off, 0, On, 1)] _ZWrite (""Z Write"", Float) = 1
            [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest(""ZTest"", Float) = 0
            [Enum(UnityEngine.Rendering.CullMode)] _CullMode(""Cull Mode"", Float) = 0
            [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend (""Source Blend mode"", Float) = 1
            [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend (""Destination Blend mode"", Float) = 1
        }";
    }
    
    string GenerateShaderTags() {
        return@"
            SubShader
            {
            Tags { ""RenderType""=""Opaque"" 
                   ""RenderPipeline""=""UniversalRenderPipeline""
                 }
            LOD 100";
    }
    
    string GenerateShaderPass() {
        return@"
            Pass
            {
            
            Blend [_SrcBlend] [_DestBlend]
            Cull [_CullMode]
            ZWrite [_ZWrite]
            ZTest [_ZTest]            

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
                        
            #include ""Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl""       
            #include """ + this.shaderName + @".hlsl""
            
                struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };";
    }
    
    private string GenerateShaderVert() {
        return @"
        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = TransformObjectToHClip(v.vertex.xyz);
            o.uv = v.uv;
            return o;
        }";
        
    }
    
    private string GenerateShaderVariables() {
        
        string shaderVariables = "   ";

        foreach (SDFNode s in this.SDFNodes) {
            if (s.variables != null && s.variables.Count > 0) {
                for (int i = 0; i < s.variables.Count; i++) {
                    shaderVariables += ""+ s.types[i] + " " + s.variables[i] + @";
        ";
                }
            }
        }

        return @"
     CBUFFER_START(UnityPerMaterial)
     " + shaderVariables + @"
     float4 insideColor, outsideColor, outlineColor;
     sampler2D insideTex, outsideTex, outlineTex;
     float2 insideTexPosition, outsideTexPosition, outlineTexPosition;
     float insideTexScale, insideTexRotation, outsideTexScale, outsideTexRotation, outlineTexScale, outlineTexRotation, outlineThickness, outlineRepetition, outlineLineDistance;
     CBUFFER_END";
    }

    private string GenerateShaderFrag() {

        string shaderVariables = "";

        for (int j = this.SDFNodes.Count -1; j >= 0; j--) {
            if (this.SDFNodes[j].variables != null && this.SDFNodes[j].variables.Count > 0) {
                for (int i = 0; i < this.SDFNodes[j].variables.Count; i++) {
                    shaderVariables += this.SDFNodes[j].variables[i];
                    shaderVariables += ", ";
                }
            }
                        
        }
        if(shaderVariables.Length>2)
            shaderVariables = shaderVariables.Substring(0, shaderVariables.Length - 2);
        
        string colorVariables = "";
        

        return @"
        float4 frag (v2f i) : SV_Target
        {
            float sdfOut = sdf(i.uv, " + shaderVariables + @");
            
            float4 col = sdfColor(i.uv, sdfOut,
                                  insideColor, insideTex, insideTexPosition, insideTexScale, insideTexRotation, 
                                  outsideColor, outsideTex, outsideTexPosition, outsideTexScale, outsideTexRotation, 
                                  outlineColor, outlineTex, outlineTexPosition, outlineTexScale, outlineTexRotation, 
                                  outlineThickness, outlineRepetition, outlineLineDistance);

            return col;
        }
";
    }
    
    private string GenerateShaderEnd() {
        return @"
            ENDHLSL
        }
    }
}";
    }

    private string GenerateIncludeIfdef() {
        return "#ifndef " + this.shaderName.ToUpper() + @"_INCLUDE
#define " + this.shaderName.ToUpper() + "_INCLUDE";

    }
    
    private string GenerateShaderSdfFunction(SDFNode node) {
        string shaderVariables = "";
        string sdfFunction = "";
        
        for(int j = this.SDFNodes.Count-1; j >= 0; j--){
            if (this.SDFNodes[j] != null) {
                sdfFunction += this.SDFNodes[j].GenerateHlslFunction();
            }

            if (this.SDFNodes[j].variables != null && this.SDFNodes[j].variables.Count > 0) {
                for (int i = 0; i < this.SDFNodes[j].variables.Count; i++) {
                    shaderVariables += this.SDFNodes[j].types[i] + " " + this.SDFNodes[j].variables[i];
                    shaderVariables += ", ";
                }
            }
                        
        }
        shaderVariables = shaderVariables.Substring(0, shaderVariables.Length - 2);

        return @"
    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, " + shaderVariables + @"){ 
         uv -= float2(0.5, 0.5);
        " + sdfFunction + @"

        return " + node.o + @";
    }
        ";
    }

    private string GenerateIncludeSdfColor() {

        return @"
    float4 sdfColor (float2 uv, float sdfOut, 
                     float4 insideColor, sampler2D insideTex, float2 insideTexPosition, float insideTexScale, float insideTexRotation, 
                     float4 outsideColor, sampler2D outsideTex, float2 outsideTexPosition, float outsideTexScale, float outsideTexRotation, 
                     float4 outlineColor, sampler2D outlineTex, float2 outlineTexPosition, float outlineTexScale, float outlineTexRotation, 
                     float outlineThickness, float outlineRepetition, float outlineLineDistance){

        float4 iColor = tex2D(insideTex, transform(insideTexPosition, insideTexRotation, insideTexScale, uv)) * insideColor;
        float4 oColor = tex2D(outsideTex, transform(outsideTexPosition, outsideTexRotation, outsideTexScale, uv)) * outsideColor;
        float4 olColor = tex2D(outlineTex, transform(outlineTexPosition, outlineTexRotation, outlineTexScale, uv)) * outlineColor;

        float sdf = smoothstep(0, outlineThickness *0.01 - outlineThickness*0.005 ,sdfOut);
        float4 col = lerp(iColor ,oColor, sdf);
        float outline = 1-smoothstep(0, outlineThickness*0.01 ,abs(frac(sdfOut / (outlineLineDistance*0.1) + 0.5) - 0.5) * (outlineLineDistance*0.1));
        outline *= step(sdfOut - outlineRepetition *0.01, 0);
        col = lerp(col, olColor, outline);

        return col;
    }";
    }

    private void WriteHlslToText() {
        
        if (this.shaderName == null) {
            this.shaderName = "defaultSDFShader";
        }
        this.pathShaderFile = "Assets/SDFEditor/SDFShader/" + this.shaderName + ".shader";
        this.pathIncludeFile = "Assets/SDFEditor/SDFShader/" + this.shaderName + ".hlsl";
        
        using (StreamWriter sw = File.CreateText(this.pathIncludeFile)) {
            foreach (string s in this.includeStrings) {
                sw.WriteLine(s);
            }
            sw.WriteLine("#endif");
            sw.Close();
        }
        
        using (StreamWriter sw = File.CreateText(this.pathShaderFile)){
            foreach (string s in this.shaderStrings) {
                sw.WriteLine(s);
            }
            sw.Close();
        }
        AssetDatabase.Refresh();
    }
    
    private void ChangeShaderValues(SDFNode node){

        if (this.sdfMaterial == null) {
            Debug.LogWarning("material has not been applied or assigned");
            return;
        }

        Undo.RecordObject(this.sdfMaterial, "changed Material");        

        switch (node.nodeType) {
            case NodeType.Circle: {
                var n = (SDFCircle) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetFloat(n.sdfName + "_radius" , n.Radius);
                break;
            }
            case NodeType.Rect: {
                var n = (SDFRectangle) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_box" , n.Box);
                this.sdfMaterial.SetFloat(n.sdfName + "_scale" , n.Scale);
                this.sdfMaterial.SetVector(n.sdfName + "_roundness" , n.Roundness);
                this.sdfMaterial.SetFloat(n.sdfName + "_rotation", n.Rotation);

                break;
            }
            case NodeType.Triangle: {
                var n = (SDFTriangle) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_a" , n.A);
                this.sdfMaterial.SetVector(n.sdfName + "_b" , n.B);
                this.sdfMaterial.SetVector(n.sdfName + "_c" , n.C);
                this.sdfMaterial.SetFloat(n.sdfName + "_scale", n.Scale);
                this.sdfMaterial.SetFloat(n.sdfName + "_rotation", n.Rotation);

                break;
            }
            case NodeType.Line: {
                var n = (SDFLine) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_a" , n.A);
                this.sdfMaterial.SetVector(n.sdfName + "_b" , n.B);
                this.sdfMaterial.SetFloat(n.sdfName + "_roundness" , n.Roundness);
                this.sdfMaterial.SetFloat(n.sdfName + "_scale", n.Scale);
                this.sdfMaterial.SetFloat(n.sdfName + "_rotation", n.Rotation);
                
                break;
            }
            case NodeType.BezierCurve: {
 
                var n = (SDFBezier) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_a" , n.A);
                this.sdfMaterial.SetVector(n.sdfName + "_b" , n.B);
                this.sdfMaterial.SetVector(n.sdfName + "_c" , n.C);
                this.sdfMaterial.SetFloat(n.sdfName + "_scale", n.Scale);
                this.sdfMaterial.SetFloat(n.sdfName + "_rotation", n.Rotation);
                this.sdfMaterial.SetFloat(n.sdfName + "_roundness" , n.Roundness);

                break;
            }
            case NodeType.Texture: {
                var n = (SDFTexture) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetFloat(n.sdfName + "_scale", n.Scale);
                this.sdfMaterial.SetFloat(n.sdfName + "_rotation", n.Rotation);
                this.sdfMaterial.SetTexture(n.sdfName + "_tex" , n.SdfTexture);
                
                break;
            }
            case NodeType.SBlend: {
                var n = (SDFSBLend) node;
                this.sdfMaterial.SetFloat(n.sdfName + "_k" , n.K);
                break;
            }
            case NodeType.Lerp: {
                var n = (SDFLerp) node;
                this.sdfMaterial.SetFloat(n.sdfName + "_t" , n.T);
                break;
            }
            default: {
                break;
            }
        }
        EditorUtility.SetDirty(this.sdfMaterial);
    }
    private void UpdateColor( ColorChange change){
        if (this.sdfMaterial == null) {
            Debug.LogWarning("material has not been applied or assigned");
            return;
        }

        Undo.RecordObject(this.sdfMaterial, "changed Material");        

        switch (change) {
            case ColorChange.Inside: {
                
                this.sdfMaterial.SetTexture("insideTex" , this.InsideTex);
                this.sdfMaterial.SetColor("insideColor", this.InsideColor);
                this.sdfMaterial.SetVector( "insideTexPosition",this.InsideTexPosition);
                this.sdfMaterial.SetFloat("insideTexScale",this.InsideTexScale);
                this.sdfMaterial.SetFloat("insideTexRotation",this.InsideTexRotation);
                break;
            }
            case ColorChange.Outside: {
                
                this.sdfMaterial.SetTexture("outsideTex" , this.OutsideTex);
                this.sdfMaterial.SetColor("outsideColor", this.OutsideColor);
                this.sdfMaterial.SetVector( "outsideTexPosition",this.OutsideTexPosition);
                this.sdfMaterial.SetFloat("outsideTexScale",this.OutsideTexScale);
                this.sdfMaterial.SetFloat("outsideTexRotation",this.OutsideTexRotation);
                break;
            }
            case ColorChange.Outline: {
                this.sdfMaterial.SetTexture("outlineTex" , this.OutlineTex);
                this.sdfMaterial.SetColor("outlineColor", this.OutlineColor);
                this.sdfMaterial.SetVector( "outlineTexPosition",this.OutlineTexPosition);
                this.sdfMaterial.SetFloat("outlineTexScale",this.OutlineTexScale);
                this.sdfMaterial.SetFloat("outlineTexRotation",this.OutlineTexRotation);

                this.sdfMaterial.SetFloat("outlineThickness", this.Thickness);
                this.sdfMaterial.SetFloat("outlineRepetition" , this.Repetition);
                this.sdfMaterial.SetFloat("outlineLineDistance" , this.LineDistance);
                break;
            }
            default: {
                Debug.LogWarning("unknow Color switch");
                break;
            }
        }
        EditorUtility.SetDirty(this.sdfMaterial);
    }

    public override string GenerateHlslFunction() {
        throw new NotImplementedException();
    }

    public void UpdateActiveNodes() {
        //remove all actions
        foreach (SDFNode s in this.SDFNodes) {
            if (s != null) {
                s.OnValueChange -= this.ChangeShaderValues;

                if (s is SDFFunction) {
                    SDFFunction sfunc = (SDFFunction) s;
                    sfunc.OnInputChange -= this.UpdateShader;
                }
            }
        }

        //get active nodes
        this.SDFNodes.Clear();
        if (this.input is SDFFunction) {
            SDFFunction i = (SDFFunction) this.input;
            i.GetActiveNodes(this.SDFNodes);
        }
        else if(!this.SDFNodes.Contains(this.input)){
            this.SDFNodes.Add(this.input);
        }

        //add actions from active nodes
        foreach (SDFNode s in this.SDFNodes) {
            if (s != null) {
                this.ChangeShaderValues(s);
                s.OnValueChange += this.ChangeShaderValues;
                //Debug.Log("subscribed to value change on " +  s.sdfName);
                if (s is SDFFunction) {
                    SDFFunction sfunc = (SDFFunction) s;
                    sfunc.OnInputChange += this.UpdateShader;
                }
            }
        }
    }
}