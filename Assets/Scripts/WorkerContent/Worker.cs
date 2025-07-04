using System;
using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.AI;
using WorkerContent.FSM;

namespace WorkerContent
{
    public abstract class Worker : MonoBehaviour
    {
        [SerializeField] protected Animator Animator;
        [SerializeField] protected NavMeshAgent Agent;
        [SerializeField] private WorkerType _workerType;
        [SerializeField] protected Transform RelaxPosition;
        [SerializeField] private WorkerTimer _workerTimer;

        protected WorkerState CurrentState;

        public WorkerTimer WorkerTimer => _workerTimer;

        public WorkerType WorkerType => _workerType;

        public WorkerStateType CurrentWorkerStateType { get; private set; }

        public bool IsTired { get; private set; } = false;

        public virtual void Activate()
        {
            _workerTimer.SetTimeWork();
            transform.position = RelaxPosition.position;
            gameObject.SetActive(true);
            IsTired = false;
        }

        public void Deactivate() => gameObject.SetActive(false);

        public void SetState(WorkerState newState)
        {
            CurrentState?.Exit(this);
            CurrentState = newState;
            CurrentState.Enter(this);
        }

        public void SetValueTired(bool value)
        {
            IsTired = value;
        }

        public void SetWorkerStateType(WorkerStateType workerStateType)
        {
            CurrentWorkerStateType = workerStateType;
        }

        private void Update()
        {
            CurrentState?.Update(this);
        }

        public virtual void StartWorking()
        {
        }

        public virtual void StartRelaxing(Action action)
        {
        }

        protected IEnumerator MoveToTarget(Transform target, Action onArrived)
        {
            Agent.SetDestination(target.position);
            Animator.SetBool("Walk", true);

            while (Agent.pathPending || Agent.remainingDistance > 0.1f)
                yield return null;

            transform.rotation = target.rotation;
            Animator.SetBool("Walk", false);
            onArrived?.Invoke();
        }
    }
}