using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float maxLifeTime;
    private Bounds bounds;
    private CircleCollider2D bulletCollider;
    private Vector2 lookDirection;
    private float lookAngle;
    private float lifeTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy _;
        if (other.gameObject.TryGetComponent(out _))
        {
            if (other.bounds.Intersects(bulletCollider.bounds))
            {
                if (Spawner.instance.RunScriptWithObjectPool)
                {
                    this.gameObject.SetActive(false);
                    other.gameObject.SetActive(false);
                }
                else
                {
                    Destroy(other.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        bulletCollider = GetComponent<CircleCollider2D>();
        bulletCollider.transform.localScale = transform.localScale;

        lifeTime = 0;

        ComputeDirection();
    }

    private void OnEnable()
    {
        lifeTime = 0;
        ComputeDirection();
    }

    private void ComputeDirection()
    {
        //Computing a direction by substracting the mouse position in screen space from the transform position of the bullet
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //Get the counter clock-wise angle of the shot and convert it to degrees
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        //Rotating the bullet into the direction that it is going
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawner.instance.RunScriptWithObjectPool)
        {
            RunWithObjectPool();
        }
        else
        {
            RunWithoutObjectPool();
        }
    }
    private void RunWithObjectPool()
    {
        transform.position += transform.rotation * Vector3.right * bulletSpeed * Time.deltaTime;

        lifeTime += Time.deltaTime;

        if (lifeTime >= maxLifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void RunWithoutObjectPool()
    {
        transform.position += transform.rotation * Vector3.right * bulletSpeed * Time.deltaTime;

        lifeTime += Time.deltaTime;

        if (lifeTime >= maxLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
