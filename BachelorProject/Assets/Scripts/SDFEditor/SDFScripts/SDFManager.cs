using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SDFManager : MonoBehaviour{
    private List<string> hlslStrings = new List<string>();
    private List <SDFScriptableObject>  SDFObjects = new List<SDFScriptableObject>();

    private TextAsset hlslInclude;
    [HideInInspector]public string path = @"Assets/Shader/SDFInclude.hlsl";

    public SDFCombine sdfCombine;
    
    private void OnValidate() {

        this.hlslStrings.Clear();
        this.hlslStrings.Add("#ifndef SDFFUNCTIONS_INCLUDED");
        this.hlslStrings.Add("#define SDFFUNCTIONS_INCLUDED");
        this.hlslStrings.Add("float dot2( in float2 v ) { return dot(v,v); }");
        
        
        if(sdfCombine != null)
            AddHlslString(sdfCombine);
        WriteHlslToText();
    }

    private void AddHlslString(SDFCombine comb) {
        string hlsl =
            @"void SDF_float (float2 uv,
             out float Out){ 
                " + comb.Combine() + @"
                Out =" + comb.o + "; }";
        
        this.hlslStrings.Add(hlsl);
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
