using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player)
        {
            FindObjectOfType<SceneChanger>().LoadScene("SuperWin");
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(collectedClip, transform.position);
        }
    }
}
