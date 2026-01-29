using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    /// <summary>
    /// 플레이어에게 카메라 고정
    /// 이전 카메라가 오브젝트에 부딪힐 시 회전하여 수정
    /// </summary>
    [SerializeField]public Transform player;
    [SerializeField]private Vector3 offset = new Vector3(0, 30, 0);

    public void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            // 플레이어를 바라보게
            transform.LookAt(player); 
        }
    }


}
