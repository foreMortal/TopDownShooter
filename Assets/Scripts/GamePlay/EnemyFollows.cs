using System;
using UnityEngine;

public class EnemyFollows : MonoBehaviour
{
    [NonSerialized] public Transform player;
    [NonSerialized] public float speed;
    [NonSerialized] public bool Active = false;

    private void Update()
    {
        if(Active)
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
