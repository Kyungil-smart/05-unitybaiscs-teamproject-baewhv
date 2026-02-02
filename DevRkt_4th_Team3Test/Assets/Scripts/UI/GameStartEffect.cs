using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartEffect : MonoBehaviour
{
    private TextMeshProUGUI _startText;

    void Awake()
    {
        _startText = GetComponent<TextMeshProUGUI>();
        // 시작할 때는 안 보이게 설정
        transform.localScale = Vector3.zero;
        _startText.alpha = 0;
    }

    void Start()
    {
        // 게임 씬 진입 시 효과 실행
        StartCoroutine(PlayStartAnimation());
    }

    private IEnumerator PlayStartAnimation()
    {
        //커지는 시간
        float duration = 0.5f;
        float timer = 0f;
        
        //시간동안만 반복 재생
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // 시간이 몇퍼인지 계산
            float progress = timer / duration;
            
            // 0에서 1까지 일정한 속도로 커짐
            // Mathf.Clamp01을 사용하면 progress가 1을 넘어가도 1에 고정
            float scale = Mathf.Clamp01(progress);
            // 크기 적용
            transform.localScale = Vector3.one * scale;
            // 투명도 변화
            // 크기가 커지는 속도와 동일하게 0에서 1로 선명해짐
            _startText.alpha = Mathf.Lerp(0, 1, progress * 2);
            
            yield return null;
        }
        
        //다 커진 상태로 잠깐 보여주기
        yield return new WaitForSeconds(0.5f);

        // Fade Out으로 사라지기
        timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            //계속 줄어듬
            _startText.alpha = 1 - (timer / 0.5f);
            yield return null;
        }

        // 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}