Shader "Unlit/fakeInterior"
{
    Properties
    {
        _RoomTex ("Texture", CUBE) = "white" {}
        _Rotate("Rotate around XYZ",vector)=(0,0,0)
        _RoomDepth("Room Depth",Range(0.0001,0.9999))=0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            //旋转函数
            float3 rotateVectorAboutX(float angle, float3 vec)
            { 
            angle = radians(angle);
            float3x3 rotationMatrix ={float3(1.0,0.0,0.0),
                                        float3(0.0,cos(angle),-sin(angle)),
                                        float3(0.0,sin(angle),cos(angle))};
            return mul(vec, rotationMatrix);
            }
            
            float3 rotateVectorAboutY(float angle, float3 vec)
            { 
            angle = radians(angle);
            float3x3 rotationMatrix ={float3(cos(angle),0.0,sin(angle)),
                                        float3(0.0,1.0,0.0),
                                        float3(-sin(angle),0.0,cos(angle))};
            return mul(vec, rotationMatrix);
            }
            
            float3 rotateVectorAboutZ(float angle, float3 vec)
            {
            angle = radians(angle);
            float3x3 rotationMatrix ={float3(cos(angle),-sin(angle),0.0),
                                        float3(sin(angle),cos(angle),0.0),
                                        float3(0.0,0.0,1.0)};
            return mul(vec, rotationMatrix);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 vertex_OS:TEXCOORD1;
                float3 viewDir_OS:TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            samplerCUBE _RoomTex;
            float4 _RoomTex_ST;
            float4 _Rotate;
            float _RoomDepth;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // slight scaling adjustment to work around "noisy wall" 
                // when frac() returns a 0 on surface
                o.vertex_OS = v.vertex * _RoomTex_ST.xyx * 0.999 + float3(_RoomDepth,_RoomTex_ST.wz);

                // get object space camera vector
                float4 objCam = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0));
                o.viewDir_OS = v.vertex.xyz - objCam.xyz;

                // adjust for tiling
                o.viewDir_OS *= _RoomTex_ST.xyx;
                return o;
            }

            fixed4 frag(v2f i):SV_TARGET{
                float3 d=normalize(i.viewDir_OS);
                
                float3 pos=frac(i.vertex_OS);
                pos=pos*2-1;

                float tx=abs(1/d.x)-pos.x/d.x;
                float ty=abs(1/d.y)-pos.y/d.y;
                float tz=abs(1/d.z)-pos.z/d.z;
                float t=min(tx,min(ty,tz));

                float3 p2=pos+t*d;
                //rotate
                p2=rotateVectorAboutX(_Rotate.x,p2);
                p2=rotateVectorAboutY(_Rotate.y,p2);
                p2=rotateVectorAboutZ(_Rotate.z,p2);

                fixed3 col;
                col=texCUBE(_RoomTex,p2);
                return fixed4(col,1.0);

            }
           
            ENDCG
        }
    }
}
