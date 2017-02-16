Shader "Unlit/HalfLambertToonShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_Quantizations("Quantizations", Range(1,10)) = 4
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(0.01,1)) = 0.2
	}
	SubShader
	{
		Tags {
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase"
		}
		Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityLightingCommon.cginc"

		// Vertex properties
		struct vertInput {
			float4 pos : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};
		struct vertOutput {
			float4 pos: SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};

		// Material properties
		half4 _Color;
		sampler2D _MainTex;
		sampler2D _NoiseTex;
		float _Quantizations;

		vertOutput vert(vertInput input) {
			vertOutput o;
			o.pos = mul(UNITY_MATRIX_MVP, input.pos);
			o.uv = input.uv;
			o.normal = mul(UNITY_MATRIX_M, input.normal); 
			return o;
		}

		half4 frag(vertOutput output) : COLOR {
			float NdotL = dot(output.normal, _WorldSpaceLightPos0);
			float value = (0.5*(NdotL)+0.5)*0.5;

			value += (tex2D(_NoiseTex, output.uv)-0.5f)*0.5;
			float value_discretized = round(value * _Quantizations) / _Quantizations;
			return  tex2D(_MainTex, output.uv) * _Color * value_discretized * _LightColor0;
		}

		ENDCG
		}
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
		half4 _OutlineColor;
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

			value += (tex2D(_NoiseTex, output.uv) - 0.5f)*0.5;

			// Project normals
			o.normal = mul(UNITY_MATRIX_M, o.normal);
			return o;
		}

		half4 frag(vertOutput output) : COLOR{
			return _OutlineColor;
		}

			ENDCG
		}
	}
}
