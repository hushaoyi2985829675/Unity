Shader "Custom/Water"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,0.5) // 包含透明度的颜色
        _Transparency ("Transparency", Range(0,1)) = 0.5 // 额外透明度控制
        _Brightness ("Brightness", Range(0, 2)) = 1.0 // 亮度调整
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" // 透明队列，确保正确渲染顺序
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane" // 预览为平面
            "CanUseSpriteAtlas"="True" // 支持精灵图集
        }

        Cull Off // 关闭背面剔除（2D物体通常不需要）
        Lighting Off // 关闭光照
        ZWrite Off // 关闭深度写入
        Blend SrcAlpha OneMinusSrcAlpha // 标准透明混合模式

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Transparency;
            float _Brightness;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // 采样纹理并应用颜色 tint
                fixed4 col = tex2D(_MainTex, IN.uv) * _Color;

                // 应用亮度调整
                col.rgb *= _Brightness;

                // 应用透明度控制（结合颜色的alpha通道）
                col.a *= _Transparency;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}