using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ship 
{
	protected override void Update()
	{
        ProcessAccelerate();
        ProcessTurn();
        ProcessShooting();

        invokeTime = Mathf.Max(invokeTime - Time.deltaTime, 0f);
	}

    private void ProcessAccelerate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Accelerate();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Break();
        }
        else
        {
            Decelerate();
        }

        ApplyAcceleration();
    }

    private void ProcessTurn()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TurnLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            TurnRight();
        }
    }

    private void ProcessShooting()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            float fireRate = laser.GetComponent<Laser>().fireRate;
            InvokeRepeating("Shoot", invokeTime, fireRate);
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            CancelInvoke("Shoot");
        }
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);

        if(health <= 0)
        {
            FindObjectOfType<SceneChanger>().LoadScene("Menu");
        }
    }

    private float invokeTime = 0f;
    public override void Shoot()
    {
        base.Shoot();
        invokeTime = laser.GetComponent<Laser>().fireRate;
    }
}
