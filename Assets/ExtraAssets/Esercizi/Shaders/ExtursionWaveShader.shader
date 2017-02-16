Shader "Custom/ExtrusionShader" {
	Properties{
		_Color("Albedo", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_ExtrusionAmount("Extrusion Amount", Range(0,2)) = 0.2
		_SineFrequency("Sine Frequency", Range(0,2)) = 0.2
	}
	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}	
		CGPROGRAM
		// Shader program in CG is written here
		#pragma surface surf Standard vertex:vert

		// Vertex properties
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		// Material properties
		float4 _Color;
		sampler2D _MainTex;
		float _ExtrusionAmount;
		float _SineFrequency;

		void vert(inout appdata_full v) {
			float3 worldPos = mul(UNITY_MATRIX_M, v.vertex);
			v.vertex.xyz += v.normal * lerp(0, _ExtrusionAmount, .5*sin(_Time.w/_SineFrequency + worldPos.y) + .5);
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
			o.Emission = lerp(0, _Color, .5*sin(_Time.w + IN.worldPos.y) + .5);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
