Shader "Custom/Enemy" {
    Properties {
        _MainTex ("Texture", 2D) = "white" { }
        _TwistUvAmount ("Twist Amount", Range(0, 3.1416)) = 1 //扭曲强度
        _TwistUvRadius ("Twist Radius", Range(0, 3)) = 0.75 //扭曲半径
        _ZoomUvAmount ("Zoom Amount", Range(0.1, 15)) = 0.5 //108
        _Red ("Red", Range(0, 1)) = 0 //108

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
            float4 _MainTex_ST;
            half _TwistUvAmount, _TwistUvRadius;
            half _ZoomUvAmount;
            float _Red;
            half2 center = half2(0.25, 0.5);

            fixed4 mix(fixed4 c1,fixed4 c2,fixed t)
            {
                fixed al = step(c1.a,0.0001);
                fixed3 col = c1*t+(1-t)*c2;
                return fixed4(col,al);
            }
            fixed4 frag(v2f i) : SV_Target {
                i.uv.x = i.uv.x%0.5;
                half2 centerTiled = center;//定义了一个半径为0.5的中心点
                i.uv -= centerTiled;//将纹理坐标平移到以纹理中心为原点的坐标系中
                i.uv = i.uv * _ZoomUvAmount;//对纹理坐标进行缩放操作
                i.uv += centerTiled;//将缩放后的纹理坐标平移到原来的坐标系中心
                clip(i.uv.x);
                clip(1-i.uv.x);
                clip(i.uv.y);
                clip(1-i.uv.y);
                half2 tempUv = i.uv - center;//计算当前像素距离纹理中心的距离
                half percent = (_TwistUvRadius - length(tempUv)) / (_TwistUvRadius + 0.001);//计算每个像素点距离扭曲中心的百分比
                half theta = percent * percent * (2.0 * sin(_TwistUvAmount)) * 8;//根据距离百分比计算角度
                half s = sin(theta);//计算出角度对应的正弦值
                half c = cos(theta);//计算出角度对应的余弦值
                half beta = max(sign(_TwistUvRadius - length(tempUv)), 0);//根据距离扭曲中心的距离，确定哪些像素点需要进行扭曲
                //dot(tempUv, half2(c, -s)), dot(tempUv, half2(s, c)))是旋转矩阵旋转的算法
                //beta + tempUv * (1 - beta) 部分根据beta来控制是否应用扭曲
                tempUv = half2(dot(tempUv, half2(c, -s)), dot(tempUv, half2(s, c))) * beta;//这里使用了向量的点乘来实现二维旋转
                tempUv += center;//将纹理坐标还原到原始范围内
                i.uv = tempUv;//将结果应用到像素的纹理坐标上
                
                fixed4 col = tex2D(_MainTex, i.uv);//采样
                //col = mix(col,(1,0,0,1),_Red);
                return col;
            }
            ENDCG
        }
    }
}