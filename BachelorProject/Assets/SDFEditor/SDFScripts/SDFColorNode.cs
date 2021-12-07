using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SDFColorNode :ScriptableObject
{
    [HideInInspector] public string o;
    [HideInInspector] public string sdfName = "newSDF";

    protected uint index;

    [HideInInspector]public List<string> variables = new List<string>();
    [HideInInspector]public List<string> types = new List<string>();

    protected bool isDirty;
    public Action<SDFColorNode> OnValueChange;
    public Action OnInputChange;

    public enum ColorNodeType {
        ColorOutput,
        Color,
        Texture
    }

    [HideInInspector]public ColorNodeType colorNodeType;

    public abstract string GenerateHlslFunction();

    public void GetActiveNodes(List<SDFColorNode> nodes, SDFColorNode input) {

        if (input is SDFColorOutput) {
            SDFColorOutput i = (SDFColorOutput) input;
            i.GetActiveNodes(nodes);
        }
        else {
            foreach (SDFColorNode s in nodes) {
                if (s.sdfName == input.sdfName) {
                    return;
                }
            }
            nodes.Add(input);
        }
    }
}
