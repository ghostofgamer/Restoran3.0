using QuestsContent;
using UnityEngine;

public abstract class Task : ScriptableObject
{
    [SerializeField] private int _index;
    [SerializeField] private string _taskName;
    [TextArea] [SerializeField] private string _description;
    
    public bool IsCompleted { get; protected set; }

    public abstract bool CheckCompletion();

    public virtual void StartTask()
    {
        Debug.Log("StartTask =>" + _index);
        
        IsCompleted = CheckCompletion();

        if (!IsCompleted)
        {
            Initialization();
            SubscribeToEvents();
        }
    }

    public abstract void UpdateTask();

    public virtual void CompleteTask()
    {
        Debug.Log("CompleteTaskTask");
        IsCompleted = true;
        UnsubscribeFromEvents();
        TasksActivator.Instance.NextTask();
    }

    protected abstract void Initialization();
    protected abstract void SubscribeToEvents();
    protected abstract void UnsubscribeFromEvents();
}