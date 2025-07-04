using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

namespace WorkerContent
{
    public abstract class Worker : MonoBehaviour
    {
        [SerializeField] protected WorkerTimerViewer WorkerTimerViewer;
        [SerializeField] protected Animator Animator;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _delayWork;
        [SerializeField] private float _delayRelax;
        [SerializeField] private WorkerType _workerType;
        protected bool IsRelaxing;
        [SerializeField] protected Transform RelaxPosition;

        protected WorkerStateType WorkerStateType;
        protected float ElapsedTime;

        public float DelayRelax => _delayRelax;
        public float DelayWork => _delayWork;
        public WorkerType WorkerType => _workerType;
        public NavMeshAgent Agent => _agent;

        public virtual void Activate()
        {
            ElapsedTime = _delayWork;
            transform.position = RelaxPosition.position;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public void Working()
        {
            ElapsedTime -= Time.deltaTime;
            WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerStateType.Work,DelayWork);

            if (ElapsedTime <= 0)
            {
                WorkerStateType = WorkerStateType.Relax;
                ElapsedTime = DelayRelax;
                WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerStateType.Relax,DelayRelax);
            }
        }

        public void Relaxing()
        {
            ElapsedTime -= Time.deltaTime;
            WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerStateType.Relax,DelayRelax);

            if (ElapsedTime <= 0)
            {
                Debug.Log("relax 3");
                IsRelaxing = false;
                WorkerStateType = WorkerStateType.Work;
                ElapsedTime = DelayWork;
                Work();
                WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerStateType.Work,DelayWork);
            }
        }

        public abstract void Work();
    }
}