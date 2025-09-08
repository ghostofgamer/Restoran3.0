using I2.Loc;
using UnityEngine;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "GoGameTask", menuName = "QuestConfigs/GoGameTaskConfig", order = 1)]
    public class GoGameTask : Task
    {
        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return true;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask Go gAME");
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("GoGame")} ({_targetAmount})";
            
            CurrentValue = _targetAmount;
            _languageChanger.LanguageChanged += LocalizationChanged;

            SubscribeToEvents();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }
        
        public override void LoadProgress(int currentValue, int targetAmount, bool isCompleted, bool isReceived)
        {
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("GoGame")} ({_targetAmount})";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();
            
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("GoGame")} ({_targetAmount})";
            

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }
        
        public override void CompleteTask()
        {
            base.CompleteTask();

            Debug.Log("!!!!!!!!!!!!!!    CompleteTaskGo gAME  " + this.name + "  !  " + CurrentValue + "  !  " +
                      _targetAmount + "  !  " + CheckCompletion());

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        public override void tESTcOMPLETED()
        {
            base.CompleteTask();
            TasksUI.ChangeValue(this, _localizationDescription, _targetAmount, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, true);
            SaveProgress();
        }

        public override void CloseTask()
        {
            base.CloseTask();
            
            if (!_isChainTask)
                TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                    PrizeTask.Amount, CheckCompletion());

            SaveProgress();
        }
    }
}