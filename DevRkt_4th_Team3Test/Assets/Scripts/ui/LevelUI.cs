using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]private Image _gauge;
    [SerializeField]private TextMeshProUGUI _expText;
    [SerializeField]private TextMeshProUGUI _levelText;
    
    public float _lerpSpeed = 5f;
    [Header("Player")]
    [Tooltip("자동으로 캐릭터 데이터 찾습니다.")]
    public ExpSystem _expSystem;

    void Start()
    {
        if (_expSystem == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _expSystem = player.GetComponent<ExpSystem>();
            }
        }
    }
    
    void Update()
    {
        UpdateExpGauge();
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
