Shader "her0in/X-Ray/Replaceable"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_EdgeColor("XRay Edge Color", Color) = (0,0,0,0)
		_MainTex("Base (RGB)", 2D) = "white" {}

		_BumpMap("Normal map", 2D) = "bump" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}

	SubShader
	{
		Tags
		{
			//////////////////////////////////////////////////////////////////////////////////////////////////////
			// In some cases, it's necessary to force XRay objects to render before the rest of the geometry	//
			// This is so their depth info is already in the ZBuffer, and Occluding objects won't mistakenly	//
			// write to the Stencil buffer when they shouldn't.													//
			//																									//
			// This is what "Queue" = "Geometry-1" is for.														//
			// I didn't bring this up in the video because I'm an idiot.										//
			//																									//
			// Cheers,																							//
			// Dan																								//
			//////////////////////////////////////////////////////////////////////////////////////////////////////
			"Queue" = "Geometry-1"
			"RenderType" = "Opaque"
			"XRay" = "ColoredOutline"
		}
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _BumpMap;

		fixed4 _Color;
		half _Metallic;
		half _Glossiness;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	
	Fallback "Legacy Shaders/VertexLit"
}
