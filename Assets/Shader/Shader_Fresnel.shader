Shader "Custom/Shader_Fresnel"
{
    Properties
    {
        _Color ("OutlineColor", Color) = (1,1,1,1)
        _OutlineStrength ("OutlineStrength", Range(0.1,5)) = 2
        _Emiss("Emiss",Range(1,10))=1
        _Cutoff ("Cutoff", Range(-1,1)) = 0.5
    }
    SubShader
    {
    
        Tags{"Queue"="Transparent"}
        Name"preZwrite"
        pass {
            Cull Off
            ZWrite On
            ColorMask 0
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _Color;
            float4 vert(float4 v:POSITION):SV_POSITION{
                return UnityObjectToClipPos(v);
            }
            float4 frag():COLOR{
                return _Color;
            }
            ENDCG

        }
        pass{
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct a2v {
                float4 vertex:POSITION;
                float3 normal:NORMAL;
            };

            struct v2f{
                float4 pos:SV_POSITION;
                float3 pos_ws:TEXCOORD0;
                float3 normal_ws:TEXCOORD1;

            };
            float4 _Color;
            fixed _OutlineStrength;
            fixed  _Cutoff;
            fixed _Emiss;

            v2f vert(a2v i){
                v2f o;
                o.pos=UnityObjectToClipPos(i.vertex);
                o.pos_ws=mul(unity_ObjectToWorld,i.vertex);
                o.normal_ws=normalize( UnityObjectToWorldNormal(i.normal));
                return o;
            }

            float4 frag(v2f v):SV_TARGET{
                
                    float3 view_Dir=normalize(_WorldSpaceCameraPos-v.pos_ws);
                    float fresnel=pow(1-saturate(dot(v.normal_ws,view_Dir)),_OutlineStrength);
                    float3 col=_Color*fresnel*_Emiss;
                    float alpha=saturate((fresnel-_Cutoff)*_Emiss);
                   

                    return float4(col,alpha);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
