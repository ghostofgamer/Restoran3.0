using System;
using System.Collections.Generic;
using I2.Loc;
using Io.AppMetrica;
using SettingsContent;
using TMPro;
using UI;
using UnityEngine;

namespace QuestsContent
{
    public class ChainTasksCounter : MonoBehaviour
    {
        [SerializeField] private List<Task> _chainTasks = new List<Task>();
        [SerializeField] private TaskUI _chainTaskUI;
        [SerializeField] private GameObject _taskLock;
        [SerializeField] private ChainTasksSaver _chainTasksSaver;
        [SerializeField] private TMP_Text _chainValueText;
        [SerializeField] private LanguageChanger _languageChanger;

        private int _currentTaskIndex = 0;
        public Task CurrentTask { get; private set; }

        public event Action CurrentTaskChanged;

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += ShowChainIndex;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= ShowChainIndex;
        }

        public void NextTask()
        {
            CurrentTask.ProgressSaved -= _chainTasksSaver.SaveProgress;
            Debug.Log("_currentTaskIndex " + _currentTaskIndex);

            AppMetrica.ReportEvent("ChainTaskCompleted", "{\"" + _currentTaskIndex + "\":null}");
            _currentTaskIndex++;
            PlayerPrefs.SetInt("CurrentChainTaskIndex", _currentTaskIndex);
            Debug.Log("NextTask " + _currentTaskIndex);

            if (CheckLockChainTasksValue())
                return;

            StartNextTask();
        }

        public void StartTask()
        {
            for (int i = 0; i < _chainTasks.Count; i++)
                _chainTasks[i].SetIndex(i);

            _currentTaskIndex = PlayerPrefs.GetInt("CurrentChainTaskIndex", _currentTaskIndex);

            ShowChainIndex();

            if (CheckLockChainTasksValue())
                return;

            ChainTasksSaveData saveData = _chainTasksSaver.LoadProgress();

            if (saveData == null || saveData.TaskIndex != _currentTaskIndex)
            {
                Debug.Log("No progress data found. Starting from the beginning.");

                Task currentTask = _chainTasks[_currentTaskIndex];
                CurrentTask = currentTask;
                CurrentTask.ProgressSaved += _chainTasksSaver.SaveProgress;
                Debug.Log("!!!_currentTask " + CurrentTask.Index);
                currentTask.InitTaskUI(_chainTaskUI);
                currentTask.StartTask();
            }
            else
            {
                Debug.Log("!!!_currentTask LoadSaveData " + saveData.TaskIndex);

                Task currentTask = _chainTasks[_currentTaskIndex];
                CurrentTask = currentTask;
                CurrentTask.ProgressSaved += _chainTasksSaver.SaveProgress;
                CurrentTask.InitTaskUI(_chainTaskUI);
                CurrentTask.LoadProgress(saveData.CurrentValue, saveData.TargetAmount, saveData.IsCompleted,
                    saveData.IsReceived);
            }

            CurrentTaskChanged?.Invoke();
        }

        public void StartNextTask()
        {
            ShowChainIndex();
            Debug.Log("No progress data found. Starting from the beginning.");
            Task currentTask = _chainTasks[_currentTaskIndex];
            CurrentTask = currentTask;
            CurrentTask.ProgressSaved += _chainTasksSaver.SaveProgress;
            Debug.Log("!!!_currentTask " + CurrentTask.Index);
            currentTask.InitTaskUI(_chainTaskUI);
            currentTask.StartTask();
            CurrentTaskChanged?.Invoke();
        }

        private bool CheckLockChainTasksValue()
        {
            if (_currentTaskIndex >= _chainTasks.Count)
            {
                Debug.Log("All tasks completed!");
                _taskLock.SetActive(true);
                return true;
            }

            Debug.Log("DONT All tasks completed! " + _currentTaskIndex + " ,,, " + _chainTasks.Count);
            _taskLock.SetActive(false);
            return false;
        }

        private void ShowChainIndex()
        {
            _chainValueText.text =
                $"{LocalizationManager.GetTermTranslation("Task")} № {(_currentTaskIndex + 1).ToString()}";
        }
        
        public void TestCompletedCurrentTask()
        {
            CurrentTask.tESTcOMPLETED();
            CurrentTask.CloseTask();
        }
    }

    [System.Serializable]
    public class ChainTasksSaveData
    {
        public int TaskIndex;
        public string TaskID;
        public int CurrentValue;
        public int TargetAmount;
        public bool IsCompleted;
        public bool IsReceived;
    }
}