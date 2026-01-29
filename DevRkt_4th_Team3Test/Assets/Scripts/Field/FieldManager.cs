using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private GameObject _fieldTile;
    [SerializeField] private GameObject _designedTile;
    private List<List<FieldTile>> _tiles = new List<List<FieldTile>>();
    //루프시킬 맵의 최대 크기
    [SerializeField] private Vector2Int _preLoadMapSize = new Vector2Int(11,11);
    //활성화시킬 맵의 크기
    [SerializeField] private Vector2Int _mapSight = new Vector2Int(2,2);

    private readonly (int x, int z) tileSize = (16, 16);
    private static FieldManager _instance;

    private Vector2Int _playerPosition;
    private Vector2Int _playerTilePosition;

    public Vector2Int PlayerTilePosition
    {
        get => _playerTilePosition;
        set
        {
            if (_playerTilePosition == value) return;
            Vector2Int temp = value - _playerTilePosition;
            if (temp.x == _preLoadMapSize.x - 1)
                temp.x = -1;
            else if (temp.x == -(_preLoadMapSize.x - 1))
                temp.x = 1;
            if (temp.y == _preLoadMapSize.y - 1)
                temp.y = -1;
            else if (temp.y == -(_preLoadMapSize.y - 1))
                temp.y = 1;
            _playerPosition += temp;
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
        PlayerTilePosition = Vector2Int.zero;
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
        for (int y = 0; y < _preLoadMapSize.y; y++)
        {
            _tiles.Add(new List<FieldTile>());
            for (int x = 0; x < _preLoadMapSize.x; x++)
            {
                FieldTile tile = Instantiate(_fieldTile).GetComponent<FieldTile>();
                tile.transform.SetParent(gameObject.transform);
                tile.TilePosition = new Vector2Int(x, y);
                _tiles[y].Add(tile);
            }
        }
    }

    // 캐릭터가 움직인 위치를 기준으로 _mapSight만큼 활성화.
    // _mapSight에 해당하는 타일에 디자인 타일이 등록되지 않았다면 생성 및 초기화.
    private void LoadTile(Vector2Int tilePos) //0 0 
    {
        Debug.Log($"player pos {_playerPosition} / tile pos {tilePos}");
        for (int y = 0; y <= _mapSight.y * 2 + 2; y++)
        {
            for (int x = 0; x <= _mapSight.x * 2 + 2; x++)
            {
                int _x = tilePos.x - _mapSight.x - 1 + x;
                int _y = tilePos.y - _mapSight.y - 1 + y;
                int playerPos_X = _playerPosition.x - _mapSight.x - 1 + x;
                int playerPos_Y = _playerPosition.y - _mapSight.y - 1 + y;
                int posX = (_x + _preLoadMapSize.x) % _preLoadMapSize.x;
                int posY = (_y + _preLoadMapSize.y) % _preLoadMapSize.y;
                if (x == 0 || y == 0 || x == _mapSight.x * 2 + 2 || y == _mapSight.y * 2 + 2) //테두리 제거
                {
                    _tiles[posY][posX].DisableFieldTile();
                }
                else if (!_tiles[posY][posX].isSetTile) //없을 때 생성
                {
                    _tiles[posY][posX].TileDesign = Instantiate(_designedTile); //TODO 타일 종류 추가 시 랜덤 생성
                    _tiles[posY][posX].TileDesign.transform.SetParent(_tiles[posY][posX].transform);
                    _tiles[posY][posX].transform.position = new Vector3(playerPos_X * tileSize.x, 0, playerPos_Y * tileSize.z);
                    _tiles[posY][posX].EnableFieldTile();
                }
                else if (_tiles[posY][posX].TileDesign.activeSelf == false) //꺼져 있을 때 다시 켜기
                {
                    _tiles[posY][posX].transform.position = new Vector3(playerPos_X * tileSize.x, 0, playerPos_Y * tileSize.z);
                    _tiles[posY][posX].EnableFieldTile();
                }
            }
        }
    }
}