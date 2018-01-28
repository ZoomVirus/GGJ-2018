Shader "Custom/Echo"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_MyVector ("Some Vector", Vector) = (0,0,0,0) 
		_WidthScale ("WidthScale", float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
			};
			//Globals
			float4 _EmitLocations[100];
			float4 _EmitData[100];

			//uniforms
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _WidthScale;
			float _Discard0;

			//x: Time
			//y: Speed
			//z: FallOff
			//w: Width
			//float _EmitTimes[100];
			//float _EmitFallOffs[100];
			//float _EmitSpeeds[100];
			//float _EmitWidths[100];
			
			v2f vert (appdata v)
			{
				v2f o;
				
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				if(_Discard0 < 1)
					return float4(abs(i.normal.x), abs(i.normal.y),abs(i.normal.z),1);
					
				float MaxColWidth = 0;

				
				float val = 0;
				float iterator = 0;
				float ran = 0;

				ran += tan(frac((i.worldPos.y + 0.0115234)*52112.452+ (i.worldPos.z + 0.347518)*34321.52127 + frac(_Time.y) * 14320.23451));
				ran += tan(frac((i.worldPos.x + 0.124112)*12324.1309+(i.worldPos.y + 0.123689)*54221.21231 + frac(_Time.y) * 35134.12342));
				ran += tan(frac((i.worldPos.z + 0.129397)*823591.1378+(i.worldPos.x + 0.348761)*14231.12435 + frac(_Time.y) * 723419.25121));
				ran = frac(ran);




				for(iterator = 0; iterator < 100; iterator++)
				{

					if(_EmitData[iterator].x == 0)
						continue;
					float width = _EmitData[iterator].w * _WidthScale;
					float speed = _EmitData[iterator].y;
					float FallOffStartDistance = 0;
					float FallOffEndDistance = _EmitData[iterator].z;
					float dis = distance(_EmitLocations[iterator] , i.worldPos);
					dis = lerp(dis-0.2,dis+0.2, ran);

					float fallOffScale = 1;
					if(dis> FallOffStartDistance)
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
					tempval = tempval * fallOffScale ;
					if(tempval < 0)
						tempval = 0;

					val = val + lerp(tempval*0.6,tempval , ran);
				}
				//if(/*_Discard0 == 1.0 &&*/ val < 0.01)
				//	discard;
				return float4(val,val,val,val);
			}
			ENDCG
		}

	}
}
