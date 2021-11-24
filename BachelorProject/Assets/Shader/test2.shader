Shader "SDF/test2"
        {

            Properties
            {
                
                [HideInInspector] lerp96_t ("lerp96_t", Float) = 0
                
                [HideInInspector] rect550_position ("rect550_position", Vector) = (0,0,0,0)
                [HideInInspector] rect550_box ("rect550_box", Vector) = (0,0,0,0)
                [HideInInspector] rect550_scale ("rect550_scale", Float) = 0
                [HideInInspector] rect550_roundness ("rect550_roundness", Vector) = (0,0,0,0)
                
                [HideInInspector] circle686_position ("circle686_position", Vector) = (0,0,0,0)
                [HideInInspector] circle686_radius ("circle686_radius", Float) = 0
                
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
   float lerp96_t;
    float2 rect550_position;
    float2 rect550_box;
    float rect550_scale;
    float4 rect550_roundness;
    float2 circle686_position;
    float circle686_radius;
    
CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv,lerp96_t, rect550_position, rect550_box, rect550_scale, rect550_roundness, circle686_position, circle686_radius);
            float4 col = smoothstep(0, 0.01, abs(sdfOut));
            return col;
        }


            ENDHLSL
        }
    }
}
