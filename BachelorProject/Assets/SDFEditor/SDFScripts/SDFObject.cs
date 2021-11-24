
using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SDFObject : SDFNode {

    private Vector2 _position;
    public Vector2 position = new Vector2(0, 0);

    public Vector2 Position {
        get => this._position;
        set {
            if (this._position == value) {return;}
            this._position = value;
            this.isDirty = true;
        }
    }

}