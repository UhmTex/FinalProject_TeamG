Shader "Unlit/TransparentDrawShader"
{
    Properties
    {
        _DrawPosition ("Draw Position", Vector) = (-1,-1,0,0)
        _BrushSize ("Brush Size", Range(0.01, 0.2)) = 0.05
        _BrushColor ("Brush Color", Color) = (1,1,1,1)
        _Opacity ("Overall Opacity", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags 
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }
        
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
            
            float4 _DrawPosition;
            float _BrushSize;
            float4 _BrushColor;
            float _Opacity;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                // Get the previous color (initially will be transparent)
                float4 previousColor = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
      
                float dist = distance(IN.localTexcoord.xy, _DrawPosition.xy);
                float brushAlpha = smoothstep(_BrushSize, 0, dist);

                // Create the draw color with proper alpha
                float4 drawColor = _BrushColor;
                drawColor.a = brushAlpha * _Opacity;

                // Blend the new color with the previous one
                return lerp(previousColor, drawColor, brushAlpha);
            }
            ENDCG
        }
    }
}