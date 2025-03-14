Shader "Custom/Wireframe"
{
    Properties
    {
        _WireColor ("Wireframe Color", Color) = (1, 1, 1, 1) // �⺻ ���
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Cull Off
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _WireColor; // ���̾������� ����

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            // Vertex Shader
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // Geometry Shader: ���̾������� ����
            [maxvertexcount(6)]
            void geom(triangle v2f input[3], inout LineStream<v2f> lineStream)
            {
                for (int i = 0; i < 3; i++)
                {
                    v2f a = input[i];
                    v2f b = input[(i + 1) % 3];

                    lineStream.Append(a);
                    lineStream.Append(b);
                }
            }

            // Fragment Shader (���� ����)
            fixed4 frag(v2f i) : SV_Target
            {
                return _WireColor; // ������ ���� ����
            }
            ENDCG
        }
    }
}
