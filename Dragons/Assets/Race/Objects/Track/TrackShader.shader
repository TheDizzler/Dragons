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