using System;
using TMPro;
using UnityEngine;

namespace Guinea.Core
{
    public class FPSCounter : MonoBehaviour 
    {
        [SerializeField]TextMeshProUGUI m_fpsText;
        [SerializeField]TextMeshProUGUI m_fpsMinText;
        [SerializeField]TextMeshProUGUI m_fpsMaxText;
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        private int m_minFps = Int32.MaxValue;
        private int m_maxFps = Int32.MinValue;

        private void Start() 
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        }

        private void Update() 
        {
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup> m_FpsNextPeriod) 
            {
                m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
                m_minFps = Mathf.Min(m_minFps, m_CurrentFps);
                m_maxFps = Mathf.Max(m_maxFps, m_CurrentFps);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_fpsText.text = $"FPS: {m_CurrentFps}";
                m_fpsMinText.text = $"Min: {m_minFps}";
                m_fpsMaxText.text = $"Max: {m_maxFps}";
            }
        }
    }
}