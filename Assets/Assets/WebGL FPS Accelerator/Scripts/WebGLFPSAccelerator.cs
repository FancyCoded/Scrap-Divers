using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Rendering;

#if USING_URP
using UnityEngine.Rendering.Universal;
#endif

namespace AG_WebGLFPSAccelerator
{
    public class WebGLFPSAccelerator : MonoBehaviour
    {

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern float getDefaultDPR();

    [DllImport("__Internal")]
    private static extern void _setDPR(float float1);
#endif

        public static WebGLFPSAccelerator instance;

        [Header("___SETTINGS____________________________________________________________________________________________________________________")]
        [Read_Only_Field]
        public float dpi = 1;
        public bool dynamicResolutionSystem;
        public float dpiDecrease = 0.050f;
        public float dpiIncrease = 0.050f;
        public int fpsMax = 35;
        public int fpsMin = 30;
        public float dpiMin = 0.6f;
        public float dpiMax = 1f;
        public float measurePeriod = 4f;
        public float fixedDPI = 1f;
        public bool useRenderScaleURP;

        [HideInInspector]
        public int fps;

        [HideInInspector]
        public float dpr = 0;

        [HideInInspector]
        private int m_FpsAccumulator = 0;

        [HideInInspector]
        public float m_FpsNextPeriod = 1.5f;

        //[HideInInspector]
        public float defaultDPR = 0f;

        [HideInInspector]
        public float lastDPR;

        [HideInInspector]
        public bool lastDynamicResolutionSystem;

        [HideInInspector]
        public bool lastShowHideUI;

        [HideInInspector]
        public bool urp;

#if USING_URP
        private UniversalRenderPipelineAsset urpAsset;
#endif

        private bool wait = true;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            requestDefaultDPR();

            Invoke("waitForOneSecond", 1);

#if USING_URP
            var rpAsset = GraphicsSettings.renderPipelineAsset;
            urpAsset = (UniversalRenderPipelineAsset)rpAsset;

            urp = true;
#endif
        }

        public void __setDPR(float float1)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        _setDPR(float1);
#endif
        }

        public void requestDefaultDPR()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        defaultDPR = getDefaultDPR();
#endif
        }

        public void Toggle1Event()
        {

        }

        public void Toggle2Event()
        {

        }

        public void getAverageFPS()
        {
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup >= m_FpsNextPeriod)
            {
                fps = (int)(m_FpsAccumulator / measurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod = Time.realtimeSinceStartup + measurePeriod;

                dynamicResolutionSystemMethod();
            }
        }

        public void dynamicResolutionSystemMethod()
        {
            if (fps > fpsMax)
            {
                dpi += dpiIncrease;
            }
            else if (fps < fpsMin)
            {
                dpi -= dpiDecrease;
            }
            dpi = Mathf.Clamp(dpi, dpiMin, dpiMax);

            if (useRenderScaleURP && urp)
            {
                dpr = dpi;

                if (dpr != lastDPR)
                {
#if USING_URP
                    urpAsset.renderScale = dpr;
#endif
                }
            }
            else
            {
                dpr = dpi * defaultDPR;

                if (dpr != lastDPR)
                {
                    __setDPR(dpr);
                }
            }

            lastDPR = dpr;
        }

        void Update()
        {
            if (defaultDPR != 0 && !wait)
            {
                if (!dynamicResolutionSystem)
                {
                    if (dynamicResolutionSystem != lastDynamicResolutionSystem || dpr == 0)
                    {
                        lastDynamicResolutionSystem = dynamicResolutionSystem;
                        lastDPR = 0;
                    }

                    dpi = fixedDPI;

                    if (useRenderScaleURP && urp)
                    {
                        dpr = dpi;

#if USING_URP
                        if (dpr != lastDPR)
                            urpAsset.renderScale = dpr;
#endif
                    }
                    else
                    {
                        dpr = dpi * defaultDPR;

                        if (dpr != lastDPR)
                        {
                            __setDPR(dpr);
                        }
                    }

                    lastDPR = dpr;

                }
                else
                {
                    if (dynamicResolutionSystem != lastDynamicResolutionSystem || dpr == 0)
                    {
                        lastDynamicResolutionSystem = dynamicResolutionSystem;
                        m_FpsNextPeriod = Time.realtimeSinceStartup + measurePeriod;
                        m_FpsAccumulator = 0;

                        lastDPR = 0;
                    }

                    getAverageFPS();
                }
            }

            dpi = (float)Math.Round(dpi * 100f) / 100f;

        }
        
        public void waitForOneSecond()
        {
            wait = false;
        }
    }
}
