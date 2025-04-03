Shader "Custom/HighlightShader"
{
    Properties
    {
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowStrength ("Glow Strength", Float) = 0.2
        _MinY ("Bottom Y", Float) = 0.0
        _MaxY ("Top Y", Float) = 1.0
        _PulseFrequency ("Pulse Frequency", Float) = 5.0
        _PulseSpeed ("Pulse Speed", Float) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                half verticalFace : TEXCOORD0;
                half2 params : TEXCOORD1;
            };

            fixed4 _GlowColor;
            half _GlowStrength;
            half _MinY;
            half _MaxY;
            half _PulseFrequency;
            half _PulseSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                // World-space normal calculation
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.verticalFace = abs(worldNormal.y);
                
                // Position calculations
                float worldY = mul(unity_ObjectToWorld, v.vertex).y;
                o.params.x = saturate((worldY - _MinY) / (_MaxY - _MinY));
                o.params.y = worldY * _PulseFrequency + _Time.y * _PulseSpeed;
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Strict top/bottom face culling
                clip(0.99 - i.verticalFace);
                
                // Pulse calculation
                half pulse = sin(i.params.y) * 0.5 + 0.5;
                
                // Final color
                return fixed4(
                    _GlowColor.rgb * (_GlowStrength * pulse) + 1.0,
                    (1.0 - i.params.x) * pulse
                );
            }
            ENDCG
        }
    }
}