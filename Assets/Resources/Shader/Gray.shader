Shader "Custom/GrayWithMaskFixed"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Grayscale ("Grayscale", Range(0,1)) = 1.0 // 灰度强度（0=原色，1=全灰）

        // Mask/Stencil 必备属性（隐藏避免手动修改，由Unity自动赋值）
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255

        // 颜色遮罩与裁剪区域（Rect Mask 2D 必备）
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _ClipRect ("Clip Rect", Vector) = (-32768, -32768, 32768, 32768)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" // 透明队列（UI标准）
            "RenderType"="Transparent" // 透明渲染类型
            "IgnoreProjector"="True" // 不受投影器影响
            "PreviewType"="Plane" // 平面预览（适配2D UI）
            "CanUseSpriteAtlas"="True" // 支持Sprite图集（避免UI图片错位）
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha // 标准透明混合（支持半透明）
        ZWrite Off // 关闭深度写入（UI层级不冲突）

        // Stencil模板测试（适配Mask组件）
        Stencil
        {
            Ref [_Stencil] // 参考值（由Mask动态设置）
            Comp [_StencilComp] // 比较方式（如相等、大于）
            Pass [_StencilOp] // 比较通过时的操作
            ReadMask [_StencilReadMask] // 读取掩码
            WriteMask [_StencilWriteMask]// 写入掩码
        }
        ColorMask [_ColorMask] // 控制输出颜色通道（默认全通道）

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ UNITY_UI_ALPHACLIP // 支持UI alpha裁剪

            // 引入Unity内置工具库
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            // 输入结构体（顶点数据）
            struct appdata
            {
                float4 vertex : POSITION; // 顶点坐标
                float2 uv : TEXCOORD0; // 纹理坐标
                float4 color : COLOR; // UI顶点颜色（Tint）
                UNITY_VERTEX_INPUT_INSTANCE_ID // 实例ID（多实例兼容）
            };

            // 输出结构体（片元数据）
            struct v2f
            {
                float2 uv : TEXCOORD0; // 纹理坐标
                float4 vertex : SV_POSITION; // 裁剪空间坐标（屏幕显示用）
                float4 color : COLOR; // 传递顶点颜色
                float4 worldPosition : TEXCOORD1; // 世界坐标（用于裁剪计算）
                UNITY_VERTEX_OUTPUT_STEREO // 立体渲染兼容
            };

            // 外部参数（从Properties或Unity自动赋值）
            sampler2D _MainTex; // 主纹理（UI图片）
            float4 _MainTex_ST; // 纹理缩放偏移
            float _Grayscale; // 灰度控制参数
            float4 _ClipRect; // 裁剪区域（Rect Mask 2D用）

            // 顶点着色器（坐标转换+数据传递）
            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); // 初始化实例ID
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); // 初始化立体渲染

                o.worldPosition = v.vertex; // 记录世界坐标
                o.vertex = UnityObjectToClipPos(v.vertex); // 顶点坐标转裁剪空间
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // 纹理坐标适配缩放偏移
                o.color = v.color; // 传递顶点颜色
                return o;
            }

            // 片元着色器（颜色计算+灰度效果）
            fixed4 frag(v2f i) : SV_Target
            {
                // 1. Rect Mask 2D裁剪（超出区域的像素丢弃）
                float2 clipResult = UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                clip(clipResult.xy - 0.001); // 0.001避免浮点精度问题

                // 2. 采样纹理颜色 + 叠加顶点颜色（支持UI Tint）
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // 3. 计算灰度值（符合人眼敏感度的亮度公式）
                fixed gray = dot(col.rgb, fixed3(0.299, 0.587, 0.114));

                // 4. 混合原色与灰度（_Grayscale控制混合比例）
                col.rgb = lerp(col.rgb, fixed3(gray, gray, gray), _Grayscale);

                // 5. UI Alpha裁剪（防止半透明边缘残留）
                #ifdef UNITY_UI_ALPHACLIP
                    clip(col.a - 0.001);
                #endif

                return col;
            }
            ENDCG
        }
    }
    FallBack "UI/Default" // 降级方案：不支持时使用Unity默认UI Shader
}

