Shader "SDF/test"
        {

        Properties
        {
            
            [HideInInspector] sCombine917_k ("sCombine917_k", Float) = 0
                
            [HideInInspector] sCombine226_k ("sCombine226_k", Float) = 0
                
            [HideInInspector] circle606_position ("circle606_position", Vector) = (0,0,0,0)
            [HideInInspector] circle606_radius ("circle606_radius", Float) = 0.2
                
            [HideInInspector] circle295_position ("circle295_position", Vector) = (0,0,0,0)
            [HideInInspector] circle295_radius ("circle295_radius", Float) = 0.2
                
            [HideInInspector] triangle152_position ("triangle152_position", Vector) = (0,0,0,0)
            [HideInInspector] triangle152_a ("triangle152_a", Vector) = (0,0,0,0)
            [HideInInspector] triangle152_b ("triangle152_b", Vector) = (0,0,0,0)
            [HideInInspector] triangle152_c ("triangle152_c", Vector) = (0,0,0,0)
            [HideInInspector] triangle152_scale ("triangle152_scale", Float) = 1
            [HideInInspector] triangle152_rotation ("triangle152_rotation", Float) = 0
                
            [HideInInspector] sCombine148_k ("sCombine148_k", Float) = 0
                
            [HideInInspector] sCombine489_k ("sCombine489_k", Float) = 0
                
            [HideInInspector] circle941_position ("circle941_position", Vector) = (0,0,0,0)
            [HideInInspector] circle941_radius ("circle941_radius", Float) = 0.2
                
            [HideInInspector] circle470_position ("circle470_position", Vector) = (0,0,0,0)
            [HideInInspector] circle470_radius ("circle470_radius", Float) = 0.2
                
            [HideInInspector] triangle332_position ("triangle332_position", Vector) = (0,0,0,0)
            [HideInInspector] triangle332_a ("triangle332_a", Vector) = (0,0,0,0)
            [HideInInspector] triangle332_b ("triangle332_b", Vector) = (0,0,0,0)
            [HideInInspector] triangle332_c ("triangle332_c", Vector) = (0,0,0,0)
            [HideInInspector] triangle332_scale ("triangle332_scale", Float) = 1
            [HideInInspector] triangle332_rotation ("triangle332_rotation", Float) = 0
                
            
            [HideInInspector] positionSDF ("positionSDF", Vector) = (0,0,0,0)
            [HideInInspector] scaleSDF ("scaleSDF", Float) = 1
            [HideInInspector] rotationSDF ("rotationSDF", Float) = 0

            [HideInInspector] distance ("distance", Vector) = (0.5,0.5,0,0)
            [HideInInspector] finiteClamp ("finiteClamp", Vector) = (1,1,0,0)

            [HideInInspector] insideTex ("inside Texture", 2D) = "white"{}
            [HideInInspector] insideColor ("inside Color", Color) = (1,1,1,1)
            [HideInInspector] insideTexPosition ("inside Texture Position", Vector) = (0,0,0,0)
            [HideInInspector] insideTexScale ("inside Texture Scale", Float) = 1
            [HideInInspector] insideTexRotation ("inside Texture Rotation", Float) = 0

            [HideInInspector] outsideTex ("outside Texture", 2D) = "white"{}
            [HideInInspector] outsideColor ("outside Color", Color) = (1,1,1,0)
            [HideInInspector] outsideTexPosition ("outside Texture Position", Vector) = (0,0,0,0)
            [HideInInspector] outsideTexScale ("outside Texture Scale", Float) = 1
            [HideInInspector] outsideTexRotation ("outside Texture Rotation", Float) = 0

            [HideInInspector] outlineTex ("outline Texture", 2D) = "white"{}
            [HideInInspector] outlineColor ("outline Color", Color) = (0,0,0,1)
            [HideInInspector] outlineTexPosition ("outline Texture Position", Vector) = (0,0,0,0)
            [HideInInspector] outlineTexScale ("outline Texture Scale", Float) = 1
            [HideInInspector] outlineTexRotation ("outline Texture Rotation", Float) = 0

            [HideInInspector] outlineThickness ("outline Thickness", Float) = 0.2
            [HideInInspector] outlineSmoothness ("outline Smoothness", Float) = 2
            [HideInInspector] outlineInRepetition ("outline In Repetition", Float) = 1
            [HideInInspector] outlineOutRepetition ("outline Out Repetition", Float) = 1
            [HideInInspector] outlineLineDistance ("outline LineDistance", Float) = 1

            [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
            [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
            [Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 0
            [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend mode", Float) = 1
            [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destination Blend mode", Float) = 1
        }

            SubShader
            {
            Tags { "RenderType"="Transparent" 
                   "Queue"="Transparent"
                   "RenderPipeline"="UniversalRenderPipeline"
                 }
            LOD 100

            Pass
            {
            
            Blend [_SrcBlend] [_DstBlend]
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
        float2 positionSDF, distance, finiteClamp;
        float rotationSDF, scaleSDF;

        float sCombine917_k;
        float sCombine226_k;
        float2 circle606_position;
        float circle606_radius;
        float2 circle295_position;
        float circle295_radius;
        float2 triangle152_position;
        float2 triangle152_a;
        float2 triangle152_b;
        float2 triangle152_c;
        float triangle152_scale;
        float triangle152_rotation;
        float sCombine148_k;
        float sCombine489_k;
        float2 circle941_position;
        float circle941_radius;
        float2 circle470_position;
        float circle470_radius;
        float2 triangle332_position;
        float2 triangle332_a;
        float2 triangle332_b;
        float2 triangle332_c;
        float triangle332_scale;
        float triangle332_rotation;
        
        float4 insideColor, outsideColor, outlineColor;
        sampler2D insideTex, outsideTex, outlineTex;
        float2 insideTexPosition, outsideTexPosition, outlineTexPosition;
        float insideTexScale, insideTexRotation, outsideTexScale, outsideTexRotation, outlineTexScale, outlineTexRotation, outlineThickness, outlineSmoothness, outlineInRepetition, outlineOutRepetition, outlineLineDistance;
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {    
            i.uv -= float2(0.5, 0.5);

            float sdfOut = sdf(i.uv, positionSDF, rotationSDF, scaleSDF, distance, finiteClamp,
                               triangle332_position, triangle332_a, triangle332_b, triangle332_c, triangle332_scale, triangle332_rotation, circle470_position, circle470_radius, circle941_position, circle941_radius, sCombine489_k, sCombine148_k, triangle152_position, triangle152_a, triangle152_b, triangle152_c, triangle152_scale, triangle152_rotation, circle295_position, circle295_radius, circle606_position, circle606_radius, sCombine226_k, sCombine917_k);
            
            float4 col = sdfColor(i.uv, sdfOut,
                                  insideColor, insideTex, insideTexPosition, insideTexScale, insideTexRotation, 
                                  outsideColor, outsideTex, outsideTexPosition, outsideTexScale, outsideTexRotation, 
                                  outlineColor, outlineTex, outlineTexPosition, outlineTexScale, outlineTexRotation, 
                                  outlineThickness, outlineSmoothness, outlineInRepetition, outlineOutRepetition, outlineLineDistance);

            return col;
        }


            ENDHLSL
        }
    }
}
