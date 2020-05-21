using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Target : MonoBehaviour 
{
    public float growthRate = 0.1f;
    public float fadeSize = 0.2f;
    public float fadeTime = 1f;
    public int score = 1000;
    public AudioClip onDestroySound;

    private GameObject parent;
    private SpriteRenderer sprite;
    private float alpha = 1;
    private int curScore;

	private void Awake()
	{
        Vector3 scale = transform.localScale;
        scale.x = 0;
        scale.y = 0;
        scale.z = 0;

        transform.localScale = scale;
        curScore = score;
	}

	private void Start()
	{
        parent = GameObject.Find("Target Spawner") ?? new GameObject("Target Spawner");
        transform.parent = parent.transform;
        sprite = GetComponent<SpriteRenderer>();
        alpha = sprite.color.a;
	}

	private void Update()
	{
        Grow();
        FadeOut();
	}

    private void Grow()
    {
        float growthRate = this.growthRate * Time.deltaTime;

        Vector3 scale = transform.localScale;
        scale.x += growthRate;
        scale.y += growthRate;

        transform.localScale = scale;
        curScore = Mathf.RoundToInt((score * (1 - scale.x * 2)) / 10) * 10;
    }

	private void FadeOut()
    {
        if (transform.localScale.x > fadeSize)
        {
            alpha -= Time.deltaTime / fadeTime;
            alpha = Mathf.Clamp01(alpha);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

            if (alpha <= 0) Destroy(gameObject);
        }
    }

    public float LifeSpan()
    {
        return fadeSize / growthRate + fadeTime;
    }

    public void Hit()
    {
        ScoreManager.Plus(curScore);
        PlayClipAtPoint(onDestroySound, transform.position, false);
        Destroy(gameObject);
    }

    private AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, bool sound3D = true)
    {
        GameObject tempObj = new GameObject("TempAudio");
        tempObj.transform.position = pos;
        AudioSource aSource = tempObj.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.spatialBlend = (sound3D) ? 1 : 0;
        aSource.Play();
        Destroy(tempObj, clip.length);
        return aSource;
    }
}
