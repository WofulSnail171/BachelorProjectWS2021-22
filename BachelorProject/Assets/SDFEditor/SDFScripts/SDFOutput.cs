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
    
    //TODO: create SDFNodeList 
    //TODO: subscribe to action for varibale changes

    private void OnValidate() {
        this.Input = this.input;
        
        if (this.applyMaterial) {
            
            this.ApplyMaterial();
            this.applyMaterial = false;
        }
    }

    private void Awake() {
        this.OnInputChange += this.UpdateShader;
        this.UpdateShader();
    }

    private void ApplyMaterial() {

        this.pathShaderFile = "Assets/SDFEditor/SDFShader/" + this.shaderName + ".shader";
        this.pathIncludeFile = "Assets/SDFEditor/SDFShader/" + this.shaderName + ".hlsl";

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
        if (this.Input != null || this.sdfMaterial != null) {
            
            this.UpdateActiveNodes();
            
            this.AddHlslString(this.Input);
            foreach (SDFNode s in this.SDFNodes) {
                this.ChangeShaderValues(s);
            }

            Debug.Log("updated shader");
        }
        else {
            Debug.LogWarning("forgot to assign Input or Material in output node");
        }
    }

    private void AddHlslString(SDFNode node) {
        
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
        this.includeStrings.Add(this.GenerateShaderSdfFunction(node));
        
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
                case SDFNode.NodeType.Circle: {
                    var n = (SDFCircle) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_radius (""" + n.sdfName + @"_radius"", Float) = 0.2
                ";
                    break;
                }
                case SDFNode.NodeType.Rect: {
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
                case SDFNode.NodeType.Triangle: {
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
                case SDFNode.NodeType.Line: {
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
                case SDFNode.NodeType.BezierCurve: {

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
                case SDFNode.NodeType.Texture: {
                    var n = (SDFTexture) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_position (""" + n.sdfName + @"_position"", Vector) = (0,0,0,0)
                [HideInInspector] " + n.sdfName + @"_tex (""" + n.sdfName + @"_tex"", 2D) = ""white""{}
                ";
                    break;
                }
                case SDFNode.NodeType.Comb: {

                    break;
                }
                case SDFNode.NodeType.Invert: {

                    break;
                }
                case SDFNode.NodeType.SBlend: {
                    var n = (SDFSBLend) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_k (""" + n.sdfName + @"_k"", Float) = 0
                ";
                    break;
                }
                case SDFNode.NodeType.Lerp: {
                    var n = (SDFLerp) node;
                    properties += @"
                [HideInInspector] " + n.sdfName + @"_t (""" + n.sdfName + @"_t"", Float) = 0
                ";
                    break;
                }
                default: {
                    Debug.LogWarning("unknow node");
                    break;
                }
            }
        }

        return @"
            Properties
            {
                " + properties + @"
                [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest(""ZTest"", Float) = 0
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
        
        string variables = "   ";

        foreach (SDFNode s in this.SDFNodes) {
            if (s.variables != null && s.variables.Count > 0) {
                for (int i = 0; i < s.variables.Count; i++) {
                    variables += ""+ s.types[i] + " " + s.variables[i] + @";
        ";
                }
            }
            else {
                Debug.Log(s.sdfName + " has no valid variables");
            }
        }

        return @"
     CBUFFER_START(UnityPerMaterial)
     " + variables + @"
     CBUFFER_END";
    }

    private string GenerateShaderFrag() {

        string variables = "";

        for (int j = this.SDFNodes.Count -1; j >= 0; j--) {
            if (this.SDFNodes[j].variables != null && this.SDFNodes[j].variables.Count > 0) {
                for (int i = 0; i < this.SDFNodes[j].variables.Count; i++) {
                    variables += this.SDFNodes[j].variables[i];
                    variables += ", ";
                }
            }
                        
        }
        variables = variables.Substring(0, variables.Length - 2);
        
        
        Debug.Log(variables);
        return @"
        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv, " + variables + @");
            
            float4 col = smoothstep(0, 0.01, sdfOut);
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
        string variables = "";
        string sdfFunction = "";
        
        for(int j = this.SDFNodes.Count-1; j >= 0; j--){
            if (this.SDFNodes[j] != null) {
                sdfFunction += this.SDFNodes[j].GenerateHlslFunction();
            }

            if (this.SDFNodes[j].variables != null && this.SDFNodes[j].variables.Count > 0) {
                for (int i = 0; i < this.SDFNodes[j].variables.Count; i++) {
                    variables += this.SDFNodes[j].types[i] + " " + this.SDFNodes[j].variables[i];
                    variables += ", ";
                }
            }
                        
        }
        variables = variables.Substring(0, variables.Length - 2);

        return @"
    
    float dot2( in float2 v ) { return dot(v,v); }

    float2 transform (float2 p, float r, float s, float2 uv){
        float2x2 rot = {cos(r), -sin(r),
                      sin(r), cos(r)};
        float2 t = mul(rot, (uv-p) * 1/ s);
        return t;
    }

    float sdf (float2 uv, " + variables + @"){ 
        " + sdfFunction + @"

        return " + node.o + @";
    }
        ";
    }

    private void WriteHlslToText() {
        
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
        
        Debug.Log("changing shader variables from " + node.sdfName);

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
            case NodeType.Comb: {

                break;
            }
            case NodeType.Invert: {

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
                Debug.LogWarning("unknow node");
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