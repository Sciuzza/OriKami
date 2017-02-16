Shader "Unlit/SolidColorShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		struct vertInput {
			float4 pos : POSITION;
		};

		struct vertOutput {
			float4 pos: SV_POSITION;
		};

		vertOutput vert(vertInput input) {
			vertOutput o;
			o.pos = mul(UNITY_MATRIX_MVP, input.pos);
			return o;
		}

		// Material property
		half4 _Color;

		half4 frag(vertOutput output) : COLOR{
			return _Color;
		}

		ENDCG
		}
	}
}
