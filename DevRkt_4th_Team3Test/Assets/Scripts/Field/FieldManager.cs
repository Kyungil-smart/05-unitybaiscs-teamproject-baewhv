using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private GameObject _fieldTile;
    [SerializeField] private GameObject _designedTile;
    private List<List<FieldTile>> _tiles = new List<List<FieldTile>>();
    private const int _mapWidth = 11;
    private const int _mapHeight = 11;
    private const int _mapSightWidth = 2;
    private const int _mapSightHeight = 2;

    private readonly (int x, int z) tileSize = (16, 16);
    private static FieldManager _instance;

    private Vector2 _playerTilePosition;

    public Vector2 PlayerTilePosition
    {
        get => _playerTilePosition;
        set
        {
            if (_playerTilePosition == value) return;
            LoadTile(value);
            _playerTilePosition = value;
        }
    }

    public void Awake()
    {
        if (!_fieldTile)
            _fieldTile = Resources.Load<GameObject>("Tile/fieldTile");
        if (!_designedTile)
            _designedTile = Resources.Load<GameObject>("Tile/TileA");
        InitMap();
    }

    private void Start()
    {
        PlayerTilePosition = Vector2.zero;
        LoadTile(PlayerTilePosition);
    }

    /// <summary>
    /// FieldManager 싱글턴 인스턴스 호출
    /// </summary>
    public static FieldManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<FieldManager>();
            }

            return _instance;
        }
    }


    //맵 생성
    private void InitMap()
    {
        for (int y = 0; y <= _mapHeight; y++)
        {
            _tiles.Add(new List<FieldTile>());
            for (int x = -_mapWidth / 2; x <= _mapWidth / 2; x++)
            {
                FieldTile tile = Instantiate(_fieldTile).GetComponent<FieldTile>();
                tile.transform.SetParent(gameObject.transform);
                tile.TilePosition = new Vector2(x, y - _mapHeight / 2);
                _tiles[y].Add(tile);
            }
        }
    }

    // 캐릭터가 움직였을 때 
    private void LoadTile(Vector2 tilePos)
    {
        for (int y = (int)tilePos.y - _mapSightHeight; y <= (int)tilePos.y + _mapSightHeight; y++)
        {
            for (int x = (int)tilePos.x - _mapSightWidth; x <= (int)tilePos.x + _mapSightWidth; x++)
            {
                int posX = (_mapWidth + x) % _mapWidth;
                int posY = (_mapHeight + y) % _mapHeight;
                if (!_tiles[posY][posX].isSetTile)
                {
                    _tiles[posY][posX].TileDesign = Instantiate(_designedTile);
                    _tiles[posY][posX].TileDesign.transform.SetParent(_tiles[posY][posX].transform);
                    _tiles[posY][posX].transform.position = new Vector3(x * tileSize.x, 0, y * tileSize.z);
                    _tiles[posY][posX].StartFieldTile();
                }
            }
        }
    }
}