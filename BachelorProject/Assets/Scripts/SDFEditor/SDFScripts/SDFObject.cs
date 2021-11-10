
using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SDFObject : SDFNode {
    
    public Vector2 position = new Vector2(0, 0);

    public Vector2 Position {
        get => this.position;
        set {
            if (this.position == value) {return;}
            this.position = value;
            this.OnValueChange?.Invoke(this);
        }
    }

}