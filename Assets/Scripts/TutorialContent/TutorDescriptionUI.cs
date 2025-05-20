using System.Collections;
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

        private Coroutine _coroutine;
        private bool _isFirstStage = true;
        private bool _isOpen;

        public void StartStage(string text)
        {
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

        public void StartCompleted(string text)
        {
            StartCoroutine(StartMoveUICompleted(text));
        }
        
        private IEnumerator StartMoveUICompleted(string text)
        {
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            yield return new WaitForSeconds(1f);
            StartNewStage(text);
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
            yield return new WaitForSeconds(1f);
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
        }
    }
}