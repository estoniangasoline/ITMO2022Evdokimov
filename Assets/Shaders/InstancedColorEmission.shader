Shader "InstancedColorEmission"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard //fullforwardshadows
        
        // Use Shader model 3.0 target
        #pragma target 3.0
        sampler2D _MainTex;
        struct Input {
            float2 uv_MainTex;
        };
        UNITY_INSTANCING_BUFFER_START(Props)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _EmissionColor)
        UNITY_INSTANCING_BUFFER_END(Props)
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            fixed4 e = UNITY_ACCESS_INSTANCED_PROP(Props, _EmissionColor);
            o.Albedo = c.rgb;
            o.Emission = e.rgba;
            o.Metallic = 0.5;
            o.Smoothness = 0.5;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
