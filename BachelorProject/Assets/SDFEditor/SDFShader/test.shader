Shader "SDF/test"
        {

            Properties
            {
                
                [HideInInspector] tex434_position ("tex434_position", Vector) = (0,0,0,0)
                [HideInInspector] tex434_tex ("tex434_tex", 2D) = "white"{}
                
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
        float2 tex434_position;
        sampler2D tex434_tex;
        float tex434_scale;
        float tex434_rotation;
        
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv, tex434_position, tex434_tex, tex434_scale, tex434_rotation);
            
            float4 col = smoothstep(0, 0.01, sdfOut);
            return col;
        }


            ENDHLSL
        }
    }
}
