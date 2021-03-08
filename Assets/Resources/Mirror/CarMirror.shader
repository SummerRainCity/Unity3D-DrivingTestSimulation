Shader "TTGO/CarLeftMirror"
{
   Properties
   {
   _MainTex ("Main Texture", 2D) = "white" {}
      _Mask ("Culling Mask", 2D) = "white" {}
   }
   SubShader
   {
      Tags {"Queue" = "Transparent"}
      //Blend SrcAlpha OneMinusSrcAlpha
      Lighting On
      ZWrite Off
     // ZTest Always
     // Alphatest LEqual 0
     Blend SrcAlpha OneMinusSrcAlpha
      Pass
      {
         SetTexture [_Mask] {combine texture}
         SetTexture [_MainTex] {combine texture, previous}
      }
   }
}