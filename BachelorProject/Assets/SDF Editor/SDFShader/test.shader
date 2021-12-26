Shader "SDF/test"
        {

        Properties
        {
            
            [HideInInspector] line490_position ("line490_position", Vector) = (0,0,0,0)
            [HideInInspector] line490_a ("line490_a", Vector) = (0,0,0,0)
            [HideInInspector] line490_b ("line490_b", Vector) = (0,0,0,0)
            [HideInInspector] line490_scale ("line490_scale", Float) = 1
            [HideInInspector] line490_roundness ("line490_roundness", Float) = 1
            [HideInInspector] line490_rotation ("line490_rotation", Float) = 0
                
            [HideInInspector] sSubtract665_k ("sSubtract665_k", Float) = 0
                
            [HideInInspector] circle442_position ("circle442_position", Vector) = (0,0,0,0)
            [HideInInspector] circle442_radius ("circle442_radius", Float) = 0.2
                
            [HideInInspector] rect365_position ("rect365_position", Vector) = (0,0,0,0)
            [HideInInspector] rect365_box ("rect365_box", Vector) = (0,0,0,0)
            [HideInInspector] rect365_scale ("rect365_scale", Float) = 1
            [HideInInspector] rect365_roundness ("rect365_roundness", Vector) = (0,0,0,0)
            [HideInInspector] rect365_rotation ("rect365_rotation", Float) = 0
                
            
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

        float2 line490_position;
        float2 line490_a;
        float2 line490_b;
        float line490_roundness;
        float line490_scale;
        float line490_rotation;
        float sSubtract665_k;
        float2 circle442_position;
        float circle442_radius;
        float2 rect365_position;
        float2 rect365_box;
        float rect365_scale;
        float4 rect365_roundness;
        float rect365_rotation;
        
        float4 insideColor, outsideColor, outlineColor;
        sampler2D insideTex, outsideTex, outlineTex;
        float2 insideTexPosition, outsideTexPosition, outlineTexPosition;
        float insideTexScale, insideTexRotation, outsideTexScale, outsideTexRotation, outlineTexScale, outlineTexRotation, outlineThickness, outlineSmoothness, outlineInRepetition, outlineOutRepetition, outlineLineDistance;
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {    
            i.uv -= float2(0.5, 0.5);

            float sdfOut = sdf(i.uv, positionSDF, rotationSDF, scaleSDF, distance, finiteClamp,
                               rect365_position, rect365_box, rect365_scale, rect365_roundness, rect365_rotation, circle442_position, circle442_radius, sSubtract665_k, line490_position, line490_a, line490_b, line490_roundness, line490_scale, line490_rotation);
            
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
