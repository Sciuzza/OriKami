Shader "Custom/ExtrusionTextureShader" {
	Properties{
		_Color("Albedo", Color) = (1,1,1,1)
		_Color2("Albedo", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_ExtrusionTex("Extrusion Texture", 2D) = "white" {}
		_ExtrusionAmount("Extrusion Amount", Range(0,2)) = 0.2
	}
	SubShader{
		Tags{
			 "Queue" = "Transparent"
			 "RenderType" = "Transparent" 
		}	
		CGPROGRAM
		// Shader program in CG is written here
		#pragma surface surf Standard vertex:vert alpha:fade

		// Vertex properties
		struct Input {
			float2 uv_MainTex;
		};

		// Material properties
		float4 _Color;
		float4 _Color2;
		sampler2D _MainTex;
		sampler2D _ExtrusionTex;
		float _ExtrusionAmount;

		void vert(inout appdata_full v) {
			float uv_x = frac(v.texcoord.x + _Time.x);
			float uv_y = frac(v.texcoord.y + _Time.x);

			float4 extrusion = tex2Dlod(_MainTex, float4(uv_x, uv_y,0,0));
			v.vertex.xyz += v.normal * extrusion * _ExtrusionAmount;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			float uv_x = frac(IN.uv_MainTex.x + _Time.x);
			float uv_y = frac(IN.uv_MainTex.y + _Time.x);
			
			o.Albedo = lerp(_Color, _Color2, tex2D(_MainTex, float2(uv_x,uv_y)).r);
			o.Alpha = lerp(0, 1, tex2D(_MainTex, float2(uv_x, uv_y)).r*2);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
