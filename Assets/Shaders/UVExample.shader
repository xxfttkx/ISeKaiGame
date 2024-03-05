Shader "Custom/UVExample"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;
            // ʹ�ö�����ɫ�������Ļ�����uv����
            // ����򵥵ؽ���������ӳ�䵽��Ļ���꣬��uv���괫�ݸ�ƬԪ��ɫ��
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // ����uv����
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                // ʹ��uv��������������
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}


