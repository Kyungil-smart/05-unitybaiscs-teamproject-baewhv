using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글턴
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                    _instance = new GameObject().AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    [field: SerializeField] private GameObject PlayerPrefab;
    public GameObject Player { get; private set; }
    [SerializeField] private Vector3 _spawnPoint; 
    
    [field:SerializeField] public PlayerCamera Camera { get; set; }

    [SerializeField] private LevelUI _levelUI;
    [SerializeField] private OrbitalWeaponManager _owm;
    [SerializeField] private RangedWeaponManager _rwm;
    
     
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        Init();
    }

    void Init()
    {
        GenerateManager<FieldManager>();
        
        Player = Instantiate(PlayerPrefab, _spawnPoint, new Quaternion());
        Instantiate(Camera).player = Player.transform;
        if(_levelUI)
            _levelUI._expSystem = Player.GetComponent<ExpSystem>();
        if(_owm)
            _owm._player = Player;
        if (_rwm)
            _rwm._player = Player;
    }

    void GenerateManager<T>() where T : Component
    {
        if (FindObjectOfType<T>() != null) return;

        var go = new GameObject(typeof(T).Name);
        go.AddComponent<T>();
        go.transform.SetParent(transform);
    }
}
