using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DynamicBillboard : MonoBehaviour
{
    [SerializeField] private AnimationClip[] anims;

    private Animator anim;
    private Transform vision;

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

        float dotProduct = Vector3.Dot(playerDir, enemyDir);
        if (dotProduct < -0.5f && dotProduct >= -1)
        {
            ChangeSprite(0);
        }
        else if (dotProduct > 0.5f && dotProduct <= 1)
        {
            ChangeSprite(1);
        }
        else
        {
            Vector3 playerRight = Camera.main.transform.right;
            playerRight.y = 0;

            dotProduct = Vector3.Dot(playerRight, enemyDir);
            if (dotProduct >= 0)
            {
                ChangeSprite(2);
            }
            else
            {
                ChangeSprite(3);
            }
        }

    }

    void ChangeSprite(int index)
    {
         anim.Play(anims[index].name);
    }
}
