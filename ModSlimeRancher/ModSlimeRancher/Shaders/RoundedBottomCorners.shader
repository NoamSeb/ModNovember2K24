Shader "Custom/RoundedBottomCorners"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BottomRadius ("Bottom Corner Radius", Float) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {}}}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _BottomRadius;
            float2 _MainTex_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);  // Replaced UnityObjectToClipPos with mul
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : COLOR
            {
                float2 uv = i.uv;
                float2 size = float2(1.0, 1.0) / _MainTex_TexelSize; // Get dimensions
            }
ENDCG