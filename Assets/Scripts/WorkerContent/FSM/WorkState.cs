using Enums;

namespace WorkerContent.FSM
{
    public class WorkState : WorkerState
    {
        public override void Enter(Worker worker)
        {
            worker.WorkerTimer.Init(WorkerStateType.Work);
            worker.StartWorking();
        }

        public override void Update(Worker worker)
        {
            worker.WorkerTimer.UpdateViewInfo(WorkerStateType.Work);
            
            if (worker.WorkerTimer.StateTimer <= 0)
                worker.SetState(new RelaxState());
        }

        public override void Exit(Worker worker)
        {
        }
    }
}