Shader "Custom/CustomStandardShader" {
	Properties{
		_Color("Albedo", Color) = (1,1,1,1)
		_Metallic("Metallic", Range(0,1)) = 0
		_Smoothness("Smoothness", Range(0,1)) = 0
		_Pippo("Main Texture", 2D) = "white" {}
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
			float4 color : COLOR;
			float3 worldPos;
			float2 uv_Pippo;
		};

		// Material properties
		float4 _Color;
		float _Metallic;
		float _Smoothness;
		sampler2D _Pippo;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			//o.Albedo.rgb = _Color;
			o.Albedo = tex2D(_Pippo, IN.uv_Pippo).rgb;
			o.Albedo = _Color - o.Albedo;

			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			//o.Emission.rgb = IN.worldPos;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
