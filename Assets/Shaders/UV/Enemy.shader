Shader "Custom/Enemy" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Noise ("Texture", 2D) = "white" {}
        _Red ("Red", Range(0, 1)) = 0
        _Count("Count",Integer) = 1
        _Reslove ("Reslove", Range(0, 1.1)) = 0 
        _NoiseCount ("NoiseCount", Range(1, 100)) = 0 
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
            sampler2D _Noise;
            float4 _MainTex_ST;
            half _NoiseCount;
            float _Red;
            float _Reslove;

            fixed4 mix(fixed4 c1,fixed4 c2,fixed t)
            {
                fixed a = step(0.01, c1.a);
                fixed3 c = c1* (1 - t)+t*c2;
                return fixed4(c,a);
            }
            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);//采样
                fixed4 noise = tex2D(_Noise, i.uv/_NoiseCount);//采样
                clip(noise.r-_Reslove);
                col = mix(col,fixed4(1,0,0,1),_Red); 

                return col;
            }
            ENDCG
        }
    }
}