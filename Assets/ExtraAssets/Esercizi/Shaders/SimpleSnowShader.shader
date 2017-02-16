Shader "Custom/SimpleSnowShader" {
	Properties{
		_Color("Snow Color", Color) = (1,1,1,1)
		_SnowAmount("Snow Amount", Range(0.1,10)) = 1
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
			float3 worldNormal;
		};

		// Material properties
		float4 _Color;
		float _SnowAmount;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			float upDotNormal = dot(float3(0, 1, 0), IN.worldNormal);
			o.Emission.rgb = lerp(float4(0,0,0,0), _Color, saturate(upDotNormal));
		}
		ENDCG
	}
	Fallback "Diffuse"
}
