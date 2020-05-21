using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public abstract class Ship : MonoBehaviour 
{
    public bool invinsible = false;
    public bool clampToViewport = false;
    public float health;
    public GameObject laser;

    public float maxSpeed = 5f;
    public float accelerationRate = 10f;
    public float grindDelay = 1f;
    public float turnRate = 180f;

    protected float acceleration;
    protected Vector2 curDirection = Vector2.zero;
    protected float curGrindDelay;
    protected float curTurnRate;

    protected float leftClamp = float.NegativeInfinity;
    protected float rightClamp = float.PositiveInfinity;
    protected float downClamp = float.NegativeInfinity;
    protected float upClamp = float.PositiveInfinity;
    private bool clamped = false;

    protected Rigidbody2D rigidbody;

    protected virtual void Awake()
    {
        DeadCheck();
    }

    protected virtual void Start()
	{
        rigidbody = GetComponent<Rigidbody2D>();
	}

    private void SetClamps()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));

        Vector3 size = GetComponent<Renderer>().bounds.size;

        float xPadding = size.x / 2;
        float yPadding = size.y / 2;

        leftClamp = bottomLeft.x + xPadding;
        rightClamp = topRight.x - xPadding;
        downClamp = bottomLeft.y + yPadding;
        upClamp = topRight.y - yPadding;
    }

    protected virtual void Update()
    {
        if(clampToViewport)
        {
            if(!clamped)
            {
                SetClamps();
                clamped = true;
            }
        }
        else if(clamped)
        {
            leftClamp = float.NegativeInfinity;
            rightClamp = float.PositiveInfinity;
            downClamp = float.NegativeInfinity;
            upClamp = float.PositiveInfinity;
            clamped = false;
        }

        ApplyAcceleration();
    }

    public void Accelerate()
    {
        acceleration += accelerationRate * Time.deltaTime;
        curDirection = transform.up;
        curTurnRate = turnRate / 2;
        curGrindDelay = grindDelay;
    }

    public void Break()
    {
        acceleration -= accelerationRate * Time.deltaTime;
        curTurnRate = turnRate * 2;
        curGrindDelay = grindDelay;
    }

    public void Decelerate()
    {
        curTurnRate = turnRate;
        curGrindDelay -= Time.deltaTime;
        curGrindDelay = Mathf.Min(0, curGrindDelay);
        if(curGrindDelay <= 0) acceleration -= accelerationRate * Time.deltaTime;
    }

    public void ApplyAcceleration()
    {
        acceleration = Mathf.Clamp(acceleration, 0, maxSpeed);
        rigidbody.velocity = curDirection * acceleration;

        Vector3 position = transform.position;
        float xClamped = Mathf.Clamp(position.x, leftClamp, rightClamp);
        float yClamped = Mathf.Clamp(position.y, downClamp, upClamp);
        transform.position = new Vector3(xClamped, yClamped, position.z);
    }

    public void TurnLeft()
    {
        transform.Rotate(Vector3.forward * curTurnRate * Time.deltaTime);
    }

    public void TurnRight()
    {
        transform.Rotate(Vector3.back * curTurnRate * Time.deltaTime);
    }

    private bool DeadCheck()
    {
        if (health > 0) return false;

        Destroy(gameObject);
        return true;
    }

    public virtual void Damage(float amount)
    {
        if (invinsible) return;

        health -= (amount <= health) ? amount : health;
        DeadCheck();
    }

    public virtual void Shoot()
    {
        Instantiate(laser, transform.position, transform.rotation);
    }
}
