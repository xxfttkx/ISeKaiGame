Shader "Custom/Chicken" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Type("Type",Integer) = 0
    }
        SubShader{
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
                half _Type;

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);//²ÉÑù
                    if(_Type==1)
                        col.gb = fixed2(0, 0);
                    else if(_Type == 2)
                        col.rb = fixed2(0, 0);
                    else if (_Type == 3)
                        col.rg = fixed2(0, 0);
                    return col;
                }
                ENDCG
            }
        }
}