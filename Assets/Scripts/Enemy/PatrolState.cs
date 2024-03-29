﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyStateBase
{
    // Start is called before the first frame update
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.SwitchPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {

        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            enemy.animState = 1;
            enemy.MoveTotarget();
        }


        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.5f)
        {
            enemy.TransitionToState(enemy.patrolState);
        }


        if (enemy.attackList.Count > 0)
            enemy.TransitionToState(enemy.attackState);

    }
}
