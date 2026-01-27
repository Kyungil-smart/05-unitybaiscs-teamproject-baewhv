using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SettingsUI: MonoBehaviour, IPopup
    {
        public static SettingsUI Instance;

        void Awake() => Instance = this;
        
        /// <summary>
        /// 설정 팝업 열기
        /// </summary>
        public void Open()
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        /// <summary>
        /// 설정 팝업 닫기
        /// </summary>
        public void Close()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false); 
        }

        public void Apply()
        {
            
        }
    }
