Shader "Hidden/Image Test1"
{
    Properties
    {
        _Color("Color", Color) = (.25, .5, .5, 1)
    }
    SubShader
    {
        // No culling or depth..
       // ZWrite On
//        Cull Off ZWrite Off ZTest Always
//        Blend SrcAlpha OneMinusSrcAlpha
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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                // just invert the colors
                return _Color;
            }
            ENDCG
        }
    }
}
