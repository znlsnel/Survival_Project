Shader "Custom/HullOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 1.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        // 첫 번째 패스: 아웃라인 렌더링
        Pass
        {
            Cull Front // 뒷면을 렌더링하여 아웃라인 생성

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _OutlineThickness;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;
                // 노멀 방향으로 버텍스를 확장하여 아웃라인 두께 적용
                float3 normal = normalize(v.normal);
                float3 outlineOffset = normal * _OutlineThickness;
                o.vertex = UnityObjectToClipPos(v.vertex + outlineOffset);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor; // 아웃라인 색상 반환
            }
            ENDCG
        }
    }
}