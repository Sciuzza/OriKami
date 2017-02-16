Shader "Custom/DiffuseRedStandard" {
	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}	
		CGPROGRAM
		// Shader program in CG is written here
		#pragma surface surf Standard

		struct Input {
			float4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutputStandard o) {
			o.Albedo.rgb = fixed4(1, 0, 0, 1);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
