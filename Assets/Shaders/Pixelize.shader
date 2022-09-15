Shader "Custom/Pixelize"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(0.0001, 0.1)) = 0.0001
        _FadeScale ("Fade Scale", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
 
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert_img
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _PixelSize;
            float _FadeScale;

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col;

                float ratio_x = (int)(i.uv.x/_PixelSize + 0.5) * _PixelSize;
                float ratio_y = (int)(i.uv.y/_PixelSize + 0.5) * _PixelSize;

                col = tex2D(_MainTex, float2(ratio_x, ratio_y));

                col = lerp(col, fixed4(0, 0, 0, 0), _PixelSize * _FadeScale);
                return col;
            }
            
            ENDCG
        }
    }
}
