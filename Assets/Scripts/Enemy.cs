using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship 
{
    [SerializeField] private float playerInRangeDistance = 7.5f;
    [SerializeField] private float playerOutOfRangeDistance = 15f;
    [SerializeField] private float deathScore = 0;

    private float playerInRangeSqrDistance => playerInRangeDistance * playerInRangeDistance;
    private float playerOutOfRangeSqrDistance => playerOutOfRangeDistance * playerOutOfRangeDistance;

    private Transform player;
    private Animator animator;

    protected override void Start()
	{
        base.Start();
        GameObject parent = GameObject.Find("Enemies") ?? new GameObject("Enemies");
        transform.parent = parent.transform;

        player = FindObjectOfType<Player>().transform;
        animator = GetComponent<Animator>();
	}

    protected override void Update()
    {
        base.Update();

        if(player)
        {
            float playerSqrDistance = (player.position - transform.position).sqrMagnitude;

            bool playerInRange = animator.GetBool("PlayerInRange");
            if(playerInRange)
            {
                if(playerSqrDistance > playerOutOfRangeSqrDistance)
                {
                    animator.SetBool("PlayerInRange", false);
                }
            }
            else
            {
                if(playerSqrDistance < playerInRangeSqrDistance)
                {
                    animator.SetBool("PlayerInRange", true);
                }
            }
        }
        else
        {
            animator.SetBool("PlayerInRange", false);
        }
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);

        if(health <= 0)
        {
            //FindObjectOfType<SceneChanger>().LoadScene("SuperWin");
            ScoreManager.Plus(deathScore);
        }
    }
}
