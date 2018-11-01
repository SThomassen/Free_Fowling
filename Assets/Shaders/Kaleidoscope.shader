//used most of my own math, verified and adjusted where necessary based on KinoMirror.
Shader "Kaleidoscope"
{
	Properties
	{
		_MainTex("-", 2D) = "" {}
	}
		CGINCLUDE

#include "UnityCG.cginc"

#pragma multi_compile _ SYMMETRY_ON

	sampler2D _MainTex;
	float _Divisor;
	float _Offset;//what the 'start' is of the triangle segment
	float _Roll;//what the 'end' is of the triangle segment

	half4 frag(v2f_img i) : SV_Target
	{
		// Convert to the polar coordinate.
		float2 sc = i.uv - 0.5;
		float phi = atan2(sc.y, sc.x);//angle between screencoord
		float r = sqrt(dot(sc, sc));//needed to reflect later, and actual polar coord

		// Angular repeating.
		phi += _Offset;
		phi = phi - _Divisor * floor(phi / _Divisor);
		#if SYMMETRY_ON
		phi = min(phi, _Divisor - phi);
		#endif
		phi += _Roll - _Offset;

		// Convert back to the texture coordinate.
		float2 uv = float2(cos(phi), sin(phi)) * r + 0.5;

		// Reflection at the border of the screen.
		uv = max(min(uv, 2.0 - uv), -uv);

		return tex2D(_MainTex, uv);
	}

	ENDCG
	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			ENDCG
		}
	}
}
