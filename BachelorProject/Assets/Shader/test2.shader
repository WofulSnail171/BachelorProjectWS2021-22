Shader "SDF/test2"
        {

            Properties
            {
                
                [HideInInspector] lerp771_t ("lerp771_t", Float) = 0
                
                [HideInInspector] circle429_position ("circle429_position", Vector) = (0,0,0,0)
                [HideInInspector] circle429_radius ("circle429_radius", Float) = 0
                
                [HideInInspector] rect983_position ("rect983_position", Vector) = (0,0,0,0)
                [HideInInspector] rect983_box ("rect983_box", Vector) = (0,0,0,0)
                [HideInInspector] rect983_scale ("rect983_scale", Float) = 0
                [HideInInspector] rect983_roundness ("rect983_roundness", Vector) = (0,0,0,0)
                
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
   float lerp45_t;
    float2 circle199_position;
    float circle199_radius;
    float2 rect773_position;
    float2 rect773_box;
    float rect773_scale;
    float4 rect773_roundness;
    float lerp771_t;
    float2 circle429_position;
    float circle429_radius;
    float2 rect983_position;
    float2 rect983_box;
    float rect983_scale;
    float4 rect983_roundness;
    
CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv,lerp45_t, circle199_position, circle199_radius, rect773_position, rect773_box, rect773_scale, rect773_roundness, lerp771_t, circle429_position, circle429_radius, rect983_position, rect983_box, rect983_scale, rect983_roundness);
            float4 col = smoothstep(0, 0.01, abs(sdfOut));
            return col;
        }


            ENDHLSL
        }
    }
}
