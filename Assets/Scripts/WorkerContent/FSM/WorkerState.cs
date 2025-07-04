namespace WorkerContent.FSM
{
    public abstract class WorkerState
    {
        public abstract void Enter(Worker worker);
        public abstract void Update(Worker worker);
        public abstract void Exit(Worker worker);
    }
}