Shader "Custom/BrightnessSaturationAndContrast"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Brightness("Brightness",Float) = 1
        _Saturation("Saturation",Float) = 1
        _Contrast("Contrast",Float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            CGPROGRAM

            sampler2D _MainTex;
            half _Brightness;
            half _Saturation;
            half _Contrast;

            #pragma vertex vert
            #pragma fragment frag
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // Apply brightness
                fixed3 finalColor = col.rgb * _Brightness;
                // Apply saturation 
                fixed luminance= 0.2125 * col.r + 0.7154 * col.g + 0.0721 * col.b; 
                fixed3 luminanceColor = fixed3(luminance, luminance, luminance) ;
                //Apply contrast 
                fixed3 avgColor = fixed3(0.5 , 0.5, 0.5); 
                finalColor = lerp(avgColor, finalColor, _Contrast);
                fixed4 c = fixed4(finalColor,col.a);
                return c;
            }
            ENDCG
        }
        
    }
    Fallback Off
}
