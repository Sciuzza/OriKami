Shader "Custom/SciuzzaShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _EmissiveRatio("The Most Emissive Ratio", Range(0.1, 4.0)) = 0.7
        _DetailTex("Ciccio", 2D) = "white" {}

        _TestInt("Test Int", Int) = 4
        _TestCube("Test Cube", Cube) = "DefaultTexture" {}
        _Test3D("Test 3D", 3D) = "DefaultTexture" {}
    }
    SubShader {
        Pass{
            Material{
            Diffuse[_Color]
        }
            Lighting On

            SetTexture[_MainTex]{
            combine previous - texture
        }
        }
        
    }
    
}
