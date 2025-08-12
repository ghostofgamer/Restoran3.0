using PlayerContent.LevelContent;
using UnityEngine;

public class InterfaceButtonActivator : MonoBehaviour
{
    [SerializeField] private PlayerLevel _playerLevel;
    [SerializeField] private GameObject _workerButton;

    private int _workerButtonActivateLevel = 5;

    private void OnEnable()
    {
        _playerLevel.LevelChanged += ChangeValue;
    }

    private void OnDisable()
    {
        _playerLevel.LevelChanged -= ChangeValue;
    }

    private void Start()
    {
        ChangeValue(_playerLevel.CurrentLevel);
    }

    private void ChangeValue(int level)
    {
        _workerButton.SetActive(level >= _workerButtonActivateLevel);
    }
}