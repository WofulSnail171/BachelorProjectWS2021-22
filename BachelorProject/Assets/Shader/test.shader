Shader "SDF/test"
        {

            Properties
            {
               
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
   float2 rect636_position;
    float2 rect636_box;
    float rect636_scale;
    float4 rect636_roundness;
    
CBUFFER_END

        float4 frag (v2f i) : SV_Target
        {
            
            float sdfOut = sdf(i.uv,rect636_position, rect636_box, rect636_scale, rect636_roundness);
            float4 col = float4(1,1,1,1);
            return col;
        }


            ENDHLSL
        }
    }
}
