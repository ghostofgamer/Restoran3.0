using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestsContent.ProgressDailyTasksContent
{
    public class ProgressDailyTasksViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private Image _progressImage;

        [SerializeField] private Button _prizeButton;
        [SerializeField] private Animator _prizeAnimation;

        public void ShowProgress(int completedTasks, int maxTasks, bool isReceived)
        {
            _progressText.text = $"{completedTasks}/{maxTasks}";
            _progressImage.fillAmount = completedTasks / maxTasks;

            ChangePrizeState(completedTasks, maxTasks, isReceived);
        }

        private void ChangePrizeState(int completedTasks, int maxTasks, bool isReceived)
        {
            _prizeButton.interactable = (completedTasks >= maxTasks) && !isReceived;
            _prizeAnimation.enabled = (completedTasks >= maxTasks) && !isReceived;
        }
    }
}