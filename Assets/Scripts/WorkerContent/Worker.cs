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
        [SerializeField] private float _stoppingDistance = 1f;
        [SerializeField] private float _delayWork;
        [SerializeField] private float _delayRelax;
        [SerializeField] private TMP_Text _timerViewText;
        [SerializeField] private Sprite _workSprite;
        [SerializeField] private Sprite _relaxSprite;
        [SerializeField] private Image _workStateImage;
        [SerializeField] private Image _radialFillImage;
        [SerializeField] private WorkerType _workerType;

        [SerializeField] protected Transform RelaxPosition;

        protected WorkerState WorkerState;
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

        public abstract void Work();
    }
}