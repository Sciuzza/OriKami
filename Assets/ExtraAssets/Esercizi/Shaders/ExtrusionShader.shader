Shader "Custom/ExtrusionShader" {
	Properties{
		_Color("Albedo", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_ExtrusionAmount("Extrusion Amount", Range(0,2)) = 0.2
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
		};

		// Material properties
		float4 _Color;
		sampler2D _MainTex;
		float _ExtrusionAmount;

		void vert(inout appdata_full v) {
			v.vertex.xyz += v.normal * _ExtrusionAmount;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
