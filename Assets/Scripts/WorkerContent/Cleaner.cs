using System;
using System.Collections;
using Enums;
using RestaurantContent;
using RestaurantContent.TableContent;
using UnityEngine;

namespace WorkerContent
{
    public class Cleaner : Worker
    {
        [SerializeField] private DirtyCounter _dirtyCounter;

        private TableCleanliness _currentDirtyTable;
        private Coroutine _coroutine;
        private Coroutine _cleanCoroutine;
        private bool _isRelaxing;

        private void Start()
        {
            Activate();
        }

        private void Update()
        {
            if (WorkerState == WorkerState.Work && _currentDirtyTable != null)
            {
                ElapsedTime -= Time.deltaTime;
                WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerState.Work,DelayWork);

                if (ElapsedTime <= 0)
                {
                    WorkerState = WorkerState.Relax;
                    ElapsedTime = DelayRelax;
                    WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerState.Relax,DelayRelax);
                }
            }
            else if (WorkerState == WorkerState.Relax && _currentDirtyTable == null && _isRelaxing)
            {
                ElapsedTime -= Time.deltaTime;
                WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerState.Relax,DelayRelax);

                if (ElapsedTime <= 0)
                {
                    _isRelaxing = false;
                    WorkerState = WorkerState.Work;
                    ElapsedTime = DelayWork;
                    Work();
                    WorkerTimerViewer.UpdateTimerView(ElapsedTime,WorkerState.Work,DelayWork);
                }

            }
        }

        public override void Work()
        {
            if (WorkerState == WorkerState.Relax)
                return;

            if (_currentDirtyTable != null)
                return;

            TableCleanliness DirtyTable = _dirtyCounter.GetDirtyTable();

            if (DirtyTable != null)
            {
                _currentDirtyTable = DirtyTable;
                WorkerState = WorkerState.Work;
                Debug.Log("WorkerState " + WorkerState);
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

            WorkerState = WorkerState.Work;
            Work();
            WorkerTimerViewer.UpdateTimerView(DelayWork, WorkerState.Work,DelayWork);
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
            Agent.SetDestination(destination.position);
            Animator.SetBool("Walk", true);
            yield return null;

            while (Agent.pathPending)
                yield return null;

            while (Agent.remainingDistance > 0.1f)
                yield return null;

            transform.LookAt(lookPosition);
            Animator.SetBool("Walk", false);
            
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

            if (WorkerState == WorkerState.Relax)
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
            WorkerState = WorkerState.Relax;
            _isRelaxing = true;
        }
    }
}