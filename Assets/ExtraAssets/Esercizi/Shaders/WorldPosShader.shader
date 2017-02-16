Shader "Custom/WorldPosShader" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_Center("Center", Vector) = (0,0,0,0)
		_Radius("Radius", Range(0.1,10)) = 0.5
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
			float2 uv_MainTex;
		};

		// Material properties
		sampler2D _MainTex;
		float4 _Center;
		float _Radius;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			float d = distance(_Center, IN.worldPos);
			float dN = 1 - saturate(d / _Radius);
			if (dN > 0.25 && dN <= 1)
				o.Metallic = 1;
			else
				o.Metallic = 0;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
