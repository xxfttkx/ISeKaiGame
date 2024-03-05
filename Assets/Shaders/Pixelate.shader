Shader "Custom/Pixelate" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _PixelSize ("Pixel Size", Range (50, 200)) = 50
    }

    //SubShader {
    //    Tags { "RenderType"="Opaque" }
    //    LOD 100

    //    CGPROGRAM
    //    #pragma surface surf Lambert

    //    struct Input {
    //        float2 uv_MainTex;
    //    };

    //    sampler2D _MainTex;
    //    fixed _PixelSize;

    //    void surf (Input IN, inout SurfaceOutput o) {
    //        // Calculate pixelated UV
    //        float2 pixelUV = IN.uv_MainTex * _PixelSize;
    //        pixelUV = floor(pixelUV) / _PixelSize;

    //        // Sample the texture
    //        fixed4 c = tex2D(_MainTex, pixelUV);

    //        // Output color with alpha
    //        o.Albedo = c.rgb;
    //        o.Alpha = c.a; // Include alpha information
    //    }
    //    ENDCG
    //}

    SubShader {
        Tags { "RenderType"="Transparent" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed _PixelSize;

        void surf (Input IN, inout SurfaceOutput o) {
            // Calculate pixelated UV
            float2 pixelUV = IN.uv_MainTex * _PixelSize;
            pixelUV = floor(pixelUV) / _PixelSize;

            // Sample the texture
            fixed4 c = tex2D(_MainTex, pixelUV);

            // Output color with alpha
            o.Albedo = c.rgb;
            o.Alpha = c.a; // Include alpha information
        }
        ENDCG
    }

    FallBack "Diffuse"
}
