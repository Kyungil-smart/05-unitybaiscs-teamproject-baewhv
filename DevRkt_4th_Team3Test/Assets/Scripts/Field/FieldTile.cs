using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    [field:SerializeField] public Vector2Int TilePosition { get; set; }
    public GameObject TileDesign { get; set; }

    public bool isSetTile => TileDesign != null;

    /// <summary>
    /// 필드 활성화
    /// </summary>
    public void EnableFieldTile()
    {
        GetComponent<BoxCollider>().enabled = true;
        if (TileDesign) TileDesign.SetActive(true);
    }
    /// <summary>
    /// 필드 비활성화
    /// </summary>
    public void DisableFieldTile()
    {
        GetComponent<BoxCollider>().enabled = false;
        if (TileDesign) TileDesign.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FieldManager.Instance.PlayerTilePosition = TilePosition;
        }
    }
}