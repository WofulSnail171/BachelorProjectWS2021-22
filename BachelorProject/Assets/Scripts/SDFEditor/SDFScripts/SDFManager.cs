using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteAlways]
public class SDFManager : MonoBehaviour{

    [SerializeField] private string shaderName; 
        
    private string pathShaderFile;
    private string pathIncludeFile;

    public SDFNode sdfNode;
    public bool apply;
    
    private List<string> shaderStrings = new List<string>();
    private List<string> includeStrings = new List<string>();
    //private List <SDFScriptableObject>  SDFObjects = new List<SDFScriptableObject>();
    
    //TODO: create SDFNodeList 
    //TODO: subscribe to action for varibale changes
    

    private void Update() {
        if (this.apply) {
            this.Apply();
            Debug.Log("updated");
            this.apply = false;
        }
    }

    private void Apply() {

        pathShaderFile = "Assets/Shader/" + this.shaderName + ".shader";
        pathIncludeFile = "Assets/Shader/" + this.shaderName + ".hlsl";

        if(this.sdfNode != null)
            this.AddHlslString(this.sdfNode);
        this.WriteHlslToText();
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
               
            }";
    }
    
    string GenerateShaderTags() {
        return@"
            SubShader
            {
            Tags { ""RenderType""=""Opaque"" }
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

        for (int i = 0; i < this.sdfNode.variables.Count; i++) {
            variables += this.sdfNode.types[i] + " " + this.sdfNode.variables[i] + @";
    ";
        }

        return @"
CBUFFER_START(UnityPerMaterial)
   " + variables + @"
CBUFFER_END";
    }

    private string GenerateShaderFrag() {

        string variables = "";
        
        for (int i = 0; i < this.sdfNode.variables.Count; i++) {
            variables += this.sdfNode.variables[i];
            if (i != this.sdfNode.variables.Count - 1) {
                variables += ", ";
            }
        }
        
        return @"
        float4 frag (v2f i) : SV_Target
        {
            
            float sdfOut = sdf(i.uv," + variables + @");
            float4 col = float4(1,1,1,1);
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

        for (int i = 0; i < this.sdfNode.variables.Count; i++) {
            variables += this.sdfNode.types[i] + " " + this.sdfNode.variables[i];
            if (i != this.sdfNode.variables.Count - 1) {
                variables += ", ";
            }
        }
        string sdfFunction = @"
    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, " + variables + @"){ 
        " + node.SdfFunction() + @"

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
}
