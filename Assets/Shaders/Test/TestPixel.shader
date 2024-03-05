Shader"Custom/TestPixel"{
    Properties{
        _MainTex("MainTex",2D)="white"{}
    }
    SubShader{
        //Tags { "LIGHTMODE"="Universal2D" "QUEUE"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "ShaderGraphShader"="true" "ShaderGraphTargetId"="UniversalSpriteUnlitSubTarget" "UniversalMaterialType"="Unlit" }
        Pass{
             // ZWrite Off
            //Cull Off
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert  
            #pragma fragment frag
            sampler2D _MainTex;
            uniform half4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            struct v2f {
                float4 pos : SV_POSITION;
                half2 uv[9] : TEXCOORD0;
            };
            v2f vert(appdata_img v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                half2 uv = v.texcoord;
                o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
                o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, -1);
                o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, -1);
                o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
                o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);		//原点
                o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
                o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
                o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, 1);
                o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, 1);
                return o;
            }
            fixed4 frag(v2f i) : SV_Target {
                fixed4 col;
                col = tex2D(_MainTex, i.uv[0]);
                col += tex2D(_MainTex, i.uv[1]);
                col += tex2D(_MainTex, i.uv[2]);
                col += tex2D(_MainTex, i.uv[3]);
                col += tex2D(_MainTex, i.uv[4]);
                col += tex2D(_MainTex, i.uv[5]);
                col += tex2D(_MainTex, i.uv[6]);
                col += tex2D(_MainTex, i.uv[7]);
                col += tex2D(_MainTex, i.uv[8]);
                col /=9;
                return col;
            }
            ENDCG 
        }
    }
    FallBack Off
}