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

    [HideInInspector] public List<ScriptableObject> nodes;

    public abstract string SdfFunction();
    
    //TODO: create update event for variables on editor change -> OnValidate()
    //TODO: check for node changes
    ////TODO: create event for unsubscribe 


    private void OnValidate() {
        //call event update variable
        //check for change in parent node(s)
        
    }
    
}