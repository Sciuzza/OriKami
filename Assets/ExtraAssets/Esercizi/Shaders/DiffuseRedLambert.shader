Shader "Custom/DiffuseRedlambert" {

	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}	
		CGPROGRAM
		// Shader program in CG is written here
		#pragma surface surf Lambert

		struct Input {
			float4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo.rgb = fixed4(1, 1, 1, 1);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
