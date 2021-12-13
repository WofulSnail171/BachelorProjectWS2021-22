using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leanTweenTest : MonoBehaviour {
    private Renderer rend;

    private void Start() {
        rend = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void tweentest() {
        float x = 1;
        LeanTween.value(this.transform.position.x, 0, 1f)
            .setOnUpdate(this.dostuff)
            .setEaseShake();

        
    }

    private void dostuff(float value) {
        
    }
    
}
