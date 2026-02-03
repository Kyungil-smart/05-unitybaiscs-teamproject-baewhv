using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    [field: SerializeField] public Vector2Int TilePosition { get; set; }
    public GameObject TileDesign { get; private set; }

    public TileData TileData { get; private set; }

    public bool isSetTile => TileDesign != null;

    public void Init(GameObject obj)
    {
        TileDesign = Instantiate(obj, transform); //타일 랜덤 생성
        TileData = TileDesign.GetComponent<TileData>();
    }

    /// <summary>
    /// 필드 활성화
    /// </summary>
    public void EnableFieldTile()
    {
        GetComponent<BoxCollider>().enabled = true;
        if (TileDesign) TileDesign.SetActive(true);
        if(TileData) TileData.EnableSpawn();
    }

    /// <summary>
    /// 필드 비활성화
    /// </summary>
    public void DisableFieldTile()
    {
        GetComponent<BoxCollider>().enabled = false;
        if (TileDesign) TileDesign.SetActive(false);
        if (TileData) TileData.DisableSpawn();
    }

}