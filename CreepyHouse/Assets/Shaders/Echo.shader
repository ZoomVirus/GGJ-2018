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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
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
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

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
				if(_Discard0 == 1.0 && val < 0.01)
					discard;
				return float4(val,val,val,val);
			}
			ENDCG
		}

	}
}
