using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour 
{
    public GameObject target;
    public float padding = 0.5f;
    public float minDelay = 0.5f;
    public float maxDelay = 3f;
    public float levelDuration = 30f;

    private float leftClamp;
    private float rightClamp;
    private float downClamp;
    private float upClamp;
    private float curDelay = 5f;
    private float lastTargetDelay = 0;

	private void Awake()
	{
        levelDuration += curDelay;
	}

	private void Start()
	{
        SetClamps();
	}

	private void SetClamps()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distance));

        Vector3 size = target.GetComponent<Renderer>().bounds.size;

        float xPadding = size.x / 2;
        float yPadding = size.y / 2;

        leftClamp = bottomLeft.x + xPadding + padding;
        rightClamp = topRight.x - xPadding - padding;
        downClamp = bottomLeft.y + yPadding + padding;
        upClamp = topRight.y - yPadding - padding;
    }

	private void Update()
	{
        Spawn();
        EndScene();
	}

    private void Spawn()
    {
        curDelay -= Time.deltaTime;

        if (curDelay <= 0)
        {
            Vector3 position = Vector3.zero;
            position.x = Random.Range(leftClamp, rightClamp);
            position.y = Random.Range(downClamp, upClamp);

            GameObject targetInst = Instantiate(target, position, Quaternion.identity);
            lastTargetDelay = targetInst.GetComponent<Target>().LifeSpan();

            curDelay = Random.Range(minDelay, maxDelay);
        }
    }

    private void EndScene()
    {
        levelDuration -= Time.deltaTime;
        lastTargetDelay -= Time.deltaTime;

        if (levelDuration <= 0)
        {
            curDelay = float.PositiveInfinity;
            if (lastTargetDelay <= 0)
            {
                FindObjectOfType<SceneChanger>().LoadScene("Win");
            }
        }
    }
}
