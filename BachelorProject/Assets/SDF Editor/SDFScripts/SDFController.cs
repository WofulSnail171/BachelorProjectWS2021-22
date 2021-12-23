using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDFController : MonoBehaviour {
    
    public SDFOutput output;
    
    void Start() {
        this.output = ScriptableObject.CreateInstance<SDFOutput>();
        this.output.InsideColor = new Vector4(1, 0, 0, 1);
    }
    
    void Update() {
        float b = (Mathf.Sin(Time.time) + 1)*0.5f;
        this.output.InsideColor = new Vector4(1, b, b, 1);
    }
}
