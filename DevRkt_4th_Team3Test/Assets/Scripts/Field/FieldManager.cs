using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField]private GameObject _tile;
    private List<GameObject> _tiles = new List<GameObject>();
    private const int _mapWidth = 5;
    private const int _mapHeight = 5;

    private static FieldManager _instance;
    
    public static FieldManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<FieldManager>();
                //DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Start()
    {
        CreateMap();
    }

    private void CreateMap()
    {
        //-32 -16 0 16 32
        (int x, int z) leftTop = (-32,-32); 
        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                GameObject tile = Instantiate(_tile, new Vector3(leftTop.x + 16 * x, 0, leftTop.z + 16 * y), new Quaternion());
                tile.transform.SetParent(gameObject.transform);
                _tiles.Add(tile);
            }
        }
    }


}
