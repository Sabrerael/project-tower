using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Transform target;
    private bool skipMove;

    // Start is called before the first frame update
    protected override void Start()
    {
        //GameManager.instance.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if(skipMove) {
            skipMove = false;
            return;
        }
        
        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    protected override void OnCantMove<T>(T component)
    {
        //Player hitPlayer = Component as Player;
        //hitPlayer.LoseFood(playerDamage);
        //animator.SetTrigger("enemyAttack:);
    }
}
