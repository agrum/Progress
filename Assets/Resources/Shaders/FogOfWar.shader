// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/FogOfWar" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_FogRadius ("FogRadius", Float) = 1.0
	_FogMaxRadius ("FogMaxRadius", Float) = 0.5
	_Player1_pos ("Player1_pos", Vector) = (0,0,0,1)
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 200
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off

	CGPROGRAM
	#pragma surface surf Lambert vertex:vert alpha:blend

	sampler2D _MainTex;
	fixed4 _Color;
	float _FogRadius;
	float _FogMaxRadius;
	float4 _Player1_pos;

	struct Input {
		float2 uv_MainTex;
		float2 location;
	};

	void vert(inout appdata_full vertexData, out Input outData)
	{
		float4 pos = UnityObjectToClipPos(vertexData.vertex);
		float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);
		outData.uv_MainTex = vertexData.texcoord;
		outData.location = posWorld.xz;
	}

	float powerForPos(float4 pos, float2 nearVertex);

	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

		float alpha = (1.0 - powerForPos(_Player1_pos, IN.location));

		o.Albedo = baseColor.rgb;
		o.Alpha = baseColor.a * alpha;
	}

	float powerForPos(float4 pos, float2 nearVertex)
	{
		float atten = (_FogRadius - length(pos.xz - nearVertex.xy));
		return max(0.0, atten / _FogRadius);
	}
	ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}
