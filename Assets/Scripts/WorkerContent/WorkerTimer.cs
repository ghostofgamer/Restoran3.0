using Enums;
using UnityEngine;

namespace WorkerContent
{
    public class WorkerTimer : MonoBehaviour
    {
        [SerializeField] private float _delayWork;
        [SerializeField] private float _delayRelax;
        [SerializeField] private WorkerTimerViewer _workerTimerViewer;

        public float StateTimer { get; private set; }

        public void SetTimeWork()
        {
            StateTimer = _delayWork;
        }

        private void SetStateRelax()
        {
            StateTimer = _delayRelax;
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
                    break;
                case WorkerStateType.Relax:
                    StateTimer -= Time.deltaTime;
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Relax, _delayRelax);
                    break;
            }
        }
    }
}