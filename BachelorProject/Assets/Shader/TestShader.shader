Shader "SDF/TestShader"
        {

            Properties
            {
               
            }

            SubShader
            {
            Tags { "RenderType"="Opaque" }
            LOD 100

            Pass
            {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
                        
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"       
            
                struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = TransformObjectToHClip(v.vertex);
            o.uv = v.uv;
            return o;
        }

CBUFFER_START(UnityPerMaterial)
float2 rect998_position;
float2 rect998_box;
float rect998_scale;
float4 rect998_roundness;

CBUFFER_END

    
    float dot2( in float2 v ) { return dot(v,v); }

    float sdf (float2 uv, float2 rect998_position, float2 rect998_box, float rect998_scale, float4 rect998_roundness){ 
        rect998_roundness.xy = (rect998_position.x - uv.x > 0.0) ? rect998_roundness.xy : rect998_roundness.zw;
    rect998_roundness.x  = (rect998_position.y - uv.y > 0.0) ? rect998_roundness.x  : rect998_roundness.y;
    float2 q = abs(rect998_position - uv) - rect998_box + rect998_roundness.x;
    float rect998_out = min(max(q.x,q.y),0.0) + length(max(q,0.0)) - rect998_roundness.x;

         return rect998_out;
        }
        

        float4 frag (v2f i) : SV_Target
        {
            
            //ffloat4 col = fixed4(sdf(i.uv, );
            float4 col = float4(1,1,1,1);
            return col;
        }


            ENDHLSL
        }
    }
}
