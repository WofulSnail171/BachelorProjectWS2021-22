using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public abstract class SDFNode : ScriptableObject
{
    [HideInInspector] public string o;
    protected string sdfName = "newSDF";
    
    protected uint index;

    [HideInInspector]public List<string> variables;
    [HideInInspector]public List<string> types;

    public abstract string SDFFunction();
}
