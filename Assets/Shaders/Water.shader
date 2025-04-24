 Shader "Water"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _Speed ("Wave Speed", Float) = 0.1
        _Opacity ("Opacity", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Permet la transparence
            ZWrite Off // Empêche les conflits de profondeur
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _Speed;
            float _Opacity;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv + float2(_Time.y * _Speed, _Time.y * _Speed * 0.5); // Défilement de la texture
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= _Opacity; // Applique la transparence
                return col;
            }
            ENDCG
        }
    }
}
