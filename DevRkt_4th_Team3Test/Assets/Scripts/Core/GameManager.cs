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
                DontDestroyOnLoad(_instance);
            }
            
            return _instance;
        }
    }

    [field: SerializeField] private GameObject PlayerPrefab;
    public GameObject Player { get; private set; }
    [field:SerializeField] public PlayerCamera Camera { get; set; }
    [SerializeField] private Vector3 _spawnPoint;
    private bool isPlayerDead = false;

    [Header("Weapon")]
    [SerializeField] private WeaponManager _wm;
    [SerializeField] private CardManager _cm;
    [SerializeField] private OrbitalWeaponManager _owm;
    [SerializeField] private RangedWeaponManager _rwm;
    
    
    [Header("UI")]
    [SerializeField] private LevelUI _levelUI;
    private int _killCount = 0;
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(_instance);
        Init();
    }

    void Init()
    {
        var fieldManager = FieldManager.Instance;
        var fieldObjectManager = FieldObjectManager.Instance;

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
            ps.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        isPlayerDead = true;
        StartCoroutine(StartGameOver());
    }

    private IEnumerator StartGameOver()
    {
        yield return YieldContainer.WaitForSeconds(2.5f);
        SetGameOver();
    }

    public void SetGameOver()
    {
        Destroy(_cm.gameObject);
        Destroy(_wm.gameObject);
        Destroy(_owm.gameObject);
        Destroy(_rwm.gameObject);
        Destroy(FieldObjectManager.Instance.gameObject);
        Destroy(FieldManager.Instance.gameObject);
        
        Destroy(gameObject);
        SceneManager.LoadScene(2);
    }
}
