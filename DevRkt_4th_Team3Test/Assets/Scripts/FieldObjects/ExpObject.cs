using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObject : ItemObject
{
    [SerializeField] private int _expValue = 1;

    /// <summary>
    /// I / 상호작용을 통한 경험치 획득
    /// </summary>
    /// <param name="pc"></param>
    public override void Interact(PlayerStats pc)
    {
        base.Interact(pc);
        ExpSystem es = pc.GetComponent<ExpSystem>();
        if (es)
            es.GainExp(_expValue);
        Debug.Log("경험치 획득");
    }
}