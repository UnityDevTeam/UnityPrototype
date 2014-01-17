Shader "Custom/MolShader" 
{
	Properties 
	{
	    _Color ("SpriteColor", Color) = (1,1,1,1)	    
        _SpriteSize("SpriteSize", Float) = 1
	}
	SubShader 
	{
	    Pass 
	    {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;
			float _SpriteSize;

			struct vertexInput 
			{
	            float4 pos : POSITION;
        	};

			struct fragmentInput 
			{
			    float4 pos : SV_POSITION;			    
			    float size : PSIZE;
			};

			fragmentInput vert (vertexInput v)
			{
			    fragmentInput output;
			    
			    output.pos = mul (UNITY_MATRIX_MVP, v.pos);
			    output.size = _SpriteSize;
			    
			    return output;
			}

			half4 frag (fragmentInput f) : COLOR
			{
				// If the distance is greater than 0 we discard the pixel
			    //if (mag > 1) discard;	   			    
			    return half4 (_Color);
			}
			ENDCG
		 }
	}
} 

