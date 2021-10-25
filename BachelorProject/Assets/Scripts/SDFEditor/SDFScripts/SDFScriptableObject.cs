
using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SDFScriptableObject : ScriptableObject {

    [SerializeField] private new string name = "newSDF";
    [SerializeField] private string functionName = "new Function Name";
    
    [SerializeField] private Vector2 position = new Vector2(0, 0);
    
    [HideInInspector] public string o;

    
    public string Name => this.name;
    public string FunctionName => this.functionName;
    public Vector2 Position => this.position;

    public abstract string SDFFunction();
}