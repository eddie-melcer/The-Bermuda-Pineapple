Shader "Space/WaterFoam"
{
	Properties
	{
		_Radius("Radius", float) = 5
		_FallOff("FallOff", float) = 5
		_Exponent("Exponent", float) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
		BlendOp Add
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float3 worldPos : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float _Radius;
			float _FallOff;
			float _Exponent;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float ring = sqrt(i.worldPos.x * i.worldPos.x + i.worldPos.z * i.worldPos.z);
				float diff = pow(clamp(abs(ring - _Radius) / _FallOff,0,1), _Exponent);
				return fixed4(1,1,1,1 - diff);
			}
			ENDCG
		}
	}
}
