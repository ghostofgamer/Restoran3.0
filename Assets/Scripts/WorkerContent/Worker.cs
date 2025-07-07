using System;
using Enums;
using UnityEngine;
using WorkerContent.FSM;

namespace WorkerContent
{
    public abstract class Worker : MonoBehaviour
    {
        [SerializeField] private WorkerMover _workerMover;
        [SerializeField] private WorkerType _workerType;
        [SerializeField] protected Transform RelaxPosition;
        [SerializeField] private WorkerTimer _workerTimer;
        [SerializeField] private WorkerAnimation _workerAnimation;
        
        protected WorkerState CurrentState;

        public WorkerTimer WorkerTimer => _workerTimer;

        public WorkerType WorkerType => _workerType;

        public WorkerMover WorkerMover => _workerMover;

        public WorkerAnimation WorkerAnimation => _workerAnimation;

        public WorkerStateType CurrentWorkerStateType { get; private set; }

        public bool IsTired { get; private set; } = false;

        private void Update()
        {
            CurrentState?.Update(this);
        }

        public abstract bool GetConditionsWorkUpdate();

        public abstract bool GetConditionsRelaxUpdate();

        public void Deactivate() => gameObject.SetActive(false);

        public virtual void Activate()
        {
            _workerTimer.SetTimeWork();
            transform.position = RelaxPosition.position;
            gameObject.SetActive(true);
            IsTired = false;
            SetState(new WorkState());
        }

        public virtual void SetState(WorkerState newState)
        {
            CurrentState?.Exit(this);
            CurrentState = newState;
            CurrentState.Enter(this,null);
        }

        public void SetValueTired(bool value)
        {
            IsTired = value;
        }

        public void SetWorkerStateType(WorkerStateType workerStateType)
        {
            CurrentWorkerStateType = workerStateType;
        }

        public virtual void StartWorking()
        {
        }

        public virtual void StartRelaxing(Action action)
        {
        }
    }
}