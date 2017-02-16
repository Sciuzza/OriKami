Shader "Unlit/UnlitShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		// Vertex properties
		struct vertInput {
			float4 pos : POSITION;
			float2 uv : TEXCOORD0;
		};
		struct vertOutput {
			float4 pos: SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		// Material properties
		half4 _Color;
		sampler2D _MainTex;

		vertOutput vert(vertInput input) {
			vertOutput o;
			o.pos = mul(UNITY_MATRIX_MVP, input.pos);
			o.uv = input.uv;
			return o;
		}

		half4 frag(vertOutput output) : COLOR {
			return tex2D(_MainTex, output.uv) * _Color;
		}

		ENDCG
		}
	}
}
