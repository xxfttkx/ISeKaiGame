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
            // 使用顶点着色器输出屏幕坐标和uv坐标
            // 这里简单地将顶点坐标映射到屏幕坐标，将uv坐标传递给片元着色器
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // 传递uv坐标
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                // 使用uv坐标进行纹理采样
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}


