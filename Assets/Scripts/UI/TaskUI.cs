using ADSContent;
using QuestsContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TaskUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _taskDescription;
        [SerializeField] private TMP_Text _taskProgress;
        [SerializeField] private Image _taskPrizeIcon;
        [SerializeField] private TMP_Text _taskPrizeAmount;
        [SerializeField] private Image _taskProgressImage;
        [SerializeField] private GameObject _activeImage;
        [SerializeField] private GameObject _completeButton;
        [SerializeField] private GameObject _receiveImage;
        [SerializeField] private TaskPrizeRecipient _taskPrizeRecipient;
        [SerializeField] private ADS _ads;

        private Task _task;

        public void ChangeValue(Task task, string taskDescription, float currentValue, float maxValue,
            Sprite taskPrizeIcon, int taskPrizeAmount, bool completed, bool received)
        {
            _task = task;
            _taskPrizeIcon.sprite = taskPrizeIcon;
            _taskPrizeAmount.text = taskPrizeAmount.ToString();
            _taskDescription.text = taskDescription;
            _taskProgress.text = $"{currentValue}/{maxValue}";
            _taskProgressImage.fillAmount = currentValue / maxValue;
            SetValue(completed, received);
        }

        public void CompleteTask()
        {
            Debug.Log("CompleteTask!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + " " + _task.TaskId);
            _task.ReceivePrize();
            SetValue(_task.IsCompleted, _task.IsReceived);
        }

        public void CompleteTaskWithAds()
        {
            if (_ads == null)
                return;

            _ads.ShowRewarded(() =>
            {
                _task.ReceivePrize();
                SetValue(_task.IsCompleted, _task.IsReceived);
            });
        }

        public void SetValue(bool completed, bool received)
        {
            Debug.Log("SetValue!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + completed + " " + _task.TaskId + received);
            Debug.Log("_task.IsReceived!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! " + _task.TaskId + _task.IsReceived);

            _activeImage.SetActive(!completed && !received);
            _completeButton.SetActive(completed && !received);
            _receiveImage.SetActive(_task.IsReceived);
        }
    }
}