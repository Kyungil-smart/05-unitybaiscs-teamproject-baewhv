using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [Header("UI")] [SerializeField] private Image _gauge;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private TextMeshProUGUI _levelText;

    public float _lerpSpeed = 5f;

    [Header("Player")] [Tooltip("자동으로 캐릭터 데이터 찾습니다.")]
    public ExpSystem _expSystem;

    [SerializeField] private GameObject _levelUpPopupObject;
    private GameObject _currentLevelUpPopup;
    private LevelUpPopupUI _levelUpPopupUI;
    private int _lastLevel = 1;

    void Start()
    {
        if (_levelUpPopupObject != null)
        {
            _currentLevelUpPopup = Instantiate(_levelUpPopupObject, GameObject.Find("Canvas").transform);
            _currentLevelUpPopup.SetActive(false);
            _levelUpPopupUI = _currentLevelUpPopup.GetComponent<LevelUpPopupUI>();
        }
    }

    void Update()
    {
        //TODO: 테스트 용도 추후 삭제
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(_levelUpPopupUI != null?"팝업있음":"팝업없음");
            _levelUpPopupUI.Open();
        }
        
        if (_expSystem == null || _levelUpPopupUI == null) return;
        
        UpdateExpGauge();

        // 레벨업 체크
        if (_expSystem.Level > _lastLevel)
        {
            _lastLevel = _expSystem.Level;
            //레벨업 팝업 표시
            _levelUpPopupUI.Open();
        }


    }

    private void UpdateExpGauge()
    {
        float targetFill = (float)_expSystem.CurrentExp / _expSystem.ExpToNextLevel;

        _gauge.fillAmount = Mathf.Lerp(_gauge.fillAmount, targetFill, Time.deltaTime * _lerpSpeed);

        if (_expText != null)
            _expText.text = $"EXP {_expSystem.CurrentExp} / {_expSystem.ExpToNextLevel}";

        if (_levelText != null)
            _levelText.text = $"{_expSystem.Level}";
    }
}