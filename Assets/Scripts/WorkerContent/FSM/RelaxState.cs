using Enums;

namespace WorkerContent.FSM
{
    public class RelaxState : WorkerState
    {
        private Cleaner _cleaner;

        public override void Enter(Worker worker)
        {
            worker.WorkerTimer.Init(WorkerStateType.Relax);
            worker.StartRelaxing(null);
        }

        public override void Update(Worker worker)
        {
            worker.WorkerTimer.UpdateViewInfo(WorkerStateType.Relax);
            
            if (worker.WorkerTimer.StateTimer <= 0)
                worker.SetState(new WorkState());
        }

        public override void Exit(Worker worker)
        {
        }
    }
}