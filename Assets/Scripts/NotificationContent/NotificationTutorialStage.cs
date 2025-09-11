using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NotificationContent
{
    public class NotificationTutorialStage : AbstractNotification<TutorialPrize>
    {
        [SerializeField]private Image _icon;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField]private NotificationMover _notificationMover;
        
        public override void Init(TutorialPrize data)
        {
            _icon.sprite = data.Sprite;
            _description.text = $" {LocalizationManager.GetTermTranslation("Task")}#{data.IndexStage}. {LocalizationManager.GetTermTranslation("CompletedTutorStage")}";
            // _description.text = $"Task {data.IndexStage} Completed";
            _valueText.text = data.Value.ToString();
            _notificationMover.AnimatePlashka();
        }
    }
}