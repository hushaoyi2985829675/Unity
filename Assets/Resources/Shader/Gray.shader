Shader "Custom/Gray"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Grayscale ("Grayscale", Float) = 1.0 // 灰度强度（0=原色，1=全灰）
    }

    SubShader
    {
        // 标签：支持 UI 和 2D/3D 物体
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha // 支持透明通道

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
                float4 color : COLOR; // 用于 UI 图片的颜色叠加
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Grayscale;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color; // 传递顶点颜色（UI 图片的 tint 颜色）
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 采样纹理颜色
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // 计算灰度值（使用亮度公式：0.299*R + 0.587*G + 0.114*B）
                fixed gray = dot(col.rgb, fixed3(0.299, 0.587, 0.114));

                // 混合原色和灰度（_Grayscale 控制混合比例）
                col.rgb = lerp(col.rgb, fixed3(gray, gray, gray), _Grayscale);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse" // 降级渲染器
}