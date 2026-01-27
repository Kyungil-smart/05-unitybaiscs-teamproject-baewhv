using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalWeaponManager : MonoBehaviour
{
    //궤도무기들 종류마다 저장
    [SerializeField] private List<OrbitalWeapon> _orbitalWeapons = new List<OrbitalWeapon>();
    [SerializeField] private WeaponPlayer _player;
    
    //무기마다 궤도에 있는 무기들의 위치를 담는 딕셔너리
    private Dictionary<OrbitalWeapon, List<Transform>> _weaponLocations = new Dictionary<OrbitalWeapon, List<Transform>>();
    
    private void Start()
    {
        foreach (OrbitalWeapon type in _orbitalWeapons)
        {
            //무기들 배치한다.
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

    // 360/각 무기 카운트 수 마다 무기 배치. 
    private void SpawnWeapons(OrbitalWeapon weapon)
    {
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
            
            //생성된 객체 transform 딕셔너리에 추가.
            _weaponLocations[weapon].Add(obj.transform);
        }
    }

    //무기의 숫자가 바뀌었을때 무기 지우고 다시 배치.
    private void ClearWeapons(OrbitalWeapon weapon)
    {
        //무기에 객체가 있으면 해당 무기에서 생성된 객체를 모두 지움. 
        if (_weaponLocations.ContainsKey(weapon))
        {
            _weaponLocations[weapon].Clear();
        }
    }

    
    /// <summary>
    /// 궤도에서의 위치를 계산
    /// </summary>
    /// <param name="type"></param>
    /// <param name="angleInDegrees"></param>
    /// <returns></returns>
    private Vector3 GetOrbitPosition(OrbitalWeapon type, float angleInDegrees)
    {
        float radius = type.weaponRange;           // WeaponBase에서 가져온 반경
        float rad = Mathf.Deg2Rad * angleInDegrees; // 각도를 라디안으로 변환

        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;
        float y = _player.transform.position.y;
        
        return transform.position + new Vector3(x, y, z); //위치를 반환.
    }
    
    /// <summary>
    /// 물체를 업데이트에서 회전 시킴.
    /// OrbitalWeapon을 받아서 각 무기종류에 있는 무기들을 회전시킴 
    /// </summary>
    /// <param name="weapon"></param>
    private void RotateWeapons(OrbitalWeapon weapon)
    {
        for (int i = 0; i < _weaponLocations[weapon].Count; i++)
        {
            _weaponLocations[weapon][i].RotateAround(_player.transform.position, Vector3.up, weapon.weaponAttackSpeed);
        }
    }
}
