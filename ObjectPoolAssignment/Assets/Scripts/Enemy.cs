using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float speed;

    private Vector3 currentPosition;
    private CircleCollider2D enemyCollider;

    private void Start()
    {
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyCollider.transform.localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
