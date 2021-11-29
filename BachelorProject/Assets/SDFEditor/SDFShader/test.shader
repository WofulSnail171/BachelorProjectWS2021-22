Shader "SDF/test"
        {

            Properties
            {
                
                [HideInInspector] lerp385_t ("lerp385_t", Float) = 0
                
                [HideInInspector] circle959_position ("circle959_position", Vector) = (0,0,0,0)
                [HideInInspector] circle959_radius ("circle959_radius", Float) = 0
                
                [HideInInspector] rect494_position ("rect494_position", Vector) = (0,0,0,0)
                [HideInInspector] rect494_box ("rect494_box", Vector) = (0,0,0,0)
                [HideInInspector] rect494_scale ("rect494_scale", Float) = 0
                [HideInInspector] rect494_roundness ("rect494_roundness", Vector) = (0,0,0,0)
                
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
        float lerp385_t;
        float2 circle959_position;
        float circle959_radius;
        float2 rect494_position;
        float2 rect494_box;
        float rect494_scale;
        float4 rect494_roundness;
        
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv,rect494_position, rect494_box, rect494_scale, rect494_roundness, circle959_position, circle959_radius, lerp385_t);
            float4 col = smoothstep(0, 0.01, abs(sdfOut));
            return col;
        }


            ENDHLSL
        }
    }
}
