using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    [field:SerializeField] public GameObject Player { get; set; }
    [SerializeField] private Vector3 _spawnPoint; 
    [field:SerializeField] public PlayerCamera Camera { get; set; }
     
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
        //GenerateManager<OrbitalWeaponManager>();
        
        GameObject player = Instantiate(Player,_spawnPoint, new Quaternion());
        Instantiate(Camera);
        Camera.player = player.transform;
    }

    void GenerateManager<T>() where T : Component
    {
        if (FindObjectOfType<T>() != null) return;

        var go = new GameObject(typeof(T).Name);
        go.AddComponent<T>();
        go.transform.SetParent(transform);
    }
}
