using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1f;

    private Vector3 offset;

    private Vector3 targetPos;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player!= null)
        {
            target = player.transform;
            offset = transform.position - target.position;
        }
        else
        {
            return;
        }
        
    }

    private void LateUpdate()
    {
        if (target == null) return;

        targetPos = target.localPosition + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
