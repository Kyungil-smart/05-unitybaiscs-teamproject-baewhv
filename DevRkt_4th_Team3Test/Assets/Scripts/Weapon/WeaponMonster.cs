using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMonster : MonoBehaviour
{
    public float HP = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
