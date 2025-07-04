using System;
using System.Collections;
using Enums;
using RestaurantContent;
using RestaurantContent.TableContent;
using UnityEngine;
using UnityEngine.AI;

namespace WorkerContent
{
    public class Cleaner : Worker
    {
        [SerializeField] private DirtyCounter _dirtyCounter;
        [SerializeField] private NavMeshObstacle _navMeshObstacle;
        
        private TableCleanliness _currentDirtyTable;
        private Coroutine _coroutine;
        private Coroutine _cleanCoroutine;

        private void Start()
        {
            Activate();
        }

        private void Update()
        {
            if (WorkerStateType == WorkerStateType.Work && _currentDirtyTable != null)
                Working();
            else if (WorkerStateType == WorkerStateType.Relax && _currentDirtyTable == null && IsRelaxing)
                Relaxing();
        }

        public override void Work()
        {
            if (WorkerStateType == WorkerStateType.Relax)
                return;

            if (_currentDirtyTable != null)
                return;

            TableCleanliness DirtyTable = _dirtyCounter.GetDirtyTable();

            if (DirtyTable != null)
            {
                _currentDirtyTable = DirtyTable;
                WorkerStateType = WorkerStateType.Work;
                Debug.Log("WorkerState " + WorkerStateType);
                SetDestination(_currentDirtyTable.CleanerPosition, _currentDirtyTable.LookDirtyPosition, StartClean);
            }
            else
            {
                return;
            }
        }

        public override void Activate()
        {
            base.Activate();

            WorkerStateType = WorkerStateType.Work;
            Work();
            WorkerTimerViewer.UpdateTimerView(DelayWork, WorkerStateType.Work,DelayWork);
        }

        private void SetDestination(Transform destination, Transform lookPositionDirty,
            Action onReachDestination = null)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(GoToDestination(destination, lookPositionDirty, onReachDestination));
        }

        private IEnumerator GoToDestination(Transform destination, Transform lookPosition,
            Action onReachDestination)
        {
            Agent.ResetPath();
            Agent.SetDestination(destination.position);
            Animator.SetBool("Walk", true);
            _navMeshObstacle.enabled = true;
            yield return null;

            Debug.Log("Agent.pathPending");
            while (Agent.pathPending)
                yield return null;
            Debug.Log("Agent.remainingDistance " + Agent.remainingDistance);
            while (Agent.remainingDistance > 0.1f)
                yield return null;

            transform.rotation = destination.rotation;
            // transform.LookAt(lookPosition);
            Animator.SetBool("Walk", false);
            _navMeshObstacle.enabled = false;
            yield return null;

            if (onReachDestination != null)
                onReachDestination?.Invoke();
        }

        private void StartClean()
        {
            if (_cleanCoroutine != null)
                StopCoroutine(_cleanCoroutine);

            _cleanCoroutine = StartCoroutine(Clean());
        }
        
        private IEnumerator Clean()
        {
            Animator.SetBool("Cleaning", true);
            yield return new WaitForSeconds(5f);
            Animator.SetBool("Cleaning", false);
            CleanTable();
        }
        
        private void CleanTable()
        {
            if (_currentDirtyTable.PollutionLevel > 0)
            {
                _currentDirtyTable.ClearTable();
                Debug.Log("Стол очищен!");
            }
            else
            {
                Debug.Log("Он и так чистый");
            }

            _currentDirtyTable = null;

            Animator.SetBool("Cleaning", false);

            if (WorkerStateType == WorkerStateType.Relax)
            {
                SetDestination(RelaxPosition, RelaxPosition, Relax);
            }
            else if (_dirtyCounter.DirtyTables.Count <= 0)
            {
                SetDestination(RelaxPosition, RelaxPosition);
            }
            else
            {
                Work();
            }
        }

        private void Relax()
        {
            WorkerStateType = WorkerStateType.Relax;
            IsRelaxing = true;
        }
    }
}