Shader "Custom/FieldOfView/View Stencil Mask" {
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
				float4 color : COLOR0;
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

			void frag(v2f i, out float depth : DEPTH, out float4 color : COLOR0)
			{
				color = float4(1, 1, 1, 1);
				depth = 1;
			}
			ENDCG
		}
	}
}
