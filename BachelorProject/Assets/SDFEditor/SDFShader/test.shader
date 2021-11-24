Shader "SDF/test"
        {

            Properties
            {
                
                [HideInInspector] lerp929_t ("lerp929_t", Float) = 0
                
                [HideInInspector] circle283_position ("circle283_position", Vector) = (0,0,0,0)
                [HideInInspector] circle283_radius ("circle283_radius", Float) = 0
                
                [HideInInspector] rect2_position ("rect2_position", Vector) = (0,0,0,0)
                [HideInInspector] rect2_box ("rect2_box", Vector) = (0,0,0,0)
                [HideInInspector] rect2_scale ("rect2_scale", Float) = 0
                [HideInInspector] rect2_roundness ("rect2_roundness", Vector) = (0,0,0,0)
                
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
        float lerp929_t;
        float2 circle283_position;
        float circle283_radius;
        float2 rect2_position;
        float2 rect2_box;
        float rect2_scale;
        float4 rect2_roundness;
        
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv,rect2_position, rect2_box, rect2_scale, rect2_roundness, circle283_position, circle283_radius, lerp929_t);
            float4 col = smoothstep(0, 0.01, abs(sdfOut));
            return col;
        }


            ENDHLSL
        }
    }
}
