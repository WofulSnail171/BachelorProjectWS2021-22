using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteAlways]
public class SDFManager : MonoBehaviour{
    private List<string> hlslStrings = new List<string>();
    private List <SDFScriptableObject>  SDFObjects = new List<SDFScriptableObject>();

    private TextAsset hlslInclude;
    private string path = @"Assets/Shader/TestShader.shader";

    public SDFNode sdfNode;
    public bool apply;

    private void Update() {
        if (apply) {
            this.Apply();
            Debug.Log("updated");
            apply = false;
        }
    }

    private void Apply() {

        this.hlslStrings.Clear();

        path = @"Assets/Shader/TestShader.shader";

        if(this.sdfNode != null)
            AddHlslString(this.sdfNode);
        WriteHlslToText();
    }

    private void AddHlslString(SDFNode node) {
        
        //////////// Generate Shader Code //////////////
        this.hlslStrings.Clear();
        this.hlslStrings.Add(this.GenerateShaderName());
        this.hlslStrings.Add(this.GenerateShaderProperties());
        this.hlslStrings.Add(this.GenerateShaderTags());
        this.hlslStrings.Add(this.GenerateShaderPass());
        this.hlslStrings.Add(this.GenerateShaderVert());
        this.hlslStrings.Add(this.GenerateShaderVariables());
        this.hlslStrings.Add(this.GenerateShaderSdfFunction(node));
        this.hlslStrings.Add(this.GenerateShaderFrag());
        this.hlslStrings.Add(this.GenerateShaderEnd());
        
        this.WriteHlslToText();
       
    }

    string GenerateShaderName() {
        return @"Shader ""SDF/TestShader""
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
    
    string GenerateShaderVert() {
        return @"
        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = TransformObjectToHClip(v.vertex);
            o.uv = v.uv;
            return o;
        }";
        
    }
    
    string GenerateShaderVariables() {
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

    string GenerateShaderSdfFunction(SDFNode node) {
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
        " + node.SDFFunction() + @"

         return " + node.o + @";
        }
        ";
        
        return sdfFunction;
    }
    
    string GenerateShaderFrag() {
        
       return @"
        float4 frag (v2f i) : SV_Target
        {
            
            //float4 col = float4(sdf(i.uv, );
            float4 col = float4(1,1,1,1);
            return col;
        }
";
    }
    
    string GenerateShaderEnd() {
        return @"
            ENDHLSL
        }
    }
}";
    }



    private void WriteHlslToText() {
        
        using (StreamWriter sw = File.CreateText(this.path)){
            foreach (string s in this.hlslStrings) {
                sw.WriteLine(s );
            }
            sw.Close();
        }
    }
}
