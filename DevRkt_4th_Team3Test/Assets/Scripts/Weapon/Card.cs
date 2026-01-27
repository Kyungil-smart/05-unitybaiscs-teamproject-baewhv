using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //딕셔너리<등급, Serializable(등급에 따른 정보)>로 카드 등급에 따른 능력치 변화 저장.
    //카드는 등급 랜덤, 상승시킬 능력치 랜덤으로 나옴.
    
    
    [SerializeField] private float _value;
    [SerializeField] private string _variableName = "attackDamage";
       
    
}

