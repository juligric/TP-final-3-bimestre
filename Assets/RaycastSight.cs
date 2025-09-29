using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSight : MonoBehaviour
{
    [SerializeField] Transform originTR; // Empty arriba de la cápsula
    [SerializeField] float rayLength = 10f; // Distancia de visión

    [HideInInspector] public bool canSeePlayer;
    [HideInInspector] public Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 direction = (player.position - originTR.position).normalized;
        RaycastHit hitInfo;

        if (Physics.Raycast(originTR.position, direction, out hitInfo, rayLength))
        {
            canSeePlayer = hitInfo.collider.CompareTag("Player");
            Debug.DrawLine(originTR.position, hitInfo.point, Color.red); // línea de debug
        }
        else
        {
            canSeePlayer = false;
            Debug.DrawLine(originTR.position, originTR.position + direction * rayLength, Color.green); // no vio nada
        }
    }
}



