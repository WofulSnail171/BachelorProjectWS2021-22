Shader "SDF/test"
        {

            Properties
            {
                [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
                [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend mode", Float) = 1
                [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destination Blend mode", Float) = 1
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
   float2 rect817_position;
    float2 rect817_box;
    float rect817_scale;
    float4 rect817_roundness;
    
CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            
            float sdfOut = sdf(i.uv,rect817_position, rect817_box, rect817_scale, rect817_roundness);
            float4 col = sdfOut;
            return col;
        }


            ENDHLSL
        }
    }
}
