using System;
using Enums;
using UnityEngine;

namespace WorkerContent
{
    [RequireComponent(typeof(Worker), typeof(WorkerTimerViewer))]
    public class WorkerTimer : MonoBehaviour
    {
        [SerializeField] private Worker _worker;
        [SerializeField] private WorkerTimerViewer _workerTimerViewer;

        private float _delayRelax;
        private float _delayWork;

        public float StateTimer { get; private set; }

        public void SetTimeWork()
        {
            _delayWork = _worker.WorkerParametersConfig.GetConfig(_worker.WorkerType,_worker.Level).DelayWork;
            StateTimer = _delayWork;
        }

        public void SetStateRelax()
        {
            _delayRelax = _worker.WorkerParametersConfig.GetConfig(_worker.WorkerType,_worker.Level).DelayRelax;
            StateTimer = _delayRelax;
        }

        public void WakeUpWorker()
        {
            StateTimer = 0;
        }

        public void Init(WorkerStateType workerStateType)
        {
            switch (workerStateType)
            {
                case WorkerStateType.Work:
                    SetTimeWork();
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Work, _delayWork);
                    break;
                case WorkerStateType.Relax:
                    SetStateRelax();
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Relax, _delayRelax);
                    break;
            }
        }

        public void UpdateViewInfo(WorkerStateType workerStateType)
        {
            switch (workerStateType)
            {
                case WorkerStateType.Work:
                    StateTimer -= Time.deltaTime;
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Work, _delayWork);
                    Debug.Log("WorkerStateType.Work:");
                    break;
                case WorkerStateType.Relax:
                    StateTimer -= Time.deltaTime;
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Relax, _delayRelax);
                    Debug.Log("WorkerStateType.Relax:");
                    break;
            }
        }
    }
}