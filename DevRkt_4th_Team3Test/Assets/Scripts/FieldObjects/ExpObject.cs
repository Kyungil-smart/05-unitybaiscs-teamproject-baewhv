using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObject : ItemObject
{
    [SerializeField] private int _expValue;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// I / 상호작용을 통한 경험치 획득
    /// </summary>
    /// <param name="pc"></param>
    public override void Interact(PlayerStats pc)
    {
        ExpSystem es = pc.GetComponent<ExpSystem>();
        if (es)
            es.GainExp(_expValue);
        Debug.Log("경험치 획득");
        FieldObjectManager.Instance.RemoveExpObject(this);
        gameObject.SetActive(false);
    }

    public void SetExpObject(EXPType type, Vector3 position)
    {
        _body.color = Color.white;
        switch (type)
        {
            case EXPType.small:
                _body.sprite = FieldObjectManager.Instance.ExpDatas.SmallEXPSprite;
                _body.transform.localScale = new Vector3(6, 6, 6);
                _expValue = FieldObjectManager.Instance.ExpDatas.SmallExpValue;
                break;
            case EXPType.medium:
                _body.sprite = FieldObjectManager.Instance.ExpDatas.MediumEXPSprite;
                _body.color = new Color(r: 0.9845836f, g: 1f, b: 0.3160377f);
                _body.transform.localScale = new Vector3(8, 8, 8);
                _expValue = FieldObjectManager.Instance.ExpDatas.MediumExpValue;
                break;
            case EXPType.large:
                _body.sprite = FieldObjectManager.Instance.ExpDatas.LargeEXPSprite;
                _body.transform.localScale = new Vector3(10, 10, 10);
                _expValue = FieldObjectManager.Instance.ExpDatas.LargeExpValue;
                break;
        }
        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }
}