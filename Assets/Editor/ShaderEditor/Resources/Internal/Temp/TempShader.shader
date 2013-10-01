Shader "ShaderEditor/EditorShaderCache"
{
	Properties 
	{
_Color("_Color", Color) = (1,0,0,1)
_RimColor("_RimColor", Color) = (0.0597015,0.2503912,1,1)
_RimPower("_RimPower", Range(0.1,3) ) = 0.7566038
_Offset("_Offset", Range(1,2) ) = 2
_SpecPower("_SpecPower", Range(0.1,1) ) = 0.9
_MySpecColor("_MySpecColor", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="True"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest Always


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 3.0


float4 _Color;
float4 _RimColor;
float _RimPower;
float _Offset;
float _SpecPower;
float4 _MySpecColor;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha + Luminance(spec);
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				viewDir = normalize(viewDir);
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot (s.Normal, lightDir));
				
				float nh = max (0, dot (s.Normal, h));
				float3 spec = pow (nh, s.Specular*128.0) * s.Gloss;
				
				half4 res;
				res.rgb = _LightColor0.rgb * (diff * atten * 2.0);
				res.w = spec * Luminance (_LightColor0.rgb);

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float3 viewDir;

			};


			void vert (inout appdata_full v, out Input o) {
float4 Multiply0=float4( v.normal, 1.0) * float4( 1);
float4 Add0=Multiply0 + v.vertex;
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);
v.vertex = Add0;


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Albedo = 0.0;
				o.Normal = float3(0.0,0.0,1.0);
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Alpha = 1.0;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=float4( 1.0 - dot( normalize( float4(IN.viewDir, 1.0).xyz), normalize( Fresnel0_1_NoInput.xyz ) ) );
float4 Pow0=pow(Fresnel0,_RimPower.xxxx);
float4 Multiply1=Pow0 * _Offset.xxxx;
float4 Floor0=floor(Multiply1);
float4 Frac0=frac(Multiply1);
float4 Subtract1=Floor0 - Frac0;
float4 Saturate0=saturate(Subtract1);
float4 Subtract0=Saturate0 - Multiply1;
float4 Multiply0=_RimColor * Subtract0;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = _Color;
o.Emission = Multiply0;
o.Specular = _SpecPower.xxxx;
o.Gloss = _MySpecColor;
o.Alpha = 1.0;

			}
		ENDCG
	}
	Fallback "Diffuse"
}