using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SDFFunction : SDFNode {
    
    public Action OnInputChange;

    public abstract void GetActiveNodes(List<SDFNode> nodes);
    public abstract void GenerateVariables();
}


