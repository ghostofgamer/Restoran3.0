using DG.Tweening;
using UnityEngine;

public class DotweenTestMoveAmnim : MonoBehaviour
{
    [Header("Настройки")] [SerializeField] private float _overshootDistance = 30f; // сила торможения
    [SerializeField] private float _flyDuration = 0.5f;
    [SerializeField] private float _exitDuration = 0.7f;
    [SerializeField] private float _pauseDuration = 1f;
    [SerializeField] private float _offsetY = -50f; // отрицательное значение = вниз от верхнего края

    private RectTransform _rect;
    private RectTransform _canvasRect;
    private bool _isAnimating = false;
    private Vector2 _startPos;
    private Vector2 _centerPos;
    private Vector2 _endPos;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _canvasRect = _rect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        _rect.pivot = new Vector2(0.5f, 0.5f);

        _startPos = new Vector2(_canvasRect.rect.width / 2 + _rect.rect.width,
            _canvasRect.rect.height / 2 + _offsetY);
        _rect.anchoredPosition = _startPos;
        _rect.localScale = Vector3.one;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !_isAnimating)
            AnimatePlashka();
    }

    public void AnimatePlashka()
    {
        _isAnimating = true;

        // стартовая позиция справа за экраном
        _startPos = new Vector2(_canvasRect.rect.width / 2 + _rect.rect.width,
            _canvasRect.rect.height / 2 + _offsetY);
        _rect.anchoredPosition = _startPos;
        _rect.localScale = Vector3.one;

        // центр Canvas с отступом
        _centerPos = new Vector2(0, _canvasRect.rect.height / 2 + _offsetY);

        // конечная позиция слева за экраном
        _endPos = new Vector2(-_canvasRect.rect.width / 2 - _rect.rect.width,
            _canvasRect.rect.height / 2 + _offsetY);

        Sequence seq = DOTween.Sequence();

        // 1. Прилет в центр
        seq.Append(_rect.DOAnchorPos(_centerPos, _flyDuration).SetEase(Ease.OutCubic));

        // 2. Торможение + squash/stretch
        seq.Append(_rect.DOAnchorPosX(_centerPos.x + _overshootDistance, 0.1f));
        seq.Join(_rect.DOScale(new Vector3(1.1f, 0.85f, 1f), 0.15f));

        seq.Append(_rect.DOAnchorPosX(_centerPos.x - _overshootDistance * 0.5f, 0.1f));
        seq.Join(_rect.DOScale(new Vector3(0.95f, 1.1f, 1f), 0.1f));

        seq.Append(_rect.DOAnchorPosX(_centerPos.x, 0.1f));
        seq.Join(_rect.DOScale(Vector3.one, 0.1f));

        // 3. Пауза
        seq.AppendInterval(_pauseDuration);

        // 4. Вылет влево
        seq.Append(_rect.DOAnchorPos(_endPos, _exitDuration).SetEase(Ease.InBack));

        seq.OnComplete(() => _isAnimating = false);
    }
}