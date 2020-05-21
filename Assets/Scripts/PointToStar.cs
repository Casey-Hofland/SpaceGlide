using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToStar : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform star;

    private void Update()
    {
        if(player && star)
        {
            var direction = star.position - player.position;

            transform.up = -direction;
        }
    }
}
