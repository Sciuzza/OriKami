Shader "Custom/SimpleDiffuseShader"{
	Properties{
		_Color("The Best Color", Color) = (1,1,1,1)
	}
	SubShader{
		Pass{
			Material{
				Diffuse[_Color]
			}
			Lighting On
		}
	}
}