
using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SDFScriptableObject : SDFNode {
    
    [SerializeField] private Vector2 position = new Vector2(0, 0);

    protected Vector2 Position => this.position;

}