Shader "Custom/AlphaCutoff" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_RimPower("Rim Position", range(0,4)) = 0.01
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 200

		CGPROGRAM
		#pragma surface surf AAA alphatest:_Cutoff

		fixed4 _Color;
		half _RimPower;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		fixed4 LightingAAA (SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}


		void surf (Input IN, inout SurfaceOutput o)
		{
			half NdotView = 1 - dot(normalize(IN.viewDir), o.Normal);
			
			NdotView = pow(NdotView, _RimPower);
			
			half4 rimColor = _Color - half4(0.5, 0.5, 0.5, 0.5);
			
			o.Albedo = _Color * ( 1 - NdotView );
			o.Alpha = _Color.a;
		}
ENDCG
}

Fallback "Custom/Basic"
}
