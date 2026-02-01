using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : ItemObject
{
    [SerializeField][Range(1,100)] public int _healValue = 50;
    public override void Interact(PlayerStats pc)
    {
        pc.Heal(_healValue);
        Debug.Log("hp회복");
        FieldObjectManager.Instance.RemoveDrobObject(this);
        base.Interact(pc);
    }
}
