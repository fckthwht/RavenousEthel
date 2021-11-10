using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : AliveCreature
{
    public override void Death()
    {
        GameManager.Instance.playerHealth.GetBigger();

        animator.SetTrigger("death");
        Destroy(gameObject);
    }
}
