Shader "Custom/MolShader" 
{
	Properties 
	{
	    _Color ("Color", Color) = (1,1,1,1)	    
        _SpriteSize("SpriteSize", Float) = 1
	}
	SubShader 
	{
	    Pass 
	    {
	    	Tags { "RenderType"="Opaque" }
	    	LOD 200
	    
			CGPROGRAM				

				#pragma only_renderers d3d11
				#pragma target 5.0
				
				#include "UnityCG.cginc"

				#pragma vertex   VS_Main
				#pragma geometry GS_Main
				#pragma fragment FS_Main
				#pragma hull HS_Main
				#pragma domain DS_Main
				
				// **************************************************************
				// Samplers
				// **************************************************************
				Texture2D _atomTexture;

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
				int _atomCount;		

				// **************************************************************
				// Vertex Program												*
				// **************************************************************			

				GS_INPUT VS_Main (appdata_base v)
				{
				    GS_INPUT output;
				    
				    output.pos = mul (UNITY_MATRIX_MV, v.vertex);
				    
				    return output;
				}
				
				// **************************************************************
				// Hull shader Program												*
				// **************************************************************
				struct ConstantOutputType
				{
				    float edges[3] : SV_TessFactor;
				    float inside[2] : SV_InsideTessFactor;
				};
				struct HS_ConstantOutput
				{
					// Tess factor for the FF HW block
					float fTessFactor[3] : SV_TessFactor;
					float fInsideTessFactor[2] : SV_InsideTessFactor;

					// molecul position
					float3 center : CENTER;
				};
				struct HS_ControlPointOutput
				{
					float3 f3Position : POS;
					float3 f3Normal : NORMAL;
					float2 f2TexCoord : TEXCOORD;
				};
				
				HS_ConstantOutput HS_MainConstant( InputPatch<HS_Input, 3> I )
				{
					HS_ConstantOutput O = (HS_ConstantOutput)0;

					// Simply output the tessellation factors from constant space 
					// for use by the FF tessellation unit
					O.fTessFactor[0] = 3000;
					O.fTessFactor[1] = 1;
					O.fTessFactor[2] = 1;
					O.fInsideTessFactor[0] = 0;
					O.fInsideTessFactor[1] = 0;

						
					// Assign Positions
					float3 f3B003 = I[0].f4Position.xyz;
					float3 f3B030 = I[1].f4Position.xyz;
					float3 f3B300 = I[2].f4Position.xyz;
					// And Normals
					float3 f3N002 = I[0].f3Normal;
					float3 f3N020 = I[1].f3Normal;
					float3 f3N200 = I[2].f3Normal;

					//retrieve ID molecule, position and orientation

					return O;
				}

				[patchsize(3)]
				[partitioning("fractional_odd")]
				[outputtopology("line")]
				[patchconstantfunc("HS_MainConstant")]
				[outputcontrolpoints(1)]
				HS_ControlPointOutput HS_Main( InputPatch<HS_Input, 3> I, uint uCPID : SV_OutputControlPointID )
				{
						HS_ControlPointOutput O = (HS_ControlPointOutput)0;
						// Just pass through inputs = fast pass through mode triggered
						
						O.f3Position = I[uCPID].f4Position.xyz;
						O.f3Normal = I[uCPID].f3Normal;
						O.f2TexCoord = I[uCPID].f2TexCoord;

						return O;
				}
				
				================================================== =============================
				// domain shader generating
				//================================================== ================================================== =============================
				struct DS_Output
				{
					float4 f4Position : SV_Position;
				};
				[domain("tri")]
				DS_Output DS_PNTriangles( HS_ConstantOutput HSConstantData, const OutputPatch<HS_ControlPointOutput, 3> I, float2 params : SV_DomainLocation )
				{
					DS_Output O = (DS_Output)0;

					// The barycentric coordinates
					float fU = params.x;
					float fV = params.y;

					//get molecular position
					float3 pos = I[0].f3Position;
					
					float3 atomPos = tex2D(_atomTex, float4(v.texcoord.xy,0,0));

					// Transform position with projection matrix
					O.f4Position = mul (UNITY_MATRIX_P, float4(f3Position.xyz,1.0));

					return O;
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
	Fallback Off
} 

