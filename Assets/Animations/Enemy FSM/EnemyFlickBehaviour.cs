using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlickBehaviour : StateMachineBehaviour 
{
    private Enemy enemy;
	private Player player;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy = animator.GetComponent<Enemy>();
        player = FindObjectOfType<Player>();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy.Break();
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
        }
    }
}
