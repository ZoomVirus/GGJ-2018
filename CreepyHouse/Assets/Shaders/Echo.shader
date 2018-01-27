Shader "Custom/Echo"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_MyVector ("Some Vector", Vector) = (0,0,0,0) 
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _EmitLocations[100];
			float _EmitTimes[100];
			
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

				float speed = 3;
				float MaxColWidth = 0;
				float width = 2;
				float FallOffStartDistance = 0;
				float FallOffEndDistance = 5;

				
				float val = 0;
				float iterator = 0;
				for(iterator = 0; iterator < 100; iterator++)
				{
				
					float dis = distance(_EmitLocations[iterator] , i.worldPos);

					float fallOffScale = 1;
					if(dis > FallOffStartDistance)
						fallOffScale = ((FallOffEndDistance - FallOffStartDistance)- (dis - FallOffStartDistance))/
						(FallOffEndDistance - FallOffStartDistance);
				
					float maxdis = ((_Time.y - _EmitTimes[iterator]) * speed);
					float fademaxdis = maxdis - MaxColWidth;
					float fademindis = fademaxdis - width;
				

					float tempval = 0;
					if(dis < maxdis && dis >= fademaxdis)
						tempval =  1;
					else if(dis < fademaxdis && dis > fademindis)
						tempval =  1 - ((width - (dis - fademindis))/width);
					tempval = tempval * fallOffScale;
					val = val + tempval;
				}
				return float4(val,val,val,val);
			}
			ENDCG
		}
	}
}
