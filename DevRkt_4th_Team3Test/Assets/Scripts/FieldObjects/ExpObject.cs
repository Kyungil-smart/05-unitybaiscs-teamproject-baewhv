using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObject : ItemObject
{
    [SerializeField] private int _expValue;

    public bool isAbsolve { get; private set; } = false;



    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isAbsolve)
        {
            transform.position = Vector3.Lerp(transform.position, GameManager.Instance.Player.transform.position,1.5f * Time.fixedDeltaTime); }
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

    /// <summary>
    /// 흡수 시작
    /// fixed 업데이트에서 플레이어 추적.
    /// </summary>
    public void StartAbsolve()
    {
        isAbsolve = true;
    }

    /// <summary>
    /// 경험치 오브젝트 초기화
    /// 오브젝트 풀에서 꺼낼 때 초기에 세팅해주는 값.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
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
        isAbsolve = false;
    }

    public void CompressExp(EXPType type)
    {
        int expValue = 0;
        _body.sprite = FieldObjectManager.Instance.ExpDatas.LargeEXPSprite;
        _body.transform.localScale = new Vector3(12, 12, 12);
        switch (type)
        {
            case EXPType.small:
                expValue = FieldObjectManager.Instance.ExpDatas.SmallExpValue;
                break;
            case EXPType.medium:
                expValue = FieldObjectManager.Instance.ExpDatas.MediumExpValue;
                break;
            case EXPType.large:
                expValue = FieldObjectManager.Instance.ExpDatas.LargeExpValue;
                break;
        }
        _expValue += expValue;
        Debug.Log($"{gameObject.name} CompressExp! {_expValue}");
    }
}