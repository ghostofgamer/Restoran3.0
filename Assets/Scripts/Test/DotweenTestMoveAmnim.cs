using DG.Tweening;
using UnityEngine;


public class DotweenTestMoveAmnim : MonoBehaviour
{
    [SerializeField] private RectTransform _startPoint; // справа
    [SerializeField] private RectTransform _endPoint;
    [SerializeField] private float _overshootDistance = 30f;
    [SerializeField] private RectTransform _centerPoint;
    
    private RectTransform _rect;
    private bool _isAnimating = false;

    /*void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rect.anchoredPosition = _startPoint.anchoredPosition;

        // Чтобы центр был в центре объекта
        _rect.pivot = new Vector2(0.5f, 0.5f);
        // AnimatePlashka();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !_isAnimating)
            AnimatePlashka();
    }*/

    /*void AnimatePlashka()
    {
        isAnimating = true;

        // начальная позиция справа за экраном
        Vector3 startPos = new Vector3(Screen.width + rect.rect.width, rect.anchoredPosition.y, 0);
        rect.anchoredPosition = startPos;

        // куда прилетает (центр)
        Vector3 targetPos = new Vector3(0, rect.anchoredPosition.y, 0);

        Sequence seq = DOTween.Sequence();

        // 1. Прилетает справа
        // seq.Append(rect.DOAnchorPos(targetPos, 0.7f).SetEase(Ease.OutBack));
        seq.Append(rect.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutBack));

        // 2. Упругое "сжатие" по X (squash & stretch)
        // seq.Append(rect.DOScale(new Vector3(1.2f, 0.8f, 1), 0.15f));
        seq.Append(rect.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.15f));
        seq.Append(rect.DOScale(Vector3.one, 0.05f));

        // 3. Небольшая пауза
        seq.AppendInterval(1f);

        // 4. Вылет влево
        Vector3 leftPos = new Vector3(-Screen.width - rect.rect.width, rect.anchoredPosition.y, 0);
        seq.Append(rect.DOAnchorPos(leftPos, 0.7f).SetEase(Ease.InBack));

        seq.OnComplete(() => isAnimating = false);
    }*/

    /*void AnimatePlashka()
    {
        _isAnimating = true;

        // pivot ставим в центр (центр картинки совпадет с центром экрана)
        _rect.pivot = new Vector2(0.5f, 0.5f);

        // начальная позиция справа за экраном
        Vector3 startPos = new Vector3(Screen.width + _rect.rect.width, _rect.anchoredPosition.y, 0);
        _rect.anchoredPosition = startPos;

        // позиция в центре (по X = 0 значит центр экрана)
        Vector3 targetPos = new Vector3(0, _rect.anchoredPosition.y, 0);

        Sequence seq = DOTween.Sequence();

        // 1. Прилетает справа
        seq.Append(_rect.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutCubic));

        // 2. "Торможение":
        // немного уходим вправо + растягиваемся по X и сплющиваем по Y
        seq.Append(_rect.DOAnchorPosX(targetPos.x + 30f, 0.1f)); // отъезд вправо
        seq.Join(_rect.DOScale(new Vector3(1.1f, 0.85f, 1f), 0.15f));

        // обратный "отскок": чуть влево + уже по X, выше по Y
        seq.Append(_rect.DOAnchorPosX(targetPos.x - 15f, 0.1f));
        seq.Join(_rect.DOScale(new Vector3(0.95f, 1.1f, 1f), 0.1f));

        // возвращаемся в норму (центр и размер)
        seq.Append(_rect.DOAnchorPosX(targetPos.x, 0.1f));
        seq.Join(_rect.DOScale(Vector3.one, 0.1f));

        // 3. Пауза
        seq.AppendInterval(1f);

        // 4. Вылет влево
        Vector3 leftPos = new Vector3(-Screen.width - _rect.rect.width, _rect.anchoredPosition.y, 0);
        seq.Append(_rect.DOAnchorPos(leftPos, 0.7f).SetEase(Ease.InBack));

        seq.OnComplete(() => _isAnimating = false);
    }*/

    /*void AnimatePlashka()
    {
        _isAnimating = true;
        _rect.anchoredPosition = _startPoint.anchoredPosition;
        _rect.localScale = Vector3.one;
        
        Sequence seq = DOTween.Sequence();

        // 1. Прилетает из точки старта в центр
        // seq.Append(_rect.DOAnchorPos(_centerPoint.anchoredPosition, 0.5f).SetEase(Ease.OutCubic));
        seq.Append(_rect.DOAnchorPos(_centerPoint.anchoredPosition, 0.5f).SetEase(Ease.OutBack));

        // 2. Торможение (немного проскакивает вправо + squash/stretch)
        seq.Append(_rect.DOAnchorPos(_centerPoint.anchoredPosition + new Vector2(_overshootDistance, 0), 0.1f));
        seq.Join(_rect.DOScale(new Vector3(1.1f, 0.85f, 1f), 0.15f));

        // 3. Отскок влево
        seq.Append(_rect.DOAnchorPos(_centerPoint.anchoredPosition + new Vector2(-_overshootDistance * 0.5f, 0), 0.1f));
        seq.Join(_rect.DOScale(new Vector3(0.95f, 1.1f, 1f), 0.1f));

        // 4. Возвращение в норму
        seq.Append(_rect.DOAnchorPos(_centerPoint.anchoredPosition, 0.1f));
        seq.Join(_rect.DOScale(Vector3.one, 0.1f));

        // 5. Пауза
        seq.AppendInterval(0.3f);

        // 6. Вылет в EndPoint
        seq.Append(_rect.DOAnchorPos(_endPoint.anchoredPosition, 0.7f).SetEase(Ease.InBack));

        seq.OnComplete(() => _isAnimating = false);
    }*/
    
    
     /*[Header("Настройки")]
    public float overshootDistance = 30f;  // сила торможения
    public float flyDuration = 0.5f;
    public float exitDuration = 0.7f;
    public float pauseDuration = 1f;

    private RectTransform rect;
    private RectTransform canvasRect;
    private bool isAnimating = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasRect = rect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        rect.pivot = new Vector2(0.5f, 0.5f);

        // стартовая позиция сразу правее экрана
        Vector2 startPos = new Vector2(canvasRect.rect.width / 2 + rect.rect.width, 0);
        rect.anchoredPosition = startPos;
        rect.localScale = Vector3.one;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAnimating)
        {
            AnimatePlashka();
        }
    }

    void AnimatePlashka()
    {
        isAnimating = true;

        // стартовая позиция правее экрана
        Vector2 startPos = new Vector2(canvasRect.rect.width / 2 + rect.rect.width, 0);
        rect.anchoredPosition = startPos;
        rect.localScale = Vector3.one;

        // центр Canvas
        Vector2 centerPos = Vector2.zero;

        // конечная позиция левее экрана
        Vector2 endPos = new Vector2(-canvasRect.rect.width / 2 - rect.rect.width, 0);

        Sequence seq = DOTween.Sequence();

        // 1. Прилет в центр
        seq.Append(rect.DOAnchorPos(centerPos, flyDuration).SetEase(Ease.OutCubic));

        // 2. Торможение + squash/stretch
        seq.Append(rect.DOAnchorPosX(centerPos.x + overshootDistance, 0.1f));
        seq.Join(rect.DOScale(new Vector3(1.1f, 0.85f, 1f), 0.15f));

        seq.Append(rect.DOAnchorPosX(centerPos.x - overshootDistance * 0.5f, 0.1f));
        seq.Join(rect.DOScale(new Vector3(0.95f, 1.1f, 1f), 0.1f));

        seq.Append(rect.DOAnchorPosX(centerPos.x, 0.1f));
        seq.Join(rect.DOScale(Vector3.one, 0.1f));

        // 3. Пауза
        seq.AppendInterval(pauseDuration);

        // 4. Вылет влево
        seq.Append(rect.DOAnchorPos(endPos, exitDuration).SetEase(Ease.InBack));

        seq.OnComplete(() => isAnimating = false);
    }*/
     
      [Header("Настройки")]
    public float overshootDistance = 30f;  // сила торможения
    public float flyDuration = 0.5f;
    public float exitDuration = 0.7f;
    public float pauseDuration = 1f;
    public float offsetY = -50f; // отрицательное значение = вниз от верхнего края

    private RectTransform rect;
    private RectTransform canvasRect;
    private bool isAnimating = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasRect = rect.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        rect.pivot = new Vector2(0.5f, 0.5f);

        // стартовая позиция справа за экраном, с отступом от верха
        Vector2 startPos = new Vector2(canvasRect.rect.width / 2 + rect.rect.width, canvasRect.rect.height / 2 + offsetY);
        rect.anchoredPosition = startPos;
        rect.localScale = Vector3.one;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAnimating)
        {
            AnimatePlashka();
        }
    }

    void AnimatePlashka()
    {
        isAnimating = true;

        // стартовая позиция справа за экраном
        Vector2 startPos = new Vector2(canvasRect.rect.width / 2 + rect.rect.width, canvasRect.rect.height / 2 + offsetY);
        rect.anchoredPosition = startPos;
        rect.localScale = Vector3.one;

        // центр Canvas с отступом
        Vector2 centerPos = new Vector2(0, canvasRect.rect.height / 2 + offsetY);

        // конечная позиция слева за экраном
        Vector2 endPos = new Vector2(-canvasRect.rect.width / 2 - rect.rect.width, canvasRect.rect.height / 2 + offsetY);

        Sequence seq = DOTween.Sequence();

        // 1. Прилет в центр
        seq.Append(rect.DOAnchorPos(centerPos, flyDuration).SetEase(Ease.OutCubic));

        // 2. Торможение + squash/stretch
        seq.Append(rect.DOAnchorPosX(centerPos.x + overshootDistance, 0.1f));
        seq.Join(rect.DOScale(new Vector3(1.1f, 0.85f, 1f), 0.15f));

        seq.Append(rect.DOAnchorPosX(centerPos.x - overshootDistance * 0.5f, 0.1f));
        seq.Join(rect.DOScale(new Vector3(0.95f, 1.1f, 1f), 0.1f));

        seq.Append(rect.DOAnchorPosX(centerPos.x, 0.1f));
        seq.Join(rect.DOScale(Vector3.one, 0.1f));

        // 3. Пауза
        seq.AppendInterval(pauseDuration);

        // 4. Вылет влево
        seq.Append(rect.DOAnchorPos(endPos, exitDuration).SetEase(Ease.InBack));

        seq.OnComplete(() => isAnimating = false);
    }
}