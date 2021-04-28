Shader "Hidden/NewImageEffectShader 1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("_GlowColor",Color)=(1,1,1,1)
        _Outline("_Outline",Range(0,10)) = 0.1

        _GlowColor ("_GlowColor",Color)=(1,1,1,1)
        _GlowStrength("_GlowStrength",Range(0,10)) = 0.1
        _GlowSize("_GlowSize",Range(0,10)) = 0.1
        
    }
    SubShader
    {
        // No culling or depth
        Tags{  "Queue" ="Transparent"}
//开始alpha 混合
        Blend SrcAlpha OneMinusSrcAlpha
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

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float3 _OutlineColor;
            float _Outline;

            float4 _GlowColor;
            float _GlowStrength;
            float _GlowSize;


            fixed4 frag (v2f i) : SV_Target
{
    fixed4 col = tex2D(_MainTex, i.uv);
     
    float2 uv_up = i.uv + _MainTex_TexelSize.xy * float2(0, 1) * _Outline;
    float2 uv_down = i.uv + _MainTex_TexelSize.xy * float2(0, -1) * _Outline;
    float2 uv_left = i.uv + _MainTex_TexelSize.xy * float2(-1, 0) * _Outline;
    float2 uv_right = i.uv + _MainTex_TexelSize.xy * float2(1, 0) * _Outline;

    float w = tex2D(_MainTex, uv_up).a * tex2D(_MainTex, uv_down).a
        * tex2D(_MainTex, uv_left).a * tex2D(_MainTex, uv_right).a;

    col.rgb = lerp(_OutlineColor, col.rgb,w);


float s = 0;
				_GlowStrength /= 50.0;

                for (int x=-5; x<=5; x++)
                    for (int y=-5; y<=5; y++){
						fixed4 color = tex2D(_MainTex, i.uv + fixed2(x * _GlowSize * _MainTex_TexelSize.x, y * _GlowSize * _MainTex_TexelSize.y));
                        if (color.r == 0 && color.g == 0 && color.b == 0)
                            continue;
                        else
                            s += color.a * _GlowStrength;
						}
				            
                s = min(s, 1);
                


    return fixed4(col.rgb, s * _GlowColor.a);
}
        ENDCG
        }
    }
}
