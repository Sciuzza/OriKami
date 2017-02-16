Shader "Custom/ColorAnimatedShader" {
	Properties{
		_Color1("Albedo 1", Color) = (1,1,1,1)
		_Color2("Albedo 2", Color) = (1,1,1,1)
		_LerpTime("Lerp time", Range(0.1,10)) = 1
	}
	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}	
		CGPROGRAM
		// Shader program in CG is written here
		#pragma surface surf Standard

		// Vertex properties
		struct Input {
			float3 worldPos;
		};

		// Material properties
		float4 _Color1;
		float4 _Color2;
		float _LerpTime;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			o.Albedo.rgb = lerp(_Color1, _Color2, + .5*cos(_Time.y / _LerpTime) +.5);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
