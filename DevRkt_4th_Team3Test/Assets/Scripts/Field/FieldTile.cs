using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    public Vector2 TilePosition { get; set; }
    public GameObject TileDesign { get; set; }

    public bool isSetTile => TileDesign != null;

    public void StartFieldTile()
    {
        GetComponent<BoxCollider>().enabled = true;
        if(TileDesign) TileDesign.SetActive(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            Debug.Log(TilePosition);
            FieldManager.Instance.PlayerTilePosition = TilePosition;
        }
    }

}
