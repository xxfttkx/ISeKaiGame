Shader "Custom/Test0323" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Effect ("Effect", 2D) = "white" {}
        _EffectScale("EffectScale", Range(1, 100)) = 0
        _EffectPercent("EffectPercent",Range(0,1))=0
        _Outline("Outline",Range(0,10)) = 0
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
            fixed4 _MainTex_TexelSize;
            half _Outline;
           
            half _EffectScale;
            half _EffectPercent;
            

            fixed4 mix(fixed4 c1,fixed4 c2,fixed t)
            {
                fixed a = step(0.01, c1.a);
                a = 1;
                fixed3 c = c1*t+(1-t)*c2;
                return fixed4(c,a);
            }
            fixed4 frag(v2f i) : SV_Target {
                fixed sizeX = _MainTex_TexelSize.x * _Outline;
                fixed sizeY = _MainTex_TexelSize.y * _Outline;
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed offset = sin(_Time.y);
                fixed4 effect = tex2D(_Effect, i.uv/ _EffectScale+offset/10);
                //col = mix(col, effect, 1-_EffectPercent);
                fixed a1 = tex2D(_MainTex, fixed2(i.uv.x-sizeX,i.uv.y)).a;
                fixed a2 = tex2D(_MainTex, fixed2(i.uv.x+sizeX,i.uv.y)).a;
                fixed a3 = tex2D(_MainTex, fixed2(i.uv.x,i.uv.y-sizeY)).a;
                fixed a4 = tex2D(_MainTex, fixed2(i.uv.x,i.uv.y+sizeY)).a;
                fixed a5 = tex2D(_MainTex, fixed2(i.uv.x-sizeX,i.uv.y+sizeY)).a;
                fixed a6 = tex2D(_MainTex, fixed2(i.uv.x-sizeX,i.uv.y-sizeY)).a;
                fixed a7 = tex2D(_MainTex, fixed2(i.uv.x+sizeX,i.uv.y+sizeY)).a;
                fixed a8 = tex2D(_MainTex, fixed2(i.uv.x+sizeX,i.uv.y-sizeY)).a;
                fixed a9 = tex2D(_MainTex, fixed2(i.uv.x,i.uv.y)).a;
                fixed a = a1+a2+a3+a4+a5+a6+a7+a8+a9;
                clip(a-0.9);
                // normal 1   outline 0
                fixed co = step(0.1, col.a);
                col = mix(col, fixed4(0, 0, 0, 1), co);
                return col;
            }
            ENDCG
        }
    }
}