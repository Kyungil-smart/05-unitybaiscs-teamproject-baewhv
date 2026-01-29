using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalWeaponManager : MonoBehaviour
{
    public static OrbitalWeaponManager OrbitalInstance;
    //무기 프리펩 저장
    [SerializeField] private List<GameObject> prefabs =  new List<GameObject>();
    //궤도무기들 종류마다 저장
    public List<OrbitalWeapon> _orbitalWeapons = new List<OrbitalWeapon>();
    [SerializeField] private GameObject _player;
    
    //무기마다 궤도에 있는 무기들의 위치를 담는 딕셔너리. List<Transform>.Count = 한 종류의 무기에 존재하는 무기 개수
    private Dictionary<OrbitalWeapon, List<Transform>> _weaponLocations = new Dictionary<OrbitalWeapon, List<Transform>>();

    private Vector3 _lastTargetPosition;

    private void Awake()
    {
        if (OrbitalInstance == null)
        {
            OrbitalInstance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CreatePrefabsObject();
    }

    private void Start()
    {
        //플레이어 위치 저장
        _lastTargetPosition = _player.transform.position;
        
        foreach (OrbitalWeapon type in _orbitalWeapons) //나중에는 근접무기 하나만 배치해두고 나머지는 카드를 먹었을때 무기 생성.
        {
            //무기들 초기 배치
            SpawnWeapons(type);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _orbitalWeapons.Count; i++)
        {
            RotateWeapons(_orbitalWeapons[i]);    
        }
        
    }

    private void LateUpdate()
    {
        
    }

    // 360/각 무기 카운트 수 마다 무기 배치. 
    public void SpawnWeapons(OrbitalWeapon weapon)
    {
        if (weapon == null) return;
        ClearWeapons(weapon);
        
        int count = weapon.projectileCount;
        
        //리스트 생성 후 해당 OrbitalWeapon에 등록
        List<Transform> locations = new List<Transform>();
        _weaponLocations[weapon] = locations;

        float angle = 360f / weapon.projectileCount; // 무기들 사이의 각도
        for (int i = 0; i < weapon.projectileCount; i++)
        {
            //각 무기 각도마다 위치 계산
            Vector3 position = GetOrbitPosition(weapon, i * angle); 
            //생성
            GameObject obj = Instantiate(weapon.objectPrefab, position, Quaternion.identity);
            //무기가 isActive = true인경우만 활성화.
            obj.SetActive(weapon.isActive);
            //생성된 객체 transform 딕셔너리에 추가.
            _weaponLocations[weapon].Add(obj.transform);
        }
    }

    //무기의 숫자가 바뀌었을때 무기 지우고 다시 배치.
    private void ClearWeapons(OrbitalWeapon weapon)
    {
        
        if (_weaponLocations.ContainsKey(weapon))
        {
            
            List<Transform> currentWeapons = _weaponLocations[weapon];

            
            for (int i = 0; i < currentWeapons.Count; i++)
            {
                if (currentWeapons[i] != null)
                {
                    Destroy(currentWeapons[i].gameObject);
                }
            }

            
            currentWeapons.Clear();
        }
    }

    
    // 궤도에서의 위치를 계산
    private Vector3 GetOrbitPosition(OrbitalWeapon type, float angleInDegrees)
    {
        float radius = type.weaponRange;           // WeaponBase에서 가져온 반경
        float rad = Mathf.Deg2Rad * angleInDegrees; // 각도를 라디안으로 변환

        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;
        float y = 0f;
        
        return _player.transform.position + new Vector3(x, y, z); //위치를 반환.
    }
    
  
    // 물체를 업데이트에서 회전 시킴.
    private void RotateWeapons(OrbitalWeapon weapon)
    {
        float baseAngle = Time.time * weapon.weaponAttackSpeed; //플레이 시간에 따라 누적되는 각도 계산.
        for (int i = 0; i < _weaponLocations[weapon].Count; i++)
        {
            //회전각도 변화 반영해서 다음위치 계산.
            float angle = baseAngle + (360f / weapon.projectileCount) * i;
            Vector3 targetPos = GetOrbitPosition(weapon, angle);

            // 이동
            _weaponLocations[weapon][i].position = Vector3.Lerp(_weaponLocations[weapon][i].position, targetPos, Time.deltaTime * weapon.weaponAttackSpeed);
            _weaponLocations[weapon][i].LookAt(_player.transform.position);
        }
    }

    //무기를 추가함. 무기를 처음 얻을때 무기를 생성하는 함수.
    private void AddWeapon()
    {
        
    }

    //프리펩으로부터 객체를 만들어서 리스트 _orbitalWeapons에 등록
    private void CreatePrefabsObject()
    {
        int k = 0;
        for (int i = 0; i < prefabs.Count; i++)
        {
            
            //무기 프리펩으로 복제본을 instantiate 해서 그 값을 변경해야 꺼저도 프리펩 값 변경 안됨
            
            //프리펩이 OrbitalWeapon일때만 _orbitalWeapons 리스트에 추가.
            if (prefabs[i].GetComponent<OrbitalWeapon>() != null)
            {
                GameObject go = Instantiate(prefabs[i]);
                go.SetActive(false);
                OrbitalWeapon ow = go.GetComponent<OrbitalWeapon>();
                _orbitalWeapons.Add(ow);
                //Debug.Log(_orbitalWeapons[k]._weaponName);
                k++;
            }
        }

    }
}