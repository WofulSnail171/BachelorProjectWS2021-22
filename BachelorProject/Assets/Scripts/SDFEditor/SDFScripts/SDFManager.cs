using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SDFManager : MonoBehaviour{
    private List<string> hlslStrings = new List<string>();
    private List <SDFScriptableObject>  SDFObjects = new List<SDFScriptableObject>();

    private TextAsset hlslInclude;
    string path = "Assets/Shader/SDFInclude.txt";

    public SDFScriptableObject sdf;

    private void OnValidate() {

        if (this.hlslStrings.Count < 3) {
            this.hlslStrings.Add("#ifndef SDFFUNCTIONS_INCLUDED");
            this.hlslStrings.Add("#def SDFFUNCTIONS_INCLUDED");
            this.hlslStrings.Add("float dot2( in float2 v ) { return dot(v,v); }");
        }

        if(sdf != null)
            AddHlslString(sdf);
        WriteHlslToText();
    }

    private void AddHlslString(SDFScriptableObject so) {
        string hlsl =
            "void " + so.FunctionName + @"_float float2 uv,
             out float Out)
            { Out = " + so.SDFFunction() + "}";
        
        this.hlslStrings.Add(hlsl);
    }

    private void WriteHlslToText() {
        
        using (StreamWriter sw = File.CreateText(path)) {

            foreach (string s in this.hlslStrings) {
                sw.WriteLine(s);
            }

            sw.WriteLine("#endif");
            sw.Close();
        }
    }
}
