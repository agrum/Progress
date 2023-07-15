// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TerrainWater" {
Properties {
	[HideInInspector]_MainTex ("Fallback texture", 2D) = "black" {}
	_DistortionTex ("Distortion texture", 2D) = "black" {}
	[HideInInspector]_OceanTex("Ocean (Overriden at runtime)", 2D) = "grey" {}
	[HideInInspector]_WorldSize("WorldWidth (Overriden at runtime)", Vector) = (1, 1, 1, 1)

	_OceanColor ("Base color", COLOR)  = ( .54, .95, .99, 0.5)
	_WavesColor("Waves color", Color) = (0.3 ,0.35, 0.25, 0.25)

	_WavesHeight("Waves height", Range(0.0, 1.0)) = 0.3
	_WavesSpread("Waves spread", Range(0.0, 1.0)) = 0.1
	_TimeScale1("Timescale for wave 1", Float) = 2.0
	_TimeScale2("Timescale for wave 2", Float) = 1.5
}


CGINCLUDE

	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	
	struct v2g
	{
		float4 pos : SV_POSITION; 
		float4 viewInterpolator : TEXCOORD1;
		float4 screenPos : TEXCOORD3;
		float3 worldPos : WORLD_POS;
		UNITY_FOG_COORDS(5)
	};

	struct g2f
	{
		float4 pos : SV_POSITION;
		float3 normal : TEXCOORD0;
		float4 viewInterpolator : TEXCOORD1;
		float4 screenPos : TEXCOORD3;
		float3 worldPos : WORLD_POS;
		UNITY_FOG_COORDS(5)
	};

	// textures
	sampler2D _DistortionTex;
	sampler2D _OceanTex;
	sampler2D _RefractionTex;
	sampler2D_float _CameraDepthTexture;
	fixed4 _WorldSize;

	// colors in use
	uniform float4 _OceanColor;
	uniform float4 _WavesColor;

	//variables
	uniform float _WavesHeight;
	uniform float _WavesSpread;
	uniform float _TimeScale1;
	uniform float _TimeScale2;

	v2g vert(appdata_full v)
	{
		v2g o;
		
		half3 worldSpaceVertex = mul(unity_ObjectToWorld,(v.vertex)).xyz;
		float4 distortionCoord = float4(worldSpaceVertex.x + _Time.x * _TimeScale1, worldSpaceVertex.z + _Time.y * _TimeScale2, 0, 0);
		float yOffest = _WavesHeight * tex2Dlod(_DistortionTex, distortionCoord * _WavesSpread);
		v.vertex.y += yOffest;

		o.viewInterpolator.xyz = worldSpaceVertex;
		o.viewInterpolator.w = 1.0f;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.screenPos = ComputeNonStereoScreenPos(o.pos);
		o.worldPos = v.vertex;
		
		UNITY_TRANSFER_FOG(o,o.pos);
		return o;
	}

	[maxvertexcount(30)]
	void geom(triangle v2g points[3], inout TriangleStream<g2f> triStream)
	{
		float3 normal = (cross(points[0].pos - points[1].pos, points[2].pos - points[1].pos));
		//float3 normal = points[0].pos - points[1].pos;

		g2f t0;
		t0.pos = points[0].pos;
		t0.viewInterpolator = points[0].viewInterpolator;
		t0.screenPos = points[0].screenPos;
		t0.worldPos = points[0].worldPos;
		t0.normal = normal;
		triStream.Append(t0);

		g2f t1;
		t1.pos = points[1].pos;
		t1.viewInterpolator = points[1].viewInterpolator;
		t1.screenPos = points[1].screenPos;
		t1.worldPos = points[1].worldPos;
		t1.normal = normal;
		triStream.Append(t1);

		g2f t2;
		t2.pos = points[2].pos;
		t2.viewInterpolator = points[2].viewInterpolator;
		t2.screenPos = points[2].screenPos;
		t2.worldPos = points[2].worldPos;
		t2.normal = normal;
		triStream.Append(t2);
	}

	half4 frag(g2f i) : SV_Target
	{
		float2 time = float2(_Time.x * _TimeScale1, _Time.x * _TimeScale2);
		float depth = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.screenPos);
		float eyeDepth = LinearEyeDepth(depth);
		float surfaceEyeDepth = LinearEyeDepth(i.pos.z);
		float waterAlpha = (eyeDepth - surfaceEyeDepth) / i.pos.w;
		float oceanDepth = tex2D(_OceanTex, i.worldPos.xz / _WorldSize).r;
		float waveHeight = 1.0f - (_Time.x * 1.5f + oceanDepth + 0.2f * tex2D(_DistortionTex, i.viewInterpolator.xz * 0.1f)) % 1.0f;
		float waveThreshold = 0.985f * saturate(oceanDepth * 4.0f);
		float waveOpacity = saturate(((saturate(1.8f - oceanDepth) * waveHeight) - waveThreshold) / (1.0f - waveThreshold));
		waveOpacity *= saturate(2.0f * (0.25f - oceanDepth + tex2D(_DistortionTex, i.viewInterpolator.xz * 0.1f + time.x) + tex2D(_DistortionTex, i.viewInterpolator.xz * 0.1f + time.y)));

		half4 baseColor = tex2D(_RefractionTex, -i.normal.xy * 300.0f * waterAlpha + i.screenPos / i.screenPos.w);
		float overlayOpacity = saturate(waveOpacity * _WavesColor.a + waterAlpha + surfaceEyeDepth / 100.0f);
		baseColor.rgb = lerp(baseColor.rgb, _OceanColor.rgb, overlayOpacity);
		baseColor.rgb = lerp(baseColor.rgb, _WavesColor.rgb, waveOpacity * _WavesColor.a);
		baseColor.a = 1.0f;
		
		UNITY_APPLY_FOG(i.fogCoord, baseColor);
		return baseColor;
	}
	
ENDCG

Subshader
{
	Tags {"RenderType"="Transparent" "Queue"="Transparent"}
	
	Lod 500
	ColorMask RGB
	
	GrabPass { "_RefractionTex" }
	
	Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Cull Off
		
			CGPROGRAM
		
			#pragma target 3.0
			#pragma require geometry
		
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
			#pragma multi_compile_fog
		
			#pragma multi_compile WATER_VERTEX_DISPLACEMENT_ON WATER_VERTEX_DISPLACEMENT_OFF
			#pragma multi_compile WATER_EDGEBLEND_ON WATER_EDGEBLEND_OFF
			#pragma multi_compile WATER_REFLECTIVE WATER_SIMPLE
		
			ENDCG
	}
}

Fallback "Transparent/Diffuse"
}
