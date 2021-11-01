using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "SDF/Triangle")]
public class SDFTriangle : SDFScriptableObject {
    [SerializeField] private Vector2 a;
    [SerializeField] private Vector2 b;
    [SerializeField] private Vector2 c;
    [SerializeField] private float scale;

    public override string SDFFunction() {
        this.o = this.SDFName + "_out";
        string hlslString ="float2 e0 = float2" + this.b + " - float2" + this.a + @";
    float2 e1 = float2" + this.c + " - float2" + this.b + @";
    float2 e2 = float2" + this.a + " - float2" + this.c + @";
    
    float2 uv_" + this.SDFName + " = 1/" + this.scale + " * uv - float2" + this.Position + @";
    
    float2 v0 = uv_" + this.SDFName + " - float2" + a + @";
    float2 v1 = uv_" + this.SDFName + " - float2" + b + @";
    float2 v2 = uv_" + this.SDFName + " - float2" + c + @";
    
    float2 pq0 = v0 - e0 * clamp( dot(v0,e0)/dot(e0,e0), 0.0, 1.0 );
    float2 pq1 = v1 - e1 * clamp( dot(v1,e1)/dot(e1,e1), 0.0, 1.0 );
    float2 pq2 = v2 - e2 * clamp( dot(v2,e2)/dot(e2,e2), 0.0, 1.0 );
    
    float s = sign( e0.x*e2.y - e0.y*e2.x ) ;
    float2 d = min(min(float2(dot(pq0,pq0), s*(v0.x*e0.y-v0.y*e0.x)),
                       float2(dot(pq1,pq1), s*(v1.x*e1.y-v1.y*e1.x))),
                       float2(dot(pq2,pq2), s*(v2.x*e2.y-v2.y*e2.x)));
    float "+ this.o + "= -sqrt(d.x) * sign(d.y) * " + this.scale +";";
        
        return hlslString;
    }
}
