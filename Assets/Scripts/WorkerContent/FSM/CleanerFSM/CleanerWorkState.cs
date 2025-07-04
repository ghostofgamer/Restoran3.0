using Enums;
using UnityEngine;

namespace WorkerContent.FSM.CleanerFSM
{
    public class CleanerWorkState : WorkState
    {
        private Cleaner _cleaner;

        public override void Enter(Worker worker)
        {
            base.Enter(worker);
            _cleaner = worker.GetComponent<Cleaner>();

            if (_cleaner != null)
                Debug.Log("УБОРЩИК ИНИЦИАЛИЗИРОВАН ТУТ");
        }

        public override void Update(Worker worker)
        {
            if (_cleaner.CurrentDirtyTable != null && _cleaner.CurrentWorkerStateType == WorkerStateType.Work)
            {
                worker.WorkerTimer.UpdateViewInfo(WorkerStateType.Work);

                if (worker.WorkerTimer.StateTimer <= 0)
                {
                    _cleaner.SetWorkerStateType(WorkerStateType.Relax);
                    worker.SetState(new CleanerRelaxState());
                }
            }
        }

        public override void Exit(Worker worker)
        {
            base.Exit(worker);
        }
    }
}