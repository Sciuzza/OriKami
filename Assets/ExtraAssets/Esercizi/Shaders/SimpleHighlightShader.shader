Shader "Custom/SimpleHighlightShader" {
	Properties{
		_Color("Highlight Color", Color) = (1,1,1,1)
		_DiffuseColor("Diffuse color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
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
			float3 viewDir;
			float2 uv_MainTex;
		};

		// Material properties
		float4 _Color;
		float4 _DiffuseColor;
		float _SnowAmount;
		sampler2D _MainTex;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _DiffuseColor;
			float normalDotView = dot(IN.viewDir, IN.worldNormal);
			o.Emission.rgb = lerp(float4(0,0,0,0), _Color, frac(_Time.w*10+1-normalDotView));
		}
		ENDCG
	}
	Fallback "Diffuse"
}
