Shader "Custom/CoolShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _MaskTex ("Mask (A channel)", 2D) = "black" { }
        _RedFilter ("Red Filter", Range(0, 1)) = 1
        _GreenFilter ("Green Filter", Range(0, 1)) = 1
        _BlueFilter ("Blue Filter", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float _RedFilter;
            float _GreenFilter;
            float _BlueFilter;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                // Sample the base texture and the mask texture
                half4 color = tex2D(_MainTex, i.uv);
                half maskValue = tex2D(_MaskTex, i.uv).r;  // Assuming the mask is grayscale in the alpha channel (r)

                // If the mask value is 1 (white), apply the channel filter, otherwise keep original color
                if (maskValue > 0.5)  // You can tweak this threshold
                {
                    color.r *= _RedFilter;
                    color.g *= _GreenFilter;
                    color.b *= _BlueFilter;
                }

                return color;
            }
            ENDCG
        }
    }
}
