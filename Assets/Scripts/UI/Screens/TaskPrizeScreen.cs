using System.Collections;
using DG.Tweening;
using SettingsContent.SoundContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class TaskPrizeScreen : AbstractScreen
    {
        [SerializeField] private Image[] rewardImages;
        [SerializeField] private float jumpHeight = 50f;
        [SerializeField] private float jumpDuration = 0.3f;
        [SerializeField] private int jumpCount = 3;
        [SerializeField] private float flyUpDuration = 0.5f;
        [SerializeField] private float flyUpDistance = 500f;

        private Vector2[] _initialPositions;

        private void Awake()
        {
            _initialPositions = new Vector2[rewardImages.Length];
            
            for (int i = 0; i < rewardImages.Length; i++)
                _initialPositions[i] = rewardImages[i].rectTransform.anchoredPosition;
        }

        public void ShowReward(Sprite rewardSprite)
        {
            foreach (var image in rewardImages)
            {
                image.sprite = rewardSprite;
                image.gameObject.SetActive(true);
                image.color = new Color(1f, 1f, 1f, 1f);
            }

            SoundPlayer.Instance.PlayTaskPrizeShow();
            StartCoroutine(PlayRewardAnimation());
        }

        private IEnumerator PlayRewardAnimation()
        {
            Sequence[] sequences = new Sequence[rewardImages.Length];

            for (int i = 0; i < rewardImages.Length; i++)
            {
                sequences[i] = DOTween.Sequence();
                Vector3 originalPos = rewardImages[i].rectTransform.anchoredPosition;

                float delay = Random.Range(0f, 0.1f);
                float heightVariation = Random.Range(0.8f, 1.2f) * jumpHeight;
                float durationVariation = Random.Range(0.9f, 1.1f) * jumpDuration;

                for (int j = 0; j < jumpCount; j++)
                {
                    sequences[i].AppendCallback(() => {
                        SoundPlayer.Instance.PlayJumpPrizeTaskImage();
                    });
                    
                    sequences[i].Append(rewardImages[i].rectTransform
                            .DOAnchorPosY(originalPos.y + heightVariation, durationVariation)
                            .SetEase(Ease.OutQuad))
                        .Append(rewardImages[i].rectTransform
                            .DOAnchorPosY(originalPos.y, durationVariation)
                            .SetEase(Ease.InQuad))
                        .AppendInterval(delay);
                }

                sequences[i].Insert(0, rewardImages[i].rectTransform
                    .DORotate(new Vector3(0, 0, Random.Range(-10f, 10f)), durationVariation * jumpCount * 2)
                    .SetEase(Ease.InOutSine));
            }

            foreach (var seq in sequences)
            {
                seq.Join(DOTween.Sequence().AppendInterval(jumpDuration * jumpCount * 2));
            }

            yield return new WaitForSeconds(jumpDuration * jumpCount * 2 + 0.2f);

            for (int i = 0; i < rewardImages.Length; i++)
            {
                float randomDelay = Random.Range(0f, 0.2f);
                var image = rewardImages[i];
                Sequence flySequence = DOTween.Sequence();
                flySequence.Append(image.rectTransform
                    .DOAnchorPosY(image.rectTransform.anchoredPosition.y + flyUpDistance, flyUpDuration)
                    .SetEase(Ease.InBack));
                flySequence.Join(image.DOFade(0f, flyUpDuration).SetEase(Ease.Linear))
                    .SetDelay(randomDelay)
                    .OnComplete(() => image.gameObject.SetActive(false));
            }

            yield return new WaitForSeconds(flyUpDuration + 0.3f);
            CloseScreen();
        }

        private void OnDisable()
        {
            for (int i = 0; i < rewardImages.Length; i++)
            {
                var image = rewardImages[i];
                image.rectTransform.DOKill();
                image.DOKill();
                image.rectTransform.anchoredPosition = _initialPositions[i];
                image.rectTransform.rotation = Quaternion.identity;
                image.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}