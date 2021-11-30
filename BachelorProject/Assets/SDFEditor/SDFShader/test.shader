Shader "SDF/test"
        {

            Properties
            {
                
                [HideInInspector] lerp979_t ("lerp979_t", Float) = 0
                
                [HideInInspector] circle125_position ("circle125_position", Vector) = (0,0,0,0)
                [HideInInspector] circle125_radius ("circle125_radius", Float) = 0
                
                [HideInInspector] rect766_position ("rect766_position", Vector) = (0,0,0,0)
                [HideInInspector] rect766_box ("rect766_box", Vector) = (0,0,0,0)
                [HideInInspector] rect766_scale ("rect766_scale", Float) = 0
                [HideInInspector] rect766_roundness ("rect766_roundness", Vector) = (0,0,0,0)
                
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
            #include "test.hlsl"
            
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
        float lerp979_t;
        float2 circle125_position;
        float circle125_radius;
        float2 rect766_position;
        float2 rect766_box;
        float rect766_scale;
        float4 rect766_roundness;
        
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv, rect766_position, rect766_box, rect766_scale, rect766_roundness, circle125_position, circle125_radius, lerp979_t);
            
            float4 col = smoothstep(0, 0.01, sdfOut);
            return col;
        }


            ENDHLSL
        }
    }
}
