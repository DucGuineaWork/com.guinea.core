using System;
using System.Collections;
using UnityEngine;

namespace Guinea.Core
{
    public class TimerManager: MonoBehaviour
{
    [Tooltip("Elapsed of a tick in second")][SerializeField]float m_tick;

    private WaitForSeconds m_wait;
    
    public static event Action OnTick;
    public static event Action<int> OnApplicationResumed;
    public static event Action OnApplicationPaused;

    private DateTime m_lastPaused = DateTime.MinValue;

    void Start()
    {
        m_wait = new WaitForSeconds(m_tick);
        StartCoroutine(RunTickCoroutine());
    }

    void OnApplicationPause(bool paused)
    {
        if(paused)
        {
            m_lastPaused = DateTime.UtcNow;
            OnApplicationPaused?.Invoke();
        }
        else 
        {
            if(m_lastPaused != DateTime.MinValue)
            {
                TimeSpan timeSpan = DateTime.UtcNow - m_lastPaused;
                OnApplicationResumed?.Invoke((int)(timeSpan.TotalSeconds/m_tick));
            }
        }
    }

    private IEnumerator RunTickCoroutine()
    {
        while(true)
        {
            OnTick?.Invoke();
            yield return m_wait;
        }
    }

    public class Timer
    {
        private int m_tick;
        private int m_startTick;
        private int m_tickLength;
        private int m_lastTick;
        public int Tick => m_tick;
        public int StartTick => m_startTick;
        public int TickLength=>m_tickLength;
        public event Action<Timer> OnTickChanged;
        public event Action<Timer> OnTimerEnd;

        public Timer(int startTick=0, int tickLength=1)
        {
            m_startTick = startTick;
            m_tickLength = tickLength;
        }

        public void SetStartTick(int startTick)
        {
            m_startTick = startTick;
        }

        private void OnTick()
        {
            m_tick += Math.Sign(m_tickLength);
            if(m_tick < 0)
            {
                m_tick = 0;
                GoldMiner_TimerManager.OnTick -= OnTick;
                OnTickChanged?.Invoke(this);
                OnTimerEnd?.Invoke(this);
                return;
            }

            if(Mathf.Abs(m_tick-m_lastTick) >= Mathf.Abs(m_tickLength))
            {
                OnTickChanged?.Invoke(this);
                m_lastTick = m_tick;
            }
        }

        private void OnApplicationResumed(int ticks)
        {
            m_tick += ticks * Math.Sign(m_tickLength);
            if(m_tick < 0)
            {
                m_tick = 0;
                GoldMiner_TimerManager.OnTick -= OnTick;
                OnTickChanged?.Invoke(this);
                OnTimerEnd?.Invoke(this);
                return;
            }

            m_lastTick = m_tick;
            TimerManager.OnTick -= OnTick;
            TimerManager.OnTick += OnTick;
        }

        private void OnApplicationPaused()
        {
            TimerManager.OnTick -= OnTick;
            TimerManager.OnApplicationResumed -= OnApplicationResumed;
            TimerManager.OnApplicationResumed += OnApplicationResumed;
        }

        public void Run()
        {
            TimerManager.OnTick -= OnTick ;
            TimerManager.OnTick += OnTick;
            TimerManager.OnApplicationPaused -= OnApplicationPaused;
            TimerManager.OnApplicationPaused += OnApplicationPaused;
            m_tick = m_startTick;
            m_lastTick = m_tick;
            OnTickChanged?.Invoke(this);
        }

        public void Stop()
        {
            TimerManager.OnTick -=OnTick;
            TimerManager.OnApplicationResumed -= OnApplicationResumed;
            m_tick = m_startTick;
        }

        public void Pause()
        {
            TimerManager.OnTick -=OnTick;
            TimerManager.OnApplicationResumed -= OnApplicationResumed;
        }

        public void Resume()
        {
            TimerManager.OnTick -= OnTick ;
            TimerManager.OnTick += OnTick;
            TimerManager.OnApplicationPaused -= OnApplicationPaused;
            TimerManager.OnApplicationPaused += OnApplicationPaused;
        }
    }
}
}