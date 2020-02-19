Shader "Gambale/HorseShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_RainbowTex("RainbowTexture", 2D) = "white" {}
		[HideInInspector] _Speed("Speed", Range(0, 1.1)) = 0
		_TimeInRainbow("TimeInRainbow", Float) = 0
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
				#pragma target 4.0
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _RainbowTex;

				struct v2f
				{
					float4 pos : SV_POSITION;
					half4 uv : TEXCOORD0;
					float4 vertex : TEXCOORD2;
				};

				v2f vertexFunc(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;
					o.vertex = v.vertex;
					return o;
				}

				float _Speed;
				float3 _StartColor;
				float3 _EndColor;
				float4 _MainTex_TexelSize;
				float _TimeInRainbow;
				float _CycleSpeed;

				fixed4 fragmentFunc(v2f i) : COLOR
				{
					half4 c = tex2D(_MainTex, i.uv);
					c.rgb *= c.a;
					if (_Speed > 1.0)
					{
						// start rainbow mode
						half2 rainpos = i.vertex.xy;
						rainpos.x += (_TimeInRainbow * _Speed * 2);
						rainpos.y += (_TimeInRainbow * _Speed * .125);
						c.rgb = tex2D(_RainbowTex, rainpos).rgb;
					}
					else if (c.a >= .1)
					{
						c.rgb = float3(0, 0, 0);
					}




					half4 zoomColor = half4(0, 0, 0, 1);
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

					return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha * upAlpha2 * downAlpha2 * rightAlpha2 * leftAlpha2));
				}

				ENDCG
			}
		}
			Fallback "Diffuse"
}
