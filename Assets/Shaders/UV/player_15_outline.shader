Shader "Custom/player_15_outline" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
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
            fixed4 _MainTex_TexelSize;
            half _Outline;

            fixed4 frag(v2f i) : SV_Target {
                fixed sizeX = _MainTex_TexelSize.x * _Outline;
                fixed sizeY = _MainTex_TexelSize.y * _Outline;
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
                //fixed r = sin(_Time.y)/2+0.5;
                //fixed g = cos(_Time.y)/2+0.5;
                fixed4 col = fixed4(1, 1, 1, 1);
                return col;
            }
            ENDCG
        }
    }
}