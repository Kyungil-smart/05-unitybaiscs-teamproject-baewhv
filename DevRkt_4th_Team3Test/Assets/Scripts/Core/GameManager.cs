using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                    _instance = new GameObject("GameManager").AddComponent<GameManager>();
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
        
        if (!Player)                                        //게임매니저에 플레이어가 등록되지 않았다면
        {
            Player = FindObjectOfType<PlayerStats>().GameObject(); //우선 하이라키에서 플레이어 탐색
            if (!Player)                                                    //그래도 없다면
                Player = Instantiate(PlayerPrefab, _spawnPoint, new Quaternion()); //플레이어 생성
        }

        Player.name = "Player";
        Instantiate(Camera).player = Player.transform;
        if(_levelUI)
            _levelUI._expSystem = Player.GetComponent<ExpSystem>();
        if(_owm)
            _owm._player = Player;
        if (_rwm)
            _rwm._player = Player;
        PlayerStats ps = Player.GetComponent<PlayerStats>();
        if(ps)
            ps.OnPlayerDeath += GameOver;
    }

    void GenerateManager<T>() where T : Component
    {
        if (FindObjectOfType<T>() != null) return;

        var go = new GameObject(typeof(T).Name);
        go.AddComponent<T>();
        go.transform.SetParent(transform);
    }

    private void GameOver()
    {
        Destroy(_owm.gameObject);
        Destroy(_rwm.gameObject);
        SceneManager.LoadScene(2);
    }
}
