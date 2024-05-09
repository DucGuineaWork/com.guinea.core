using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Guinea.Core
{
    public class AsyncOperationManager : MonoBehaviour
    {
        private static List<UniTask> s_tasks = new List<UniTask>();
        private static List<string> s_taskNames = new List<string>();
        private static int s_completedTaskCounter = 0;
        private static bool s_busy = false;
        private static Indicator s_indicator;

        private static MonoBehaviour s_routineRunner;

        public static event Action<Indicator> OnProgress = delegate { };

        #region Unity Callbacks
        void Awake()
        {
            s_routineRunner = this;
        }
        #endregion

        public static void AddTask(AsyncOperation asyncOperation, CancellationToken cancellationToken = default, Action continuation=null, string taskName = null)
        {
            UniTask task; 
            if(continuation != null)
            {
                task = asyncOperation.ToUniTask(cancellationToken: cancellationToken).ContinueWith(Increment).ContinueWith(continuation);
            }
            else
            {
                task = asyncOperation.ToUniTask(cancellationToken: cancellationToken).ContinueWith(Increment);
            }
            s_tasks.Add(task);
            s_taskNames.Add(taskName);
            Logger.Log($"AddTask {asyncOperation}({taskName})");
        }

        public static void AddTask(UniTask task, string taskName = null)
        {
            UniTask modified_task = task.ContinueWith(Increment);
            s_tasks.Add(modified_task);
            s_taskNames.Add(taskName);
            Logger.Log($"AddTask {task}({taskName})");
        }
        private static void Increment()
        {
            s_completedTaskCounter++;
        }

        public static UniTask GetTask()
        {
            if (s_busy)
            {
                return UniTask.WaitUntil(() => !s_busy && s_tasks.Count == 0);
            }

            s_routineRunner.StartCoroutine(UpdateProgressCoroutine());
            return UniTask.WhenAll(s_tasks).ContinueWith(Clear);
        }

        private static IEnumerator UpdateProgressCoroutine()
        {
            s_indicator = Indicator.Empty;
            OnProgress(s_indicator);
            float progress = 0f;
            while (true)
            {
                if (!s_busy)
                {
                    break;
                }

                int currentTaskIndex = Mathf.Clamp(s_completedTaskCounter, 0, s_taskNames.Count - 1);
                int completedTaskCounter = Mathf.Min(s_completedTaskCounter, s_tasks.Count);
                progress = completedTaskCounter / (float)s_tasks.Count;

                s_indicator.Report(progress, $"{s_taskNames[currentTaskIndex]}({completedTaskCounter}/{s_tasks.Count})");
                OnProgress(s_indicator);
                yield return null;
            }

            s_indicator.Report(1.0f, "Complete"); // * Workaround when all tasks completed before while loop
            OnProgress(s_indicator);
        }

        private static void Clear()
        {
            s_tasks.Clear();
            s_taskNames.Clear();
            s_completedTaskCounter = 0;
            s_busy = false;
        }

        public struct Indicator
        {
            public float progress;
            public string task;

            public static readonly Indicator Empty = new Indicator { progress = -1f };

            public void Report(float progress, string task = null)
            {
                this.progress = progress;
                this.task = task;
            }
        }
    }
}