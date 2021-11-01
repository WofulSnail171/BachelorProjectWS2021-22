using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SDFNode : ScriptableObject
{
    [SerializeField] private string sdfName = "newSDF";

    [HideInInspector] public string o;

    private uint index;
    protected string SDFName => this.sdfName + this.index;

    public abstract string SDFFunction();

    private void OnValidate() {
        this.index = (uint)Random.Range(0, 1000);
    }
}
