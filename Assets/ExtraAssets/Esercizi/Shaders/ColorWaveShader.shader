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

		float3 Hue(float H)
		{
			float R = abs(H * 6 - 3) - 1;
			float G = 2 - abs(H * 6 - 2);
			float B = 2 - abs(H * 6 - 4);
			return saturate(float3(R, G, B));
		}

		float4 HSVtoRGB(in float3 HSV)
		{
			return float4(((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z, 1);
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			//o.Albedo.rgb = HSVtoRGB(frac(_Time.z));
			//o.Smoothness = IN.worldPos.x;
			o.Albedo.rgb = lerp(_Color1, _Color2, .5*sin(IN.worldPos.x*.1 + _Time.w/_LerpTime)+.5);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
