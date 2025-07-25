using System;
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

        public event Action TaskCompleted;

        private Task _task;

        public void ChangeValue(Task task, string taskDescription, float currentValue, float maxValue,
            Sprite taskPrizeIcon, int taskPrizeAmount, bool completed)
        {
            _task = task;
            _taskPrizeIcon.sprite = taskPrizeIcon;
            _taskPrizeAmount.text = taskPrizeAmount.ToString();
            _taskDescription.text = taskDescription;
            _taskProgress.text = $"{currentValue}/{maxValue}";
            _taskProgressImage.fillAmount = currentValue / maxValue;
            SetValue(completed, false);
        }

        public void CompleteTask()
        {
            SetValue(true, true);
            _taskPrizeRecipient.ClaimPrize(_task.PrizeTask);
            TaskCompleted?.Invoke();
        }

        private void SetValue(bool completed, bool received)
        {
            _activeImage.SetActive(!completed && !received);
            _completeButton.SetActive(completed && !received);
            _receiveImage.SetActive(received);
        }
    }
}