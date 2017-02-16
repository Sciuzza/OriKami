Shader "Unlit/OutlinePartShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(0.01,1)) = 0.2
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Pass{
		Cull Front

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		struct vertInput {
			float4 pos : POSITION;
			float3 normal : NORMAL;
		};

		struct vertOutput {
			float4 pos: SV_POSITION;
			float3 normal : NORMAL;
		};

		// Material property
		half4 _Color;
		float _OutlineWidth;

		vertOutput vert(vertInput input) {
			vertOutput o;

			// Extrude positions
			o.pos = input.pos;
			o.pos += float4(input.normal*_OutlineWidth, 0);

			// Project positions
			o.pos = mul(UNITY_MATRIX_MVP, o.pos);

			// Invert normals
			o.normal = -input.normal;

			// Project normals
			o.normal = mul(UNITY_MATRIX_M, o.normal);
			return o;
		}

		half4 frag(vertOutput output) : COLOR{
			return _Color;
		}

		ENDCG
		}
	}
}
