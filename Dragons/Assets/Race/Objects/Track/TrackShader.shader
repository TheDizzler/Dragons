Shader "Gambale/TrackShader"
{
	Properties
	{
		_MainTex("TextureImage", 2D) = "white" {}
	}

		SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vertexFunc
			#pragma fragment fragmentFunc
			#pragma target 4.0
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
			};

			v2f vertexFunc(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				float sinX = sin(1.5708);
				float cosX = cos(1.5708);
				float sinY = sin(1.5708);
				float2x2 rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);
				o.uv.xy = mul(v.texcoord.xy, rotationMatrix);
				return o;
			}

			fixed4 fragmentFunc(v2f i) : COLOR
			{
			/*sin =1
			cos = 0*/
				return tex2D(_MainTex, i.uv);
			}

			ENDCG
		}
	}
		FallBack "Diffuse"
}