using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseBehaviour : StateMachineBehaviour
{
    [SerializeField] private float flickAngle = 150f;
    [SerializeField] private float shootAngle = 30f;

    private Enemy enemy;
    private Player player;
    private float fireRate;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        player = FindObjectOfType<Player>();

        fireRate = enemy.laser.GetComponent<Laser>().fireRate;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.Accelerate();
        enemy.ApplyAcceleration();

        if(player)
        {
            Vector2 directionToPlayer = player.transform.position - enemy.transform.position;
            directionToPlayer.Normalize();
            float angleToPlayer = Vector2.SignedAngle(directionToPlayer, enemy.transform.up);

            if(angleToPlayer > float.Epsilon)
            {
                enemy.TurnRight();
            }
            else if(angleToPlayer < -float.Epsilon)
            {
                enemy.TurnLeft();
            }

            if(Mathf.Abs(angleToPlayer) <= shootAngle)
            {
                if(!enemy.IsInvoking("Shoot"))
                {
                    enemy.InvokeRepeating("Shoot", 0.00001f, fireRate);
                }
            }
            else
            {
                enemy.CancelInvoke("Shoot");
            }

            if(Mathf.Abs(angleToPlayer) >= flickAngle)
            {
                animator.SetTrigger("Flick");
            }
        }
        else
        {
            enemy.CancelInvoke("Shoot");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.CancelInvoke("Shoot"); 
    }
}
