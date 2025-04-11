using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject _openBox;
    [SerializeField] private GameObject _closeBox;

    public bool IsOpen { get; private set; }


    public void SetValue(bool value)
    {
        IsOpen = value;
    }

    private void MoveSash(Vector3 angle)
    {
       
    }
}