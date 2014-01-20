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
			#pragma only_renderers d3d11
				#pragma target 5.0
				
				#include "UnityCG.cginc"

				#pragma vertex   VS_Main
				#pragma geometry GS_Main
				#pragma fragment FS_Main

				// **************************************************************
				// Data structures												*
				// **************************************************************
				
				struct GS_INPUT
				{
					float4 pos : SV_POSITION;
				};

				struct FS_INPUT
				{
					float4 pos : SV_POSITION;									
					float3 normal : NORMAL;			
					float2 tex0	: TEXCOORD0;
				};
				
				// **************************************************************
				// Global Vars													*
				// **************************************************************				
				
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
				// **************************************************************
				// Vertex Program												*
				// **************************************************************			

				GS_INPUT VS_Main (vertexInput v)
				{
				    GS_INPUT output;
				    
			    	float4 position = float4(particles[v.vertexid].position,1.0);
				    output.pos = mul (UNITY_MATRIX_MV, position);
				    
				    return output;
				}
				
				// **************************************************************
				// Geometry Program												*
				// **************************************************************	
				
				[maxvertexcount(4)]
				void GS_Main(point GS_INPUT input[1], inout TriangleStream<FS_INPUT> pointStream)
				{
					FS_INPUT output;
					
					output.pos = input[0].pos + float4(  0.5,  0.5, 0, 0) * _SpriteSize;
					output.pos = mul (UNITY_MATRIX_P, output.pos);
					output.normal = float3(0.0f, 0.0f, -1.0f);
					output.tex0 = float2(1.0f, 1.0f);
					pointStream.Append(output);
					
					output.pos = input[0].pos + float4(  0.5, -0.5, 0, 0) * _SpriteSize;
					output.pos = mul (UNITY_MATRIX_P, output.pos);
					output.normal = float3(0.0f, 0.0f, -1.0f);
					output.tex0 = float2(1.0f, 0.0f);
					pointStream.Append(output);					
					
					output.pos = input[0].pos + float4( -0.5,  0.5, 0, 0) * _SpriteSize;
					output.pos = mul (UNITY_MATRIX_P, output.pos);
					output.normal = float3(0.0f, 0.0f, -1.0f);
					output.tex0 = float2(0.0f, 1.0f);
					pointStream.Append(output);
					
					output.pos = input[0].pos + float4( -0.5, -0.5, 0, 0) * _SpriteSize;
					output.pos = mul (UNITY_MATRIX_P, output.pos);
					output.normal = float3(0.0f, 0.0f, -1.0f);
					output.tex0 = float2(0.0f, 0.0f);
					pointStream.Append(output);					
				}
				
				// **************************************************************
				// Fragment Program												*
				// **************************************************************

				float4 FS_Main (FS_INPUT input) : COLOR
				{
					// Center the texture coordinate
				    float3 normal = float3(input.tex0 * 2.0 - float2(1.0, 1.0), 0);

				    // Get the distance from the center
				    float mag = dot(normal, normal);

				    // If the distance is greater than 0 we discard the pixel
				    if (mag > 1) discard;
				
					// Find the z value according to the sphere equation
				    normal.z = sqrt(1.0-mag);
					normal = normalize(normal);
				
					// Lambert shading
					float3 light = float3(0, 0, 1);
					float ndotl = max( 0.0, dot(light, normal));	
				
					return _Color * ndotl;
				}
			ENDCG
		 }
	}
} 

