Shader "Custom/Shader_Fresnel"
{
    Properties
    {
        _Color ("OutlineColor", Color) = (1,1,1,1)
        _OutlineStrength ("OutlineStrength", Range(0.1,5)) = 2
        _Cutoff ("Cutoff", Range(0.1,1)) = 0.5
    }
    SubShader
    {
        Cull Back
        Tags{"Queue"="AlphaTest""RenderType"="TransparentCutOut""IgnoreProjector"="True"}
        pass{
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
                    float4 col=_Color*fresnel;
                    clip(col.a-_Cutoff);

                    return col;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
