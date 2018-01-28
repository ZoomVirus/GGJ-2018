// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline" 
 {
     Properties 
     {
         _Color("Color", Color) = (1,0,0,1)
         _Thickness("Thickness", float) = 4
		_WidthScale ("WidthScale", float) = 1
     }
     SubShader 
     {
     
         Tags { /*"Queue"="Geometry" "IgnoreProjector"="True"*/ "RenderType"="Opaque" }
         //Blend SrcAlpha OneMinusSrcAlpha
         Cull Off
         //ZTest always
         Pass
         {
             Stencil {
                 Ref 1
                 Comp always
                 Pass replace
             }
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma multi_compile_fog
             
             #include "UnityCG.cginc"
             
             struct v2g 
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
				 float3 worldPos : TEXCOORD1;
             };
             struct g2f 
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
                 float3  viewT : TANGENT;
                 float3  normals : NORMAL;
				 float3 worldPos : TEXCOORD1;
             };

			 //Globals
			float4 _EmitLocations[100];
			float4 _EmitData[100];

			//uniforms
			//sampler2D _MainTex;
			//float4 _MainTex_ST;
			float _WidthScale;
			float _Discard0;
 
             v2g vert(appdata_base v)
             {
                 v2g OUT;
				 
                OUT.worldPos = mul (unity_ObjectToWorld, v.vertex);
                 OUT.pos = UnityObjectToClipPos(v.vertex);
                 OUT.uv = v.texcoord; 
                  OUT.normals = v.normal;
                 OUT.viewT = ObjSpaceViewDir(v.vertex);
                 
                 return OUT;
             }
             fixed4 frag (g2f i) : SV_Target
			 {
			 
				if(_Discard0 < 1)
					return float4(abs(i.normals.x), abs(i.normals.y),abs(i.normals.z),1);
				float MaxColWidth = 0;

				
				float val = 0;
				float iterator = 0;
				for(iterator = 0; iterator < 100; iterator++)
				{
					if(_EmitData[iterator].x == 0)
						continue;
					float width = _EmitData[iterator].w * _WidthScale;
					float speed = _EmitData[iterator].y;
					float FallOffStartDistance = 0;
					float FallOffEndDistance = _EmitData[iterator].z;
					float dis = distance(_EmitLocations[iterator] , i.worldPos);

					float fallOffScale = 1;
					if(dis > FallOffStartDistance)
						fallOffScale = ((FallOffEndDistance - FallOffStartDistance)- (dis - FallOffStartDistance))/
						(FallOffEndDistance - FallOffStartDistance);
				
					float maxdis = ((_Time.y - _EmitData[iterator].x) * speed);
					float fademaxdis = maxdis - MaxColWidth;
					float fademindis = fademaxdis - width;
				

					float tempval = 0;
					if(dis < maxdis && dis >= fademaxdis)
						tempval =  1;
					else if(dis < fademaxdis && dis > fademindis)
						tempval =  1 - ((width - (dis - fademindis))/width);
					tempval = tempval * fallOffScale;
					if(tempval < 0)
						tempval = 0;

					val = val + tempval;
				}
				
				//if(_Discard0 == 1.0 && val < 0.01)
				//	discard;
				return float4(val,val,val,1);
			}
            ENDCG
         }
         Pass 
         {
             Stencil {
                 Ref 0
                 Comp equal
             }
             CGPROGRAM
             #include "UnityCG.cginc"
             #pragma target 4.0
             #pragma vertex vert
             #pragma geometry geom
             #pragma fragment frag
             
             
             half4 _Color;
             float _Thickness;
         
             struct v2g 
             {
                 float4 pos : SV_POSITION;
                 float2 uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
             };
             
             struct g2f 
             {
                 float4 pos : SV_POSITION;
                 float2 uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
             };
 
             v2g vert(appdata_base v)
             {
                 v2g OUT;
                 OUT.pos = UnityObjectToClipPos(v.vertex);
                 
                 OUT.uv = v.texcoord;
                  OUT.normals = v.normal;
                 OUT.viewT = ObjSpaceViewDir(v.vertex);
                 
                 return OUT;
             }
             
             void geom2(v2g start, v2g end, inout TriangleStream<g2f> triStream)
             {
                 float thisWidth = _Thickness/100;
                 float4 parallel = end.pos-start.pos;
                 normalize(parallel);
                 parallel *= thisWidth;
                 
                 float4 perpendicular = float4(parallel.y,-parallel.x, 0, 0);
                 perpendicular = normalize(perpendicular) * thisWidth;
                 float4 v1 = start.pos-parallel;
                 float4 v2 = end.pos+parallel;
                 g2f OUT;
                 OUT.pos = v1-perpendicular;
                 OUT.uv = start.uv;
                 OUT.viewT = start.viewT;
                 OUT.normals = start.normals;
                 triStream.Append(OUT);
                 
                 OUT.pos = v1+perpendicular;
                 triStream.Append(OUT);
                 
                 OUT.pos = v2-perpendicular;
                 OUT.uv = end.uv;
                 OUT.viewT = end.viewT;
                 OUT.normals = end.normals;
                 triStream.Append(OUT);
                 
                 OUT.pos = v2+perpendicular;
                 OUT.uv = end.uv;
                 OUT.viewT = end.viewT;
                 OUT.normals = end.normals;
                 triStream.Append(OUT);
             }
             
             [maxvertexcount(12)]
             void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
             {
                 geom2(IN[0],IN[1],triStream);
                 geom2(IN[1],IN[2],triStream);
                 geom2(IN[2],IN[0],triStream);
             }
             
             half4 frag(g2f IN) : COLOR
             {
                 _Color.a = 1;
                 return _Color;
             }
             
             ENDCG
 
         }
     }
     //FallBack "Diffuse"
 }