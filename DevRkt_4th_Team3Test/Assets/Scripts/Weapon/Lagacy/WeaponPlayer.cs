using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayer : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float _moveSpeed;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            rigid.velocity = new Vector3(h*_moveSpeed, 0, v*_moveSpeed);    
        }
    }
}
