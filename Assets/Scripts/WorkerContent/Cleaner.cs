using System;
using System.Collections;
using Enums;
using RestaurantContent;
using RestaurantContent.TableContent;
using UnityEngine;
using UnityEngine.AI;
using WorkerContent.FSM;

namespace WorkerContent
{
    public class Cleaner : Worker
    {
        [SerializeField] private DirtyCounter _dirtyCounter;
        [SerializeField] private NavMeshObstacle _navMeshObstacle;

        public TableCleanliness CurrentDirtyTable { get; private set; }
        private Coroutine _cleaningCoroutine;

        private void Start()
        {
            Activate();
        }

        public override bool GetConditionsWorkUpdate()
        {
            return CurrentDirtyTable != null;
        }

        public override bool GetConditionsRelaxUpdate()
        {
            return CurrentDirtyTable == null && IsTired;
        }

        public override void StartWorking()
        {
            if (CurrentWorkerStateType == WorkerStateType.Relax)
                return;

            if (CurrentDirtyTable != null)
                return;

            if (CurrentDirtyTable == null)
                FindDirtyTable();
        }

        public override void StartRelaxing(Action action)
        {
            if (WorkerMover.Agent.destination != RelaxPosition.position)
            {
                StartCoroutine(WorkerMover.MoveToTarget(RelaxPosition, () =>
                {
                    if (action != null)
                        action?.Invoke();
                }));
            }
        }

        private void FindDirtyTable()
        {
            TableCleanliness dirtyTable = _dirtyCounter.GetDirtyTable();

            if (dirtyTable == null)
            {
                StartRelaxing(null);
                return;
            }

            CurrentDirtyTable = dirtyTable;
            StartCoroutine(WorkerMover.MoveToTarget(
                CurrentDirtyTable.CleanerPosition,
                () => StartCoroutine(CleanTable())
            ));
        }

        private IEnumerator CleanTable()
        {
            WorkerAnimation.SetCleaningAnimValue(true);
            yield return new WaitForSeconds(5f);

            if (CurrentDirtyTable != null && CurrentDirtyTable.PollutionLevel > 0)
                CurrentDirtyTable.ClearTable();

            WorkerAnimation.SetCleaningAnimValue(false);
            CurrentDirtyTable = null;

            if (CurrentState is WorkState)
                FindDirtyTable();
            else
                StartRelaxing(() => SetValueTired(true));
        }
    }
}