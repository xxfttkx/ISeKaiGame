Shader "Custom/Noise" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("Effect", 2D) = "white" {}
        _EffectScale("EffectScale", Range(1, 100)) = 0
        _EffectPercent("EffectPercent",Range(0,1))=0
    }
    SubShader {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Effect;
            half _EffectScale;
            half _EffectPercent;

            fixed4 mix(fixed4 c1,fixed4 c2,fixed t)
            {
                fixed a = step(0.01, c1.a);
                fixed3 c = c1* (1 - t)+t*c2;
                return fixed4(c,a);
            }
            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed offset = sin(_Time.y);
                fixed4 effect = tex2D(_Effect, i.uv/ _EffectScale+offset/10);
                col = mix(col, effect, _EffectPercent);

                return col;
            }
            ENDCG
        }
    }
}