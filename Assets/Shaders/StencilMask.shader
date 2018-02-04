Shader "Custom/Stencil Mask" {
	Properties 
	{
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry-100" }
		ColorMask 0
		ZWrite Off
		LOD 200

		Stencil 
		{
			Ref 1
			Pass replace
		}
		
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
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return (0, 0, 0, 0);
			}
			ENDCG
		}
	}
}
