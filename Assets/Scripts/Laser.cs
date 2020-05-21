using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Laser : MonoBehaviour 
{
    public float damage;
    public float speed;
    public float fireRate;
    public AudioClip sound;
    public GameObject explosion;

    private GameObject parent;
    private Rigidbody2D rigidbody;

	private void Start()
	{
        parent = GameObject.Find("Lasers") ?? new GameObject("Lasers");
        transform.parent = parent.transform;
        rigidbody = GetComponent<Rigidbody2D>();

        Vector3 pos = transform.position;
        pos.z += 0.1f;
        transform.position = pos;
        rigidbody.velocity = (transform.up * speed);

        pos.z -= 5f;

        PlayClipAtPoint(sound, transform.position, false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        Target target = collision.gameObject.GetComponent<Target>();
        if (target) target.Hit();

        Ship ship = collision.gameObject.GetComponent<Ship>();
        if (ship) ship.Damage(damage);

        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
	}

	private void OnBecameInvisible()
	{
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
