using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DynamicBillboard : MonoBehaviour
{
    private Animator anim;
    private Transform vision;
    private float dotProduct;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        vision = GetComponentInChildren<EnemyVision>().transform;
    }

    private void Update()
    {
        GetAngle();
    }

    private void GetAngle()
    {
        Vector3 playerDir = Camera.main.transform.forward;
        playerDir.y = 0;

        Vector3 enemyDir = vision.forward;
        enemyDir.y = 0;

        dotProduct = Vector3.Dot(playerDir, enemyDir);
        if ((dotProduct < -0.5f && dotProduct >= -1) || (dotProduct > 0.5f && dotProduct <= 1))
        {
            anim.SetFloat("dotProduct", dotProduct);
            anim.SetBool("isMovingSides", false);
        }
        else
        {
            anim.SetFloat("dotProduct", 0);
            anim.SetBool("isMovingSides", true);
            Vector3 playerRight = Camera.main.transform.right;
            playerRight.y = 0;

            dotProduct = Vector3.Dot(playerRight, enemyDir);
            anim.SetFloat("dotProductSides", dotProduct);
        }
        
    }
}
