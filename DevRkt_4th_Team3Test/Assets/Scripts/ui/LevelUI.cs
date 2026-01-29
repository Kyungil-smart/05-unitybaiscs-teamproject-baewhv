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
    
    [Header("Data")]
    [SerializeField] public ExpSystem _expSystem;
    
    
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
