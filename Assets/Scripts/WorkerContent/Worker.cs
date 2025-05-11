using System;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace WorkerContent
{
    public abstract class Worker : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
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
        
        
        
        [SerializeField]private Transform _relaxPosition;
        
        protected WorkerState WorkerState;
        private float _elapsedTime;

        public WorkerType WorkerType => _workerType;

        private void Update()
        {
            
        }

        public void Activate()
        {
            WorkerState = WorkerState.Work;
            transform.position = _relaxPosition.position;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public abstract void Work();
    }
}