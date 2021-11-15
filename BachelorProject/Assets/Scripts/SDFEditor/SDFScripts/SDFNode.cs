using System;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public abstract class SDFNode : ScriptableObject
{
    [HideInInspector] public string o;
    [HideInInspector] public string sdfName = "newSDF";

    protected uint index;

    [HideInInspector]public List<string> variables;
    [HideInInspector]public List<string> types;

    [HideInInspector] public List<ScriptableObject> nodes;

    protected bool isDirty;
    public Action<SDFNode> OnValueChange;

    public enum NodeType {
        Circle,
        Rect,
        Triangle, 
        Line,
        BezierCurve,
        Texture,
        Comb,
        Invert,
        SBlend,
        Lerp
    }

    [HideInInspector]public NodeType nodeType;

    public abstract string SdfFunction();
    
    //TODO: create update event for variables on editor change -> OnValidate()
    //TODO: check for node changes
    ////TODO: create event for unsubscribe 

}