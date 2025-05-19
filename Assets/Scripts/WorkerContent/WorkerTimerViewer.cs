using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WorkerContent
{
    public class WorkerTimerViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Sprite _workSprite;
        [SerializeField] private Sprite _relaxSprite;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _timeFillImage;
        
        public void UpdateTimerView(float elapsedTime,WorkerState state,float duration)
        {
            Debug.Log("UpdateTimerView");
            _timerText.text = Mathf.CeilToInt(elapsedTime).ToString("00") + "s";

            if (state == WorkerState.Work)
            {
                _icon.sprite = _workSprite;
                Debug.Log("Work");
            }
            else
            {
                _icon.sprite = _relaxSprite;
                Debug.Log("relax");
            }
            
            float fillAmount = elapsedTime / duration;
            _timeFillImage.fillAmount = fillAmount;
            
            /*if (_isRelax)
        {
            _timerViewText.text = Mathf.CeilToInt(_elapsedTime).ToString("00") + "s";
            _workStateImage.sprite = _relaxSprite;
        }
        else
        {
            _timerViewText.text = Mathf.CeilToInt(_elapsedTime).ToString("00") + "s";
            _workStateImage.sprite = _workSprite;
        }
            
        float fillAmount = _elapsedTime / (_isRelax ? _delayRelax : _delayWork);
        _radialFillImage.fillAmount = fillAmount;*/
        }
    }
}