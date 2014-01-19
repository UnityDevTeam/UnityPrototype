Shader "Custom/SimpleRS" 
{
    Properties 
    {
        _ParticleTexture ("Diffuse Tex", 2D) = "white" {}
        _Ramp1Texture ("G_Ramp1", 2D) = "white" {}
    }

    SubShader 
    {
        Pass 
        {
            Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
            Blend OneMinusDstColor One
            Cull Off 
            Lighting Off 
            ZWrite Off 
            Fog { Color (0,0,0,0) }


            CGPROGRAM
            #pragma target 5.0
            #pragma vertex VSMAIN
            #pragma fragment PSMAIN
            #pragma geometry GSMAIN
            #include "UnityCG.cginc" 

            struct Particle 
            {
                int    index;
                float3 position;
                float3 velocity;
                float  size;
                float  age;
                float  normAge;
                int    type;

            };

            StructuredBuffer<Particle>  particles;

            Texture2D                   _ParticleTexture;           
            SamplerState                sampler_ParticleTexture;

            Texture2D                   _Ramp1Texture;
            SamplerState                sampler_Ramp1Texture;

            float maxAge;
            float maxRad;

            struct VS_INPUT
            {
                uint vertexid           : SV_VertexID;
            };
            //--------------------------------------------------------------------------------
            struct GS_INPUT
            {
                float4 position         : SV_POSITION;
                float size              : TEXCOORD0;
                float age               : TEXCOORD1;
                float type              : TEXCOORD2;
            };
            //--------------------------------------------------------------------------------
            struct PS_INPUT
            {
                float4 position         : SV_POSITION;
                float2 texcoords        : TEXCOORD0;
                float size              : TEXCOORD1;
                float age               : TEXCOORD2;
                float type              : TEXCOORD3;
            };
            //--------------------------------------------------------------------------------
            GS_INPUT VSMAIN( in VS_INPUT input )
            {
                GS_INPUT output;

                output.position.xyz = particles[input.vertexid].position;
                output.position.w = 1.0;                
                output.age = particles[input.vertexid].normAge;
                output.size = particles[input.vertexid].size;
                output.type = particles[input.vertexid].type;
                return output;
            }
            //--------------------------------------------------------------------------------
            [maxvertexcount(4)]
            void GSMAIN( point GS_INPUT p[1], inout TriangleStream<PS_INPUT> triStream )
            {           
                float4 pos = mul(UNITY_MATRIX_MVP, p[0].position);

                float halfS = p[0].size * 0.5f;
                float4 offset = mul(UNITY_MATRIX_P, float4(halfS, halfS, 0, 1));

                float4 v[4];
                v[0] = pos + float4(offset.x, offset.y, 0, 1);
                v[1] = pos + float4(offset.x, -offset.y, 0, 1);
                v[2] = pos + float4(-offset.x, offset.y, 0, 1);
                v[3] = pos + float4(-offset.x, -offset.y, 0, 1);

                PS_INPUT pIn;
                pIn.position = v[0];
                pIn.texcoords = float2(1.0f, 0.0f);

                    pIn.size = p[0].size;
                    pIn.age = p[0].age;
                    pIn.type = p[0].type;                       

                triStream.Append(pIn);

                pIn.position =  v[1];
                pIn.texcoords = float2(1.0f, 1.0f);
                triStream.Append(pIn);

                pIn.position =  v[2];
                pIn.texcoords = float2(0.0f, 0.0f);
                triStream.Append(pIn);

                pIn.position =  v[3];
                pIn.texcoords = float2(0.0f, 1.0f);
                triStream.Append(pIn);                  
            }
            //--------------------------------------------------------------------------------
            float4 PSMAIN( in PS_INPUT input ) : COLOR
            {
                float4 color = _ParticleTexture.Sample( sampler_ParticleTexture, input.texcoords );
                float4 tint = _Ramp1Texture.Sample(sampler_Ramp1Texture, float2(min(1.0, input.age),0));
                color *= tint;

                if (input.age == 0) discard;

                return color;
            }
            //--------------------------------------------------------------------------------
            ENDCG
        }
    } 
}