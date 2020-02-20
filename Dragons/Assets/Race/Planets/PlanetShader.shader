Shader "Gambale/PlanetShader"
{
	Properties
	{
		_MainTex("Texture Image", 2D) = "white" {}
	}

		SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
			};

			v2f vert(vertexInput input)
			{
				v2f output;

				output.tex = input.uv;
				output.pos = UnityObjectToClipPos(input.vertex);
				return output;
			}

			float4 frag(v2f input) : COLOR
			{
				return tex2D(_MainTex, input.tex.xy);
			}

			ENDCG
		}
	}

	Fallback "Unlit/Texture"
}
