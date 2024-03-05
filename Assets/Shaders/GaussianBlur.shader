Shader "Custom/GaussianBlurTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _BlurSize ("Blur Size", Range (0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
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
            float4 _MainTex_ST;
            float _BlurSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                //o.uv = v.vertex.xy; // Use vertex position as UV for simplicity
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : COLOR
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float blur = _BlurSize;
                int count = 0;
                for (float x = -blur; x <= blur; x += 0.005)
                { 
                    for (float y = -blur; y <= blur; y += 0.005)
                    {
                        col += tex2D(_MainTex, i.uv + float2(x, y) / _ScreenParams.xy);
                        count = count + 1;
                    }
                }
                // Output color with alpha
                col.a = tex2D(_MainTex, i.uv).a; // Include alpha information

                return col / count;
            }
            ENDCG
        }
    }
}
