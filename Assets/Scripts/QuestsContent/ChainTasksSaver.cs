using System.IO;
using UnityEngine;

namespace QuestsContent
{
    public class ChainTasksSaver : MonoBehaviour
    {
        private const string SaveFileName = "chain_tasks_save.json";

        [SerializeField] private ChainTasksCounter _chainTasksCounter;

        public void SaveProgress()
        {
            if (_chainTasksCounter.CurrentTask == null)
            {
                Debug.LogWarning("No current task to save.");
                return;
            }

            ChainTasksSaveData saveData = new ChainTasksSaveData
            {
                TaskIndex = _chainTasksCounter.CurrentTask.Index,
                TaskID = _chainTasksCounter.CurrentTask.TaskID,
                CurrentValue = _chainTasksCounter.CurrentTask.CurrentValueTask,
                TargetAmount = _chainTasksCounter.CurrentTask.TargetAmount,
                IsCompleted = _chainTasksCounter.CurrentTask.IsCompleted,
                IsReceived = _chainTasksCounter.CurrentTask.IsReceived
            };

            Debug.Log($"Saving Chain progress: TaskIndex={saveData.TaskIndex}, TaskID={saveData.TaskID}, " +
                      $"CurrentValue={saveData.CurrentValue}, IsCompleted={saveData.IsCompleted}, " +
                      $"IsReceived={saveData.IsReceived}");

            try
            {
                string json = JsonUtility.ToJson(saveData, true);
                string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
                File.WriteAllText(filePath, json);
                Debug.Log($"Progress saved to {filePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save progress: {e.Message}");
            }
        }

        public ChainTasksSaveData LoadProgress()
        {
            string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                ChainTasksSaveData saveData = JsonUtility.FromJson<ChainTasksSaveData>(json);

                Debug.Log($"Loading Chain progress: TaskIndex={saveData.TaskIndex}, TaskID={saveData.TaskID}, " +
                          $"CurrentValue={saveData.CurrentValue}, IsCompleted={saveData.IsCompleted}, " +
                          $"IsReceived={saveData.IsReceived}");

                return saveData;
            }

            return null;
        }

        [ContextMenu("Clear Progress")]
        public void ClearProgress()
        {
            string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Saved progress file deleted successfully.");
            }
        }
    }
}