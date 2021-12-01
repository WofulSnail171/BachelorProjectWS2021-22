Shader "SDF/test"
        {

            Properties
            {
                
                [HideInInspector] line59_position ("line59_position", Vector) = (0,0,0,0)
                [HideInInspector] line59_a ("line59_a", Vector) = (0,0,0,0)
                [HideInInspector] line59_b ("line59_b", Vector) = (0,0,0,0)
                [HideInInspector] line59_scale ("line59_scale", Float) = 1
                [HideInInspector] line59_roundness ("line59_roundness", Float) = 1
                [HideInInspector] line59_rotation ("line59_rotation", Float) = 0
                
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
        float2 line59_position;
        float2 line59_a;
        float2 line59_b;
        float line59_roundness;
        float line59_scale;
        float line59_rotation;
        
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv, line59_position, line59_a, line59_b, line59_roundness, line59_scale, line59_rotation);
            
            float4 col = smoothstep(0, 0.01, sdfOut);
            return col;
        }


            ENDHLSL
        }
    }
}
