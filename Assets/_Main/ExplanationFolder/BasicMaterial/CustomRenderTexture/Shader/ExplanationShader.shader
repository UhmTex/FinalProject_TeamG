Shader "Grass Height map shader"
{
    Properties
    {
        _DrawPosition("Draw Position", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Lighting Off
        Blend One Zero
        
        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
            
            float4 _DrawPosition;
            
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                //float4 previousColor = tex2D(_SelfTexture2D, IN.localTexcoord.xy);
                float4 drawColor = smoothstep(0,.2,distance(IN.localTexcoord.xy, _DrawPosition));
                
                return drawColor; //min(drawColor, previousColor);
            }
            ENDCG
        }
    }
}