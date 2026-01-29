using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldObjectManager : Singleton<FieldObjectManager>
{
    private List<FieldObject> _objs = new List<FieldObject>();
    private int _maxObjects = 10;

    private List<ExpObject> _expObjects = new List<ExpObject>();
    private int _maxExpObject = 100;
    private int _expObjectActiveCount = 0;
    private ExpObject _compressExpObject = null;
    private GameObject _go_Exp;
    
    public EXPSprites ExpSprites { get; private set; }
    public GameObject _item_Exp { get; private set; }

    private void Awake()
    {
        _go_Exp = new GameObject("ExpObjects");
        _go_Exp.transform.SetParent(transform);
        _item_Exp = Resources.Load<GameObject>("FieldObject/Item/Item_EXP");
        ExpSprites = Resources.Load("FieldObject/Item/EXPSprites").GetComponent<EXPSprites>();
        PoolingExpObject(40);
    }

    //오브젝트 풀링용
    private void PoolingExpObject(int count)
    {
        if (_expObjects.Count >= _maxObjects) return;   //최대면 더 늘리지 않음.
        if (_expObjects.Count + count > _maxExpObject) count = _expObjects.Count + count - _maxObjects;
        for (int i = 0; i < count; i++)
        {
            _expObjects.Add(Instantiate(_item_Exp, _go_Exp.transform).GetComponent<ExpObject>());
        }
    }

    /// <summary>
    /// 경험치 오브젝트 생성
    /// </summary>
    /// <param name="type"></param>
    public void MakeExpObject(EXPType type, Vector3 position)
    {
        if (_expObjectActiveCount >= _maxObjects) return; //Todo 빨간거에 합치기
        if(_expObjectActiveCount >= _expObjects.Count) PoolingExpObject(10);
        foreach (ExpObject obj in _expObjects)
        {
            if (!obj.gameObject.activeSelf)
            {
                obj.SetExpObject(type, position);
                //빨강일 경우, 빨강이 등록되어있는데 꺼져있는 경우,
                break;
            }
        }
        
        _expObjectActiveCount++;
    }

    public void RemoveExpObject(ExpObject obj)
    {
        if (_compressExpObject == obj) _compressExpObject = null; //모아둔거 먹으면 떼놓기.
        _expObjectActiveCount--;
    }

    /// <summary>
    /// 필드 오브젝트 추가
    /// </summary>
    /// <param name="obj">복제할 오브젝트 원본</param>
    /// <param name="position">생성할 위치</param>
    /// <returns></returns>
    public FieldObject SetObject(FieldObject obj, Vector3 position)
    {
        FieldObject makedObject = Instantiate(obj, position, new Quaternion(), transform);
        _objs.Add(makedObject);
        return makedObject;
    }

    /// <summary>
    /// 오브젝트 삭제
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObject(FieldObject obj)
    {
        if (_objs.Contains(obj))
            _objs.Remove(obj);
        Destroy(obj.gameObject);
    }
}

public enum EXPType
{
    small,
    medium,
    large
}