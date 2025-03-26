Shader "Unlit/PaintShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Coordinates("Coordinate", Vector) = (0,0,0,0) // Holding the coordinates player wants to paint
        _Color("Draw Color", Color) = (1,0,0,0) // Determines the color paint in texture
        _Strength("Strength", Range(0,1)) = 1 // Amount of painting
        _Size("Size", Range(1,500)) = 0 // Amount of painting
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Coordinates, _Color;
            half _Size, _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // holds values of the render texture
                float draw = pow(saturate(1-distance(i.uv, _Coordinates.xy)), 500/_Size);
                fixed4 drawcol = _Color * (draw * _Strength);
                return saturate(col + drawcol);
            }
            ENDCG
        }
    }
}
