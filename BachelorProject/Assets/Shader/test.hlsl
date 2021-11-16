#ifndef TEST_INCLUDE
#define TEST_INCLUDE

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 triangle999_position, float2 triangle999_a, float2 triangle999_b, float2 triangle999_c, float triangle999_scale){ 
        float2 e0 = triangle999_b - triangle999_a;
    float2 e1 = triangle999_c - triangle999_b;
    float2 e2 = triangle999_a - triangle999_c;
    
    float2 uv_triangle999 = 1/triangle999_scale * uv - triangle999_position;
    
    float2 v0 = uv_triangle999 - triangle999_a;
    float2 v1 = uv_triangle999 - triangle999_b;
    float2 v2 = uv_triangle999 - triangle999_c;
    
    float2 pq0 = v0 - e0 * clamp( dot(v0,e0)/dot(e0,e0), 0.0, 1.0 );
    float2 pq1 = v1 - e1 * clamp( dot(v1,e1)/dot(e1,e1), 0.0, 1.0 );
    float2 pq2 = v2 - e2 * clamp( dot(v2,e2)/dot(e2,e2), 0.0, 1.0 );
    
    float s = sign( e0.x*e2.y - e0.y*e2.x ) ;
    float2 d = min(min(float2(dot(pq0,pq0), s*(v0.x*e0.y-v0.y*e0.x)),
                       float2(dot(pq1,pq1), s*(v1.x*e1.y-v1.y*e1.x))),
                       float2(dot(pq2,pq2), s*(v2.x*e2.y-v2.y*e2.x)));
    float triangle999_out= -sqrt(d.x) * sign(d.y) * 0.86;

         return triangle999_out;
        }
        
#endif
