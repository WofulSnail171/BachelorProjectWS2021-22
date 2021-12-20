Shader "SDF/test"
        {

        Properties
        {
            
            [HideInInspector] rect828_position ("rect828_position", Vector) = (0,0,0,0)
            [HideInInspector] rect828_box ("rect828_box", Vector) = (0,0,0,0)
            [HideInInspector] rect828_scale ("rect828_scale", Float) = 1
            [HideInInspector] rect828_roundness ("rect828_roundness", Vector) = (0,0,0,0)
            [HideInInspector] rect828_rotation ("rect828_rotation", Float) = 0
                
            
            [HideInInspector] positionSDF ("positionSDF", Vector) = (0,0,0,0)
            [HideInInspector] scaleSDF ("scaleSDF", Float) = 1
            [HideInInspector] rotationSDF ("rotationSDF", Float) = 0

            [HideInInspector] infiniteDistance ("infiniteDistance", Vector) = (0,0,0,0)
            [HideInInspector] finiteDistance ("finiteDistance", Vector) = (0,0,0,0)
            [HideInInspector] finiteClamp ("finiteClamp", Vector) = (0,0,0,0)

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
            [HideInInspector] outlineRepetition ("outline Repetition", Float) = 1
            [HideInInspector] outlineLineDistance ("outline LineDistance", Float) = 1

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
        float2 positionSDF, infiniteDistance, finiteDistance, finiteClamp;
        float rotationSDF, scaleSDF;
        float2 rect828_position;
        float2 rect828_box;
        float rect828_scale;
        float4 rect828_roundness;
        float rect828_rotation;
        
        float4 insideColor, outsideColor, outlineColor;
        sampler2D insideTex, outsideTex, outlineTex;
        float2 insideTexPosition, outsideTexPosition, outlineTexPosition;
        float insideTexScale, insideTexRotation, outsideTexScale, outsideTexRotation, outlineTexScale, outlineTexRotation, outlineThickness, outlineRepetition, outlineLineDistance;
     CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {    
            i.uv -= float2(0.5, 0.5);

            float sdfOut = sdf(i.uv, positionSDF, rotationSDF, scaleSDF, infiniteDistance, finiteDistance, finiteClamp,
                               rect828_position, rect828_box, rect828_scale, rect828_roundness, rect828_rotation);
            
            float4 col = sdfColor(i.uv, sdfOut,
                                  insideColor, insideTex, insideTexPosition, insideTexScale, insideTexRotation, 
                                  outsideColor, outsideTex, outsideTexPosition, outsideTexScale, outsideTexRotation, 
                                  outlineColor, outlineTex, outlineTexPosition, outlineTexScale, outlineTexRotation, 
                                  outlineThickness, outlineRepetition, outlineLineDistance);

            return col;
        }


            ENDHLSL
        }
    }
}
