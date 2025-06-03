using System;
using System.Collections;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialContent
{
    public class TutorDescriptionUI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _completedImage;
        [SerializeField] private Image _fillProgrees;
        [SerializeField] private TMP_Text _progressPrecentValueText;

        private Coroutine _coroutine;
        private bool _isFirstStage = true;
        private bool _isOpen;
        private int _maxTutorialSteps;

        public event Action TutorialCompleted;

        private void Awake()
        {
            _maxTutorialSteps = System.Enum.GetValues(typeof(TutorialType)).Length - 1;
        }

        public void StartStage(string text, int indexState)
        {
            UpdateProgressVisual(indexState);

            if (!_isOpen)
            {
                if (_isFirstStage)
                {
                    _animator.SetBool("Open", true);
                    _isFirstStage = false;
                    StartNewStage(text);
                }
                else
                {
                    MoveUI(text);
                }
            }
            else
            {
                MoveUI(text);
            }

            _isOpen = true;
        }

        private void StartNewStage(string text)
        {
            _completedImage.gameObject.SetActive(false);
            _descriptionText.gameObject.SetActive(true);
            _descriptionText.text = text;
        }


        private void CompleteStage()
        {
            _completedImage.gameObject.SetActive(true);
            _descriptionText.gameObject.SetActive(false);
        }

        private void MoveUI(string text)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartMoveUI(text));
        }

        private IEnumerator StartMoveUI(string text)
        {
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            yield return new WaitForSeconds(1f);
            StartNewStage(text);
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
        }

        public void StartCompleted(string text, int currentStage)
        {
            StartCoroutine(StartMoveUICompleted(text, currentStage));
        }

        private IEnumerator StartMoveUICompleted(string text, int currentStage)
        {
            UpdateProgressVisual(currentStage);
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            yield return new WaitForSeconds(1f);
            StartNewStage(text);
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
            yield return new WaitForSeconds(1f);
            TutorialCompleted?.Invoke();
            yield return new WaitForSeconds(2f);
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
        }

        private void UpdateProgressVisual(int currentState)
        {
            if (_fillProgrees == null)
                return;

            float progress = CalculateFillAmount(currentState);
            int percentage = Mathf.RoundToInt(progress * 100);
            _progressPrecentValueText.text = $"Tutorial progress:{percentage}%";
            _fillProgrees.fillAmount = Mathf.Clamp01(progress);
        }

        private float CalculateFillAmount(int currentValue)
        {
            return Mathf.Clamp01((float)currentValue / _maxTutorialSteps);
        }
    }
}