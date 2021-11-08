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
    private string path = @"Assets/Shader/HlslInclude.shader";

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
        this.hlslStrings.Add("#ifndef SDFFUNCTIONS_INCLUDED");
        this.hlslStrings.Add("#define SDFFUNCTIONS_INCLUDED");
        this.hlslStrings.Add("float dot2( in float2 v ) { return dot(v,v); }");
        
        path = @"Assets/Shader/TestShader.shader";

        if(this.sdfNode != null)
            AddHlslString(this.sdfNode);
        WriteHlslToText();
    }

    private void AddHlslString(SDFNode node) {
        string variables = "";

        for (int i = 0; i < this.sdfNode.variables.Count; i++) {
            variables += this.sdfNode.types[i] + " " + this.sdfNode.variables[i] + ", ";
        }
        

        string hlsl = @"
void SDF_float (float2 uv, " + variables + @"out float Out){ 
    " + node.SDFFunction() + @"

    Out = " + node.o + @";
}
";
        
        this.hlslStrings.Add(hlsl);
        this.hlslStrings.Add(node.SDFFunction());
    }

    private void WriteHlslToText() {
        
        using (StreamWriter sw = File.CreateText(this.path)){
            foreach (string s in this.hlslStrings) {
                sw.WriteLine(s );
            }

            sw.WriteLine("#endif");
            sw.Close();
        }
    }
}
