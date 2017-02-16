Shader "Custom/SimpleTextureShader"{
	Properties{
		_Color("The Best Color", Color) = (1,1,1,1)
		_MainTex("The Best Texture", 2D) = "white" {}
		_EmissiveRatio("The Most Emissive Ratio", Range(0.1, 4.0)) = 0.7
		_Emissive("The Most Emissive Color", Color) = (1,1,1,1)
		_DetailTex("The Most Detailed Texture", 2D) = "white" {}
		//_TestInt("Test Int", Int) = 4
		//_TestCube("Test Cube", Cube) = "defaulttexture" {}
		//_Test3D("Test 3D", 3D) = "defaulttexture" {}
	}
	SubShader{
		Pass{
			Material{
				Diffuse[_Color]
				Emission[_Emissive]
			}
			Lighting On

			SetTexture[_MainTex] {
				combine previous * texture
			}
			SetTexture[_DetailTex]{
				combine previous lerp(texture) texture
			}
		}
	}
}