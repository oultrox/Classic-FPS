using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBillboard : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AnimationClip[] anims;
    [SerializeField] private bool isAnimated;

    [SerializeField] private float initialAngle = 0f;
    private Animator anim;
    private SpriteRenderer sr;
    private float angle;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        angle = GetAngle();
        
        if (angle >= 225 &&  angle <= 315)
        {
            ChangeSprite(0);
        }
        else if (angle < 225 && angle > 135)
        {
            ChangeSprite(1);
        }
        else if (angle <= 135 && angle >= 45)
        {
            ChangeSprite(2);
        }
        else if ((angle < 45 && angle > 0) || (angle > 315 && angle < 360))
        {
            ChangeSprite(3);
        }
    }

    private float GetAngle()
    {
        Vector3 direction = Camera.main.transform.position - this.transform.position;
        float angleTemp = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        angleTemp += initialAngle % 360;
        if (angleTemp < 0)
        {
            angleTemp += 360;
        }
        return angleTemp;
    }

    void ChangeSprite(int index)
    {
        if (isAnimated)
        {
            anim.Play(anims[index].name);
        }
        else
        {
            sr.sprite = sprites[index];
        }
    }

    
}
