using Enums;
using UnityEngine;

namespace WorkerContent.FSM.CleanerFSM
{
    public class CleanerRelaxState : RelaxState
    {
        private Cleaner _cleaner;

        public override void Enter(Worker worker)
        {
            _cleaner = worker.GetComponent<Cleaner>();
            worker.WorkerTimer.Init(WorkerStateType.Relax);
        }

        public override void Update(Worker worker)
        {
            if (_cleaner.CurrentDirtyTable == null && worker.IsTired &&
                _cleaner.CurrentWorkerStateType == WorkerStateType.Relax)
            {
                worker.WorkerTimer.UpdateViewInfo(WorkerStateType.Relax);

                if (worker.WorkerTimer.StateTimer <= 0)
                {
                    worker.SetValueTired(false);
                    _cleaner.SetWorkerStateType(WorkerStateType.Work);
                    worker.SetState(new CleanerWorkState());
                }
            }
        }

        public override void Exit(Worker worker)
        {
        }
    }
}