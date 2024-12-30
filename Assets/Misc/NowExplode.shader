Shader "Custom/ExplosionMask"
{
    Properties
    {
        _ExplosionTex ("Explosion Texture", 2D) = "white" { }
        _MaskTex ("Mask Texture", 2D) = "white" { }
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _ExplosionTex;
            sampler2D _MaskTex;
            float4 _ExplosionTex_ST;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float luminance(float4 color)
            {
                return dot(color.rgb, float3(0.299, 0.587, 0.114)); // Standard luminance calculation
            }

            half4 frag(v2f i) : SV_Target
            {
                // Sample both the explosion texture and the mask texture (using UV)
                half4 explosionColor = tex2D(_ExplosionTex, i.uv);
                half4 maskColor = tex2D(_MaskTex, i.uv);

                // Calculate brightness of the mask (luminance)
                float brightness = luminance(maskColor);

                // Use brightness as the alpha value (inverse transparency)
                return half4(explosionColor.rgb, 1.0 - brightness);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}