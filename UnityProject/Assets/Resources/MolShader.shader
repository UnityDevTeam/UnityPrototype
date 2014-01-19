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
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;
			float _SpriteSize;
			struct PosVelo
			{
				float3 position;
				float3 velocity;
			};
			
			StructuredBuffer<PosVelo>  particles;
			
			struct vertexInput 
			{
	            uint vertexid           : SV_VertexID;
        	};
			struct fragmentInput 
			{
			    float4 pos : SV_POSITION;			    
			    float size : PSIZE;
			};

			fragmentInput vert (vertexInput v)
			{
			    fragmentInput output;
			    //float4 position = float4(particles[v.vertexid].position,1.0);
			    float4 position = float4(25,25,25,1.0);
			    output.pos = mul (UNITY_MATRIX_MVP, position);
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

