Shader "Unlit/DrawShader"
{
    Properties
    {
        _DrawPosition ("Draw Position", Vector) = (-1,-1,0,0)
        _BrushSize ("Brush Size", Range(0.01, 0.2)) = 0.05
        _BrushColor ("Brush Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Lighting Off
        Blend One OneMinusSrcAlpha

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

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float4 previousColor = tex2D(_SelfTexture2D, IN.localTexcoord.xy);

                float dist = distance(IN.localTexcoord.xy, _DrawPosition.xy);
                
                float innerEdge = _BrushSize * 0.98;
                float brushAlpha = smoothstep(_BrushSize, innerEdge, dist);
                
                float4 drawColor = _BrushColor;
                drawColor.a = brushAlpha;

                return lerp(previousColor, drawColor, brushAlpha);
            }
            ENDCG
        }
    }
}