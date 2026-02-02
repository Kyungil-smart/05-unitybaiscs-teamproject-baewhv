using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

    public class IntroDialogueManager: MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Image _dialogueImage;
    
        [Header("Dialog Data")]
        [SerializeField] private Sprite[] _introImages;
        [SerializeField] private string[] _sentences;
        
        [Header("Settings")]
        [SerializeField] private float _typingSpeed = 0.05f;
        private int _currentIndex = 0;
        private bool _isTyping = false;
        private Coroutine _typingCoroutine;
        
        void Start()
        {
            ShowCurrentDialogue();
        }

        void Update()
        {
            // 클릭이나 스페이스바 입력 시 다음으로
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnChangeDialogue();
            }
        }

        public void OnChangeDialogue()
        {
            if (_isTyping)
            {
                // 타이핑 중일 때 클릭하면 대사 바로 완성
                FinishTyping();
            }
            else
            {
                // 대사가 완성된 상태면 다음 대사로
                NextDialogue();
            }
        }

        private void ShowCurrentDialogue()
        {
            if (_currentIndex < _sentences.Length)
            {
                // 배경 이미지 교체
                if (_introImages.Length > _currentIndex)
                {
                    _dialogueImage.sprite = _introImages[_currentIndex];
                }

                // 타이핑 시작
                if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
                _typingCoroutine = StartCoroutine(TypeSentence(_sentences[_currentIndex]));
            }
        }
        
        // 한 글자씩 출력
        private IEnumerator TypeSentence(string sentence)
        {
            _isTyping = true;
            _dialogueText.text = "";

            // \n 줄바꿈 처리
            string processedSentence = sentence.Replace("\\n", "\n");
            
            //텍스트 한글자씩 표시
            foreach (char letter in processedSentence.ToCharArray())
            {
                _dialogueText.text += letter;
                // 설정한 속도만큼 대기
                yield return new WaitForSeconds(_typingSpeed); 
            }

            _isTyping = false;
        }
            
        //그냥 한번에 출력
        private void FinishTyping()
        {
            StopCoroutine(_typingCoroutine);
            _dialogueText.text = _sentences[_currentIndex].Replace("\\n", "\n");
            _isTyping = false;
        }

        public void NextDialogue()
        {
            _currentIndex++;

            if (_currentIndex < _sentences.Length)
            {
                ShowCurrentDialogue();
            }
            else
            {
                // 모든 대사가 끝났을 때 씬 전환
                EnterGameScene();
            }
        }
        
        /// <summary>
        /// 게임씬으로 이동
        /// </summary>
        public void EnterGameScene()
        {
            AudioManager.Instance.PlayStartSFX();
            MonsterManager.ResetCount(); 
            SceneManager.LoadScene(1);
        }
    }
