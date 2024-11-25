Shader "Custom/CanvasSilhouette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amount ("Amount", Range(0, 1)) = 0
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Amount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Handle special cases first
                if (_Amount == 0.0)
                {
                    return fixed4(0, 0, 0, 0);
                }
                else if (_Amount == 1.0)
                {
                    return fixed4(0, 0, 0, 1);
                }

                // Calculate scaled amount using log space transformation
                float amount_scaled = exp(log(1.0/16.0) + (log(128.0) - log(1.0/16.0)) * _Amount);
                
                // Calculate UV offset from center
                float2 centeredUV = i.uv - float2(0.5, 0.5);
                float2 scaledUV = centeredUV * amount_scaled + float2(0.5, 0.5);
                
                // Sample texture and use red channel
                float value = tex2D(_MainTex, scaledUV).r;
                
                // Create silhouette by inverting the value for alpha
                return fixed4(0, 0, 0, 1.0 - value);
            }
            ENDCG
        }
    }
}
