Shader "Custom/2DRimLighting" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" { }
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimPower("Rim Power", Range(0.1, 10)) = 3
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f {
                    float4 pos : POSITION;
                    float4 color : COLOR;
                };

                uniform float _RimPower;
                uniform float4 _RimColor;

                v2f vert(appdata v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.color = _RimColor;
                    return o;
                }

                fixed4 frag(v2f i) : COLOR {
                    float rim = 1.0 - dot(normalize(i.color.rgb), normalize(_WorldSpaceCameraPos - i.pos));
                    rim = saturate(pow(rim, _RimPower));
                    return rim * i.color;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
