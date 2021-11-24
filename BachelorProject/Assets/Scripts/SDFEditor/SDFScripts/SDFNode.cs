using System;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public abstract class SDFNode : ScriptableObject
{
    [HideInInspector] public string o;
    [HideInInspector] public string sdfName = "newSDF";

    protected uint index;

    [HideInInspector]public List<string> variables = new List<string>();
    [HideInInspector]public List<string> types = new List<string>();

    [HideInInspector] public List<ScriptableObject> nodes;

    protected bool isDirty;
    public Action<SDFNode> OnValueChange;
    public Action OnInputChange;

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

    public abstract string GenerateHlslFunction();
}