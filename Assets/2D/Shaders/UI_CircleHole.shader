Shader "UI/CircleOrEllipseHole"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _HoleCenter ("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleSize ("Hole Size (X,Y)", Vector) = (0.08, 0.08, 0, 0)
        _Feather ("Feather", Float) = 0.002
        _RectAspect ("Rect Aspect", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            float2 _HoleCenter;
            float2 _HoleSize;
            float _Feather;
            float _RectAspect;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // Przesunięcie względem środka dziury
                float2 delta = i.uv - _HoleCenter;

                // Korekta aspect ratio RectTransform, żeby koło nie robiło się elipsą
                delta.x *= _RectAspect;

                // Normalizacja przez rozmiar dziury
                float2 normalized = delta / max(_HoleSize, float2(0.0001, 0.0001));

                // Dla koła: HoleSize.x == HoleSize.y
                // Dla elipsy: HoleSize.x != HoleSize.y
                float dist = length(normalized);

                // Wewnątrz dziury alpha = 0, poza nią alpha = 1
                float keep = smoothstep(1.0 - _Feather, 1.0, dist);

                col.a *= keep;
                return col;
            }
            ENDCG
        }
    }
}
