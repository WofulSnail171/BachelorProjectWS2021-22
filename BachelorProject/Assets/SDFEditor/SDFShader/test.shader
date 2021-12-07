Shader "SDF/test"
        {

            Properties
            {
                
                [HideInInspector] circle673_position ("circle673_position", Vector) = (0,0,0,0)
                [HideInInspector] circle673_radius ("circle673_radius", Float) = 0.2
                
                [HideInInspector] colorOutput51_thickness ("colorOutput51_thickness", Float) = 0.2
                [HideInInspector] colorOutput51_repetition ("colorOutput51_repetition", Float) = 1
                [HideInInspector] colorOutput51_lineDistance ("colorOutput51_lineDistance", Float) = 1
                
                [HideInInspector] color665 ("color665", Color) = (1,1,1,1)
                
                [HideInInspector] color566 ("color566", Color) = (1,1,1,1)
                

                [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
                [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
                [Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 0
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
            
            Blend [_SrcBlend] [_DestBlend]
            Cull [_CullMode]
            ZWrite [_ZWrite]
            ZTest [_ZTest]            

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
        float2 circle673_position;
        float circle673_radius;
        float colorOutput51_thickness;
        float colorOutput51_repetition;
        float colorOutput51_lineDistance;
        float4 color665;
        float4 color566;
        
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv, circle673_position, circle673_radius);
            
            float4 col = sdfColor(i.uv, sdfOut, color566, color665, colorOutput51_thickness, colorOutput51_repetition, colorOutput51_lineDistance);
            return col;
        }


            ENDHLSL
        }
    }
}
