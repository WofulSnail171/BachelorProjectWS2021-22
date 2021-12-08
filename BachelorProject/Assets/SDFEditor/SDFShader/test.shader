Shader "SDF/test"
        {

            Properties
            {
                
                [HideInInspector] circle646_position ("circle646_position", Vector) = (0,0,0,0)
                [HideInInspector] circle646_radius ("circle646_radius", Float) = 0.2
                
                [HideInInspector] triangle490_position ("triangle490_position", Vector) = (0,0,0,0)
                [HideInInspector] triangle490_a ("triangle490_a", Vector) = (0,0,0,0)
                [HideInInspector] triangle490_b ("triangle490_b", Vector) = (0,0,0,0)
                [HideInInspector] triangle490_c ("triangle490_c", Vector) = (0,0,0,0)
                [HideInInspector] triangle490_scale ("triangle490_scale", Float) = 1
                [HideInInspector] triangle490_rotation ("triangle490_rotation", Float) = 0
                
                [HideInInspector] colorOutput839_thickness ("colorOutput839_thickness", Float) = 0.2
                [HideInInspector] colorOutput839_repetition ("colorOutput839_repetition", Float) = 1
                [HideInInspector] colorOutput839_lineDistance ("colorOutput839_lineDistance", Float) = 1
                
                [HideInInspector] tex705_position ("tex705_position", Vector) = (0,0,0,0)
                [HideInInspector] tex705_scale ("tex705_scale", Float) = 1
                [HideInInspector] tex705_rotation ("tex705_rotation", Float) = 0
                [HideInInspector] tex705_tex ("tex705_tex", 2D) = "white"{}
                [HideInInspector] tex705_color ("tex705_color", Color) = (1,1,1,1)
                
                [HideInInspector] color646 ("color646", Color) = (1,1,1,1)
                
                [HideInInspector] color855 ("color855", Color) = (1,1,1,1)
                

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
        float2 circle646_position;
        float circle646_radius;
        float2 triangle490_position;
        float2 triangle490_a;
        float2 triangle490_b;
        float2 triangle490_c;
        float triangle490_scale;
        float triangle490_rotation;
        float colorOutput839_thickness;
        float colorOutput839_repetition;
        float colorOutput839_lineDistance;
        float2 tex705_position;
        sampler2D tex705_tex;
        float tex705_scale;
        float tex705_rotation;
        float4 tex705_color;
        float4 color646;
        float4 color855;
        
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            i.uv -= float2(0.5, 0.5);
            float sdfOut = sdf(i.uv, triangle490_position, triangle490_a, triangle490_b, triangle490_c, triangle490_scale, triangle490_rotation, circle646_position, circle646_radius);
            
            float4 col = sdfColor(i.uv, sdfOut, color855, color646, tex705_position, tex705_tex, tex705_scale, tex705_rotation, tex705_color, colorOutput839_thickness, colorOutput839_repetition, colorOutput839_lineDistance);
            return col;
        }


            ENDHLSL
        }
    }
}
