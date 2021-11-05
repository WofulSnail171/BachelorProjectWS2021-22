using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SDFNode : ScriptableObject
{
    [HideInInspector] public string o;
    protected string sdfName = "newSDF";
    
    protected uint index;

    public List<string> variables;
    public List<string> types;

    public abstract string SDFFunction();

    private void OnValidate() {
        this.index = (uint)Random.Range(0, 1000);
    }
}
