Shader "SDF/test2"
        {

            Properties
            {
                
                [HideInInspector] lerp305_t ("lerp305_t", Float) = 0
                
                [HideInInspector] tex829_position ("tex829_position", Vector) = (0,0,0,0)
                [HideInInspector] tex829_tex ("tex829_tex", 2D) = "white"{}
                
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
   float lerp305_t;
    float2 tex829_position;
    sampler2D tex829_tex;
    float2 circle429_position;
    float circle429_radius;
    
CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv,lerp305_t, tex829_position, tex829_tex, circle429_position, circle429_radius);
            float4 col = smoothstep(0, 0.01, abs(sdfOut));
            return col;
        }


            ENDHLSL
        }
    }
}