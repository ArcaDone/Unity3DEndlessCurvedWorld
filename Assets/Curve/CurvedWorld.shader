// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/CurvedWorld" {
     Properties {
         _Color ("Color", Color) = (1,1,1,1)
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
         _Glossiness ("Smoothness", Range(0,1)) = 0.5
         _Metallic ("Metallic", Range(0,1)) = 0.0
         _Curvature("Curvature", Float) = 0
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200

         CGPROGRAM
         // Physically based Standard lighting model, and enable shadows on all light types
         #pragma surface surf Standard vertex:vert fullforwardshadows
 
         // Use shader model 3.0 target, to get nicer looking lighting
         #pragma target 3.0
         sampler2D _MainTex;
         uniform float _Curvature;
 
         half _Glossiness;
         half _Metallic;
         fixed4 _Color;
 
 
         struct Input {
             float2 uv_MainTex;
         };
 
         // This is where the curvature is applied
         void vert( inout appdata_full v)
         {
             // Transform the vertex coordinates from model space into world space
             float4 vv = mul( unity_ObjectToWorld, v.vertex );
  
             // Now adjust the coordinates to be relative to the camera position
             vv.xyz -= _WorldSpaceCameraPos.xyz;
  
             // Reduce the y coordinate (i.e. lower the "height") of each vertex based
             // on the square of the distance from the camera in the z axis, multiplied
             // by the chosen curvature factor
             vv = float4( 0.0f, (vv.z * vv.z) * - _Curvature, 0.0f, 0.0f );
  
             // Now apply the offset back to the vertices in model space
             v.vertex += mul(unity_WorldToObject, vv);
         }
 
 
         // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
         // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
         // #pragma instancing_options assumeuniformscaling
         UNITY_INSTANCING_BUFFER_START(Props)
             // put more per-instance properties here
         UNITY_INSTANCING_BUFFER_END(Props)
 
         void surf (Input IN, inout SurfaceOutputStandard o) {
             // Albedo comes from a texture tinted by color
             fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
             o.Albedo = c.rgb;
             // Metallic and smoothness come from slider variables
             o.Metallic = _Metallic;
             o.Smoothness = _Glossiness;
             o.Alpha = c.a;
         }
         ENDCG
     }
     FallBack "Diffuse"
 }