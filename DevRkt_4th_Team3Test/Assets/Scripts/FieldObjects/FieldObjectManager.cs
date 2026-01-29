using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectManager : Singleton<FieldObjectManager>
{
    private List<FieldObject> _objs = new List<FieldObject>();
    [SerializeField] private int _initObjectCount = 40;
    public GameObject _item_Exp_Small { get; private set; }
    public GameObject _item_Exp_Medium { get; private set; }
    public GameObject _item_Exp_Large { get; private set; }

    private void Awake()
    {
        _item_Exp_Small = Resources.Load<GameObject>("FieldObject/Item/Item_EXP_Small");
        _item_Exp_Medium = Resources.Load<GameObject>("FieldObject/Item/Item_EXP_Medium");
        _item_Exp_Large = Resources.Load<GameObject>("FieldObject/Item/Item_EXP_Large");
    }

    /// <summary>
    /// 필드 오브젝트 추가
    /// </summary>
    /// <param name="obj">복제할 오브젝트 원본</param>
    /// <param name="position">생성할 위치</param>
    /// <returns></returns>
    public FieldObject SetObject(FieldObject obj, Vector3 position)
    {
        FieldObject makedObject = Instantiate(obj, position, new Quaternion());
        _objs.Add(makedObject);
        return makedObject;
    }
    
    /// <summary>
    /// 오브젝트 삭제
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObject(FieldObject obj)
    {
        if(_objs.Contains(obj))
            _objs.Remove(obj);
        Destroy(obj);
    }
}
