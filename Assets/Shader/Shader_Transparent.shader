Shader "Custom/Shader_Transparent"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _HighLightColor ("HighLightColor", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _AlphaScale ("AlphaScale", Range(0.01,1)) = 0.5
        _Speed ("Speed", Range(0.01,100)) = 1
		_TexScale ("TexScale", Range(0.0001,1)) = 1
		_Wavewidth ("Wavewidth", Range(0.01,10)) = 1
    }
    SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		// Extra pass that renders to depth buffer only
		Pass {
			ZWrite On
			ColorMask 0
		}
		
		Pass {
			Tags { "LightMode"="ForwardBase" }
			
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"
			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _AlphaScale;
            fixed _Speed;
			fixed4 _HighLightColor;
			fixed _TexScale;
			float _Wavewidth;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};
			
			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
            
				float2 texUV=(i.worldPos.y,i.worldPos.y)*_TexScale;
				texUV-=_Time*_Speed;
				//texUV=texUV%1;
				fixed4 texColor = tex2D(_MainTex, texUV);

				fixed3 emission = texColor.a*_HighLightColor.rgb;
				
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz ;
				
				fixed3 diffuse = _LightColor0.rgb  * max(0, dot(worldNormal, worldLightDir));

		
				
				return fixed4(_Color.rgb+emission , _AlphaScale);
			}
			
			ENDCG
		}
	} 
	FallBack "Transparent/VertexLit"
}
