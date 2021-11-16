using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteAlways][CreateAssetMenu(menuName = "SDF Output/Output")]
public class SDFOutput : SDFFunction{

    [SerializeField] private string shaderName; 
        
    private string pathShaderFile;
    private string pathIncludeFile;

    public SDFNode input;
    public Material sdfMaterial;
    private Shader sdfShader;
    
    public bool apply;
    
    private List<string> shaderStrings = new List<string>();
    private List<string> includeStrings = new List<string>();
    private List <SDFNode>  SDFNodes = new List<SDFNode>();
    
    //TODO: create SDFNodeList 
    //TODO: subscribe to action for varibale changes

    private void OnValidate() {
        if (this.apply) {
            this.Apply();

            this.apply = false;
        }
    }

    private void Apply() {

        pathShaderFile = "Assets/Shader/" + this.shaderName + ".shader";
        pathIncludeFile = "Assets/Shader/" + this.shaderName + ".hlsl";

        if (this.input != null) {
            this.AddHlslString(this.input);

            this.UpdateActiveNodes();
        }

        if (this.sdfMaterial == null){
            sdfShader = Shader.Find("SDF/" + this.shaderName);
            if (sdfShader != null) {
                this.sdfMaterial = new Material("SDFMaterial");
                this.sdfMaterial.shader = this.sdfShader;
            }
            else{
                Debug.LogWarning("shader could not be found");
            }
        }
        else if(this.sdfMaterial.shader != this.sdfShader) {
            this.sdfMaterial.shader = this.sdfShader;
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
        return @"
            Properties
            {
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
        string variables = "";

        for (int i = 0; i < this.input.variables.Count; i++) {
            variables += this.input.types[i] + " " + this.input.variables[i] + @";
    ";
        }
        Debug.Log(variables);
        return @"
CBUFFER_START(UnityPerMaterial)
   " + variables + @"
CBUFFER_END";
    }

    private string GenerateShaderFrag() {

        string variables = "";
        
        for (int i = 0; i < this.input.variables.Count; i++) {
            variables += this.input.variables[i];
            if (i != this.input.variables.Count - 1) {
                variables += ", ";
            }
        }
        
        return @"
        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv," + variables + @");
            float4 col = smoothstep(0, 0.01, abs(sdfOut));
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

        for (int i = 0; i < this.input.variables.Count; i++) {
            variables += this.input.types[i] + " " + this.input.variables[i];
            if (i != this.input.variables.Count - 1) {
                variables += ", ";
            }
        }
        string sdfFunction = @"
    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, " + variables + @"){ 
        " + node.GenerateHlslFunction() + @"

         return " + node.o + @";
        }
        ";
        
        return sdfFunction;
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
    }
    
    private void ChangeShaderValues(SDFNode node){

        switch (node.nodeType) {
            case SDFNode.NodeType.Circle: {
                var n = (SDFCircle) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetFloat(n.sdfName + "_radius" , n.Radius);
                break;
            }
            case SDFNode.NodeType.Rect: {
                var n = (SDFRectangle) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_box" , n.Box);
                this.sdfMaterial.SetFloat(n.sdfName + "_scale" , n.Scale);
                this.sdfMaterial.SetVector(n.sdfName + "_roundness" , n.Roundness);

                break;
            }
            case SDFNode.NodeType.Triangle: {
                var n = (SDFTriangle) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_a" , n.A);
                this.sdfMaterial.SetVector(n.sdfName + "_b" , n.B);
                this.sdfMaterial.SetVector(n.sdfName + "_c" , n.C);
                this.sdfMaterial.SetFloat(n.sdfName + "_scale", n.Scale);

                break;
            }
            case SDFNode.NodeType.Line: {
                var n = (SDFLine) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_a" , n.A);
                this.sdfMaterial.SetVector(n.sdfName + "_b" , n.B);
                this.sdfMaterial.SetFloat(n.sdfName + "_roundness" , n.Roundness);
                
                break;
            }
            case SDFNode.NodeType.BezierCurve: {
 
                var n = (SDFBezier) node;
                this.sdfMaterial.SetVector(n.sdfName + "_position", n.Position);
                this.sdfMaterial.SetVector(n.sdfName + "_a" , n.A);
                this.sdfMaterial.SetVector(n.sdfName + "_b" , n.B);
                this.sdfMaterial.SetVector(n.sdfName + "_c" , n.C);
                
                break;
            }
            case SDFNode.NodeType.Texture: {
                var n = (SDFTexture) node;
                this.sdfMaterial.SetTexture(n.sdfName + "_tex" , n.SdfTexture);
                
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
                this.sdfMaterial.SetFloat(n.sdfName + "_k" , n.K);
                break;
            }
            case SDFNode.NodeType.Lerp: {
                var n = (SDFLerp) node;
                this.sdfMaterial.SetFloat(n.sdfName + "_t" , n.T);
                break;
            }
        }
    }

    public override string GenerateHlslFunction() {
        throw new NotImplementedException();
    }

    public override void GetActiveNodes(List<SDFNode> nodes) {
        throw new NotImplementedException();
    }

    public void UpdateActiveNodes() {
        //remove all actions
        foreach (SDFNode s in this.SDFNodes) {
            s.OnValueChange -= this.ChangeShaderValues;

            if (s is SDFFunction) {
                SDFFunction sfunc = (SDFFunction) s;
                sfunc.OnInputChange -= this.UpdateActiveNodes;
            }
        }
        //get active nodes
        if (this.input is SDFFunction) {
            SDFFunction i = (SDFFunction) this.input;
            i.GetActiveNodes(this.SDFNodes);
        }
        else {
            nodes.Add(this.input);
        }
        //add actions from active nodes
        foreach (SDFNode s in this.SDFNodes) {
            ChangeShaderValues(s);
            s.OnValueChange += this.ChangeShaderValues;

            if (s is SDFFunction) {
                SDFFunction sfunc = (SDFFunction) s;
                sfunc.OnInputChange += this.UpdateActiveNodes;
            }
        }
    }
}