Shader "Custom/SciuzzaShader"
{
	Properties
	{
		_Color1("Albedo1", Color) = (1,1,1,1)
		_Color2("Albedo2", Color) = (1,1,1,1)
		_LerpTime("Lerp TIme", Range(0.1,10)) = 1
	}

		SubShader
	{
	 Tags
		{
			"RenderType" = "Opaque"
		}

		CGPROGRAM
		#pragma surface surf Standard

		struct Input
		{
			float3 worldPos;
		};

		float4 _Color1;
		float4 _Color2;
		float _LerpTime;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo.rgb = lerp(_Color1, _Color2, +.5 * sin(_Time.y / _LerpTime) + .5);
		}
		ENDCG
	}
		Fallback "Diffuse"
}