Shader "Gambale/HorseShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Speed("Speed", Range(0, 1.1)) = 0
	}

		SubShader
		{
			Cull Off
			Blend One OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM

				#pragma vertex vertexFunc
				#pragma fragment fragmentFunc
				#include "UnityCG.cginc"

				sampler2D _MainTex;

				struct v2f
				{
					float4 pos : SV_POSITION;
					half2 uv : TEXCOORD0;
				};

				v2f vertexFunc(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;
					return o;
				}

				float _Speed;
				fixed4 _Color;
				float4 _MainTex_TexelSize;
				float timeInRainbowMode;

				fixed4 fragmentFunc(v2f i) : COLOR
				{
					half4 c = tex2D(_MainTex, i.uv);
					c.rgb *= c.a;
					if (c.a >= .1)
					{
						c.rgb = float3(0, 0, 0);
					}

					half4 zoomColor = half4(0, 0, 0, 1);
					//if (_Speed > 1.0)
					//{
					//	// start rainbow mode
					//	timeInRainboMode += unity_DeltaTime;
					//	zoomColor
					//}

					zoomColor.r = lerp(0, 1, _Speed);
					zoomColor.g = lerp(0, .8, _Speed * _Speed);
					zoomColor.b = lerp(0, 1, _Speed * 2);
					half4 outlineC = zoomColor;
					outlineC.a *= ceil(c.a);
					outlineC.rgb *= outlineC.a;

					fixed upAlpha = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
					fixed downAlpha = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
					fixed rightAlpha = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
					fixed leftAlpha = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;

					fixed upAlpha2 = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * 2)).a;
					fixed downAlpha2 = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * 2)).a;
					fixed rightAlpha2 = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * 2, 0)).a;
					fixed leftAlpha2 = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * 2, 0)).a;

					return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha /** upAlpha2 * downAlpha2 * rightAlpha2 * leftAlpha2*/));
				}

				ENDCG
			}
		}
			Fallback "Diffuse"
}
