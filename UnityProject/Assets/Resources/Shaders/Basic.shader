Shader "Custom/BasicShader" {
	Properties
	{
		_Color("Color", Color) = (1,0,0)
		_RimPower("Rim Position", range(0,4)) = 2
	}
	
	SubShader {
		//Tags { "RenderType"="Opaque" }
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200	
		
		CGPROGRAM
		#pragma surface surf Cartoon alpha
		//#pragma surface surf Lambert alpha
		
		half4 _Color;
		half _RimPower;
		
		struct Input {
			float3 viewDir;
		};

		half4 LightingCartoon(SurfaceOutput s, half3 dir, half attend)
		{
			half4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			
			return c;
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half NdotView = 1 - dot(normalize(IN.viewDir), o.Normal);
			
			NdotView = pow(NdotView, _RimPower);
			
			half4 rimColor = _Color - half4(0.5, 0.5, 0.5, 0.5);

			//o.Emission = NdotView * rimColor;
			
			o.Albedo = _Color * ( 1 - NdotView );
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}