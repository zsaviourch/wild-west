using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyFlowerAIController : AIController
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        //AudioManager.Instance.Play("golemHurt");
        if (isDead)
        {
            Destroy(gameObject);
            //AudioManager.Instance.Play("golemDie");
        }
    }
}

