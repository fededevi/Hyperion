 Shader "Example/Rim" {
    Properties {
      //_MainTex ("Texture", 2D) = "white" {}
      //_BumpMap ("Bumpmap", 2D) = "bump" {}
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,1.0)
      _RimPower ("Rim Power", Range(0.2,5.0)) = 3.0
      _RimOffset ("Rim Offset", Range(1.0,4.0)) = 1.1
    }
    SubShader {
     Tags { "Queue"="transparent" "RenderType"="transparent" }
      CGPROGRAM
      #pragma surface surf BlinnPhong alpha
      struct Input {
          //float2 uv_MainTex;
          //float2 uv_BumpMap;
          float3 viewDir;
      };
      sampler2D _MainTex;
      sampler2D _BumpMap;
      float4 _RimColor;
      float _RimPower;
      float _RimOffset;
      
      void surf (Input IN, inout SurfaceOutput o) {
      
      				// i.normal = normalize(i.normal);
                    //i.viewdir = normalize(i.viewdir);
                    
                    //float4 color = _AtmoColor;
                    //color.a = pow(saturate(dot(i.viewdir, i.normal)), _Falloff);
                    //color.a *= _Transparency*_Color;
      
          //o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          //o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
		  //float3 dir = mul (UNITY_MATRIX_MV , (0.0,0.0,1.0);
          half rim = 1.0 - dot (normalize( IN.viewDir ), o.Normal);
       	  rim = pow (rim, _RimPower);
       	  rim = rim * _RimOffset;
       	  if (rim > 1.0) rim = 2.0 - rim;
       	  
          o.Emission = ( _RimColor * rim );
          
          o.Alpha = rim * _RimColor.a;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }