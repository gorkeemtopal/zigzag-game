using System;
using UnityEngine;
using Zenject;

[DisallowMultipleComponent, RequireComponent(typeof(Mover), typeof(GroundDetection), typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private ScorePopup _scorePopupPrefab;
    [SerializeField] private Canvas _uiCanvas;

    [SerializeField] private TriggerDetection _triggerDetection = null;
    
    private GameConfig _config = null;
    private Mover _mover = null;
    private GroundDetection _groundDetector = null;
    private int _score = 0;
    private IUserInput _userInput = null;
    private Rigidbody _body = null;

    public event Action<int> OnScoreChanged = null;
    public event Action OnLose = null; 

    [Inject]
    private void Construct(IUserInput userInput, GameConfig config)
    {
        _userInput = userInput;
        _config = config;
        
        _userInput.OnPress += EnableMover;
        _mover = GetComponent<Mover>();
        _mover.SetSpeed(_config.Speed);
    }
    
    private void Awake()
    {
        _groundDetector = GetComponent<GroundDetection>();
        _body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _triggerDetection.OnEnter += OnObjectDetected;
        _groundDetector.OnStateChanged += OnGroundStateChanged;
    }

    private void OnDisable()
    {
        _triggerDetection.OnEnter -= OnObjectDetected;
        _groundDetector.OnStateChanged -= OnGroundStateChanged;
    }

    private void EnableMover()
    {
        _mover.enabled = true;
        _userInput.OnPress -= EnableMover;
        _userInput.OnPress += ChangeDirection;
    }
    
    private void ChangeDirection()
    {
        _mover.ChangeDirection();
        IncreaseScore(1);
    }
    private void ShowPopup(int gained)
    {
        if (_scorePopupPrefab == null || _uiCanvas == null) return;

        // Popup oluþtur
        var popup = Instantiate(_scorePopupPrefab, _uiCanvas.transform);

        // TOPUN DÜNYA POZÝSYONU (biraz yukarýsý)
        Vector3 worldPos = transform.position + Vector3.up * 5f;

        // n
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Screes)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _uiCanvas.transform as RectTransform,
            screenPos,
            null, // Canvas Screen Space - Overlay ise null
            out Vector2 localPos
        );

        // UI pozisyonu
        popup.GetComponent<RectTransform>().anchoredPosition = localPos;

        // Yazýyý göster
        popup.Show("+" + gained);
    }
    private void OnObjectDetected(Collider other)
    {
      
        var pickup = other.GetComponent<Pickupable>();
        if (pickup == null)
        {
            Debug.Log("Pickupable yok");
            return;
        }

        int gained = pickup.Take();                 // SADECE 1 KEZ
        AudioManager.Instance?.PlayDiamondSound();  // ses

        IncreaseScore(gained);                      // skoru artýr
    
        ShowPopup(gained);
    }
    
    private void OnGroundStateChanged(bool isGrounded)
    {
        if (!isGrounded)
        {
            AudioManager.Instance?.StopMusic();
            enabled = false;
            OnLose?.Invoke();
            _userInput.OnPress -= ChangeDirection;
            _body.useGravity = true;
        }
    }

    private void IncreaseScore(int amount)
    {
        _score += amount;
        OnScoreChanged?.Invoke(_score);
    }
}