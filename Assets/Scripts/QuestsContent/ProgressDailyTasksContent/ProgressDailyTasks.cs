using System;
using System.Collections.Generic;
using MysteryGiftContent;
using PlayerContent.LevelContent;
using UI.Screens;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestsContent.ProgressDailyTasksContent
{
    public class ProgressDailyTasks : MonoBehaviour
    {
        [SerializeField] private ProgressDailyTasksViewer _progressDailyTasksViewer;
        [SerializeField] private TasksActivator _tasksActivator;
        [SerializeField] private DailyTasksCounter _dailyTasksCounter;
        [SerializeField] private List<MysteryPrize> prizes = new List<MysteryPrize>();
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private DailyGlobalTaskPrizeScreen _dailyGlobalTaskPrizeScreen;

        private bool _isReceived = false;
        private MysteryPrize _randomPrize;
        private List<MysteryPrize> _randomPrizes;

        public event Action<List<MysteryPrize>> DailyTasksProgressChanged;

        private void OnEnable()
        {
            _dailyTasksCounter.DailyTasksProgressChanged += ChangeValue;
            _dailyTasksCounter.DailyTasksUpdated += ClearValue;
        }

        private void OnDisable()
        {
            _dailyTasksCounter.DailyTasksProgressChanged -= ChangeValue;
            _dailyTasksCounter.DailyTasksUpdated -= ClearValue;
        }

        public void GetPrize()
        {
            Debug.Log("_isReceived " + _isReceived);
            Debug.Log("!_dailyTasksCounter.CheckCompletion() " + !_dailyTasksCounter.CheckCompletion());
            
            if (_isReceived || !_dailyTasksCounter.CheckCompletion())
            {
                Debug.Log("MNOTNOT BOTBPBOTGetPrize");
                return;
            }

            Debug.Log("GetPrize");
            SetReceivedValue(true);
            _tasksActivator.ChangeValue();
            SelectRandomPrize();
        }

        private void SelectRandomPrize()
        {
            List<MysteryPrize> eligiblePrizes = new List<MysteryPrize>();
            List<MysteryPrize> selectedPrizes = new List<MysteryPrize>();

            foreach (MysteryPrize prize in prizes)
            {
                if (prize.Level <= _playerLevel.CurrentLevel)
                    eligiblePrizes.Add(prize);
            }

            if (eligiblePrizes.Count > 0 && eligiblePrizes.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    int randomIndex = Random.Range(0, eligiblePrizes.Count);
                    selectedPrizes.Add(eligiblePrizes[randomIndex]);
                    eligiblePrizes.RemoveAt(randomIndex);
                }

                foreach (MysteryPrize prize in selectedPrizes)
                {
                    Debug.Log("Вы выиграли: " + prize.MysteryPrizeType);
                }
            }
            else
            {
                Debug.Log("Нет доступных призов для вашего уровня.");
            }

            _randomPrizes = selectedPrizes;
            Debug.Log("Init ща будет ");
            _dailyGlobalTaskPrizeScreen.OpenScreen();
            _dailyGlobalTaskPrizeScreen.Init(_randomPrizes);

            DailyTasksProgressChanged?.Invoke(_randomPrizes);
        }

        private void ChangeValue(int completedTasks, int maxTasks)
        {
            _progressDailyTasksViewer.ShowProgress(completedTasks, maxTasks, _isReceived);
        }

        private void ClearValue()
        {
            SetReceivedValue(false);
        }

        private void SetReceivedValue(bool value)
        {
            _isReceived = value;
        }
    }
}