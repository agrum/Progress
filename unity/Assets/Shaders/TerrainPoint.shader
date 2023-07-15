Shader "Custom/TerrainPoint"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _WorldTex("World (Overriden at runtime)", 2D) = "grey" {}
        _WorldSize("WorldWidth (Overriden at runtime)", Vector) = (1, 1, 1, 1)
    }
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		Pass
		{
			Cull OFF
			Tags{ "LightMode" = "ForwardBase" }
			AlphaToMask On


			CGPROGRAM

			#include "UnityCG.cginc" 
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#include "UnityLightingCommon.cginc"

			#pragma target 4.0

			sampler2D _MainTex;
			sampler2D _WorldTex;
			fixed4 _Color;
			fixed4 _WorldSize;

			float _Height;
			float _Width;
			struct v2g
			{
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float3 worldPos : WORLD_POS;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float3 worldPos : WORLD_POS;
				float2 faceCenter : FACE_CENTER;
			};

			v2g vert(appdata_full v)
			{
				v2g o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.norm = v.normal;
				o.worldPos = v.vertex;

				return o;
			}

			[maxvertexcount(30)]
			void geom(triangle v2g points[3], inout TriangleStream<g2f> triStream)
			{
				float2 faceCenter = (points[0].worldPos.xz + points[1].worldPos.xz + points[2].worldPos.xz) * 0.3333f;

				g2f t1;
				t1.pos = points[0].pos;
				t1.norm = points[0].norm;
				t1.worldPos = points[0].worldPos;
				t1.faceCenter = faceCenter;
				triStream.Append(t1);

				g2f t2;
				t2.pos = points[1].pos;
				t2.norm = points[1].norm;
				t2.worldPos = points[1].worldPos;
				t2.faceCenter = faceCenter;
				triStream.Append(t2);

				g2f t3;
				t3.pos = points[2].pos;
				t3.norm = points[2].norm;
				t3.worldPos = points[2].worldPos;
				t3.faceCenter = faceCenter;
				triStream.Append(t3);
			}


			half4 frag(g2f IN) : COLOR
			{
				int3 faceCenter = IN.worldPos - IN.norm * 0.01;
				float blockColorFactor = 0.1f + 0.1f * faceCenter.y;

				fixed4 worldData = tex2D(_WorldTex, (IN.faceCenter.xy - IN.norm.xz * 0.01) / _WorldSize.xy);
				fixed4 color = tex2D(_MainTex, worldData.xy) * _Color * (1 - worldData.b - blockColorFactor);
				return fixed4(color.rgb, 1);

			}
			ENDCG
		}

		GrabPass
		{
			"_IntermediateTexture"
		}

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _WorldTex;
		sampler2D _IntermediateTexture;
		fixed4 _WorldSize;

		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			//fixed4 worldData = tex2D(_WorldTex, (IN.worldPos.xz - IN.worldNormal.xz * 0.01) / _WorldSize.xz);
			//o.Albedo = (tex2D(_MainTex, worldData.xz) * _Color).rgb * (1 - worldData.b);
			o.Albedo = tex2D(_IntermediateTexture, IN.screenPos / IN.screenPos.w).rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
