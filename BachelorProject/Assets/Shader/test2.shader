Shader "SDF/test2"
        {

            Properties
            {
                
                [HideInInspector] sblend571_k ("sblend571_k", Float) = 0
                
                [HideInInspector] lerp305_t ("lerp305_t", Float) = 0
                
                [HideInInspector] rect983_position ("rect983_position", Vector) = (0,0,0,0)
                [HideInInspector] rect983_box ("rect983_box", Vector) = (0,0,0,0)
                [HideInInspector] rect983_scale ("rect983_scale", Float) = 0
                [HideInInspector] rect983_roundness ("rect983_roundness", Vector) = (0,0,0,0)
                
                [HideInInspector] bezier796_position ("bezier796_position", Vector) = (0,0,0,0)
                [HideInInspector] bezier796_a ("bezier796_a", Vector) = (0,0,0,0)
                [HideInInspector] bezier796_b ("bezier796_b", Vector) = (0,0,0,0)
                [HideInInspector] bezier796_c ("bezier796_c", Vector) = (0,0,0,0)
                
                [HideInInspector] circle429_position ("circle429_position", Vector) = (0,0,0,0)
                [HideInInspector] circle429_radius ("circle429_radius", Float) = 0
                
                [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
                [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend mode", Float) = 1
                [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destination Blend mode", Float) = 1
            }

            SubShader
            {
            Tags { "RenderType"="Opaque" 
                   "RenderPipeline"="UniversalRenderPipeline"
                 }
            LOD 100

            Pass
            {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
                        
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"       
            #include "test2.hlsl"
            
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
            o.vertex = TransformObjectToHClip(v.vertex.xyz);
            o.uv = v.uv;
            return o;
        }

CBUFFER_START(UnityPerMaterial)
   float sblend571_k;
    float lerp305_t;
    float2 rect983_position;
    float2 rect983_box;
    float rect983_scale;
    float4 rect983_roundness;
    float2 bezier796_position;
    float2 bezier796_a;
    float2 bezier796_b;
    float2 bezier796_c;
    float2 circle429_position;
    float circle429_radius;
    
CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv,sblend571_k, lerp305_t, rect983_position, rect983_box, rect983_scale, rect983_roundness, bezier796_position, bezier796_a, bezier796_b, bezier796_c, circle429_position, circle429_radius);
            float4 col = smoothstep(0, 0.01, abs(sdfOut));
            return col;
        }


            ENDHLSL
        }
    }
}
