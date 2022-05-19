using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject endScreenPanel;
    public int objectPoolSize;
    public float fireRate;
    private CircleCollider2D playerCollider;
    private float counter;


    #region Object Pool

    [HideInInspector] public static Dictionary<System.Int32, GameObject> bulletObjectPool;
    #endregion

    private void Awake()
    {
        playerCollider = gameObject.GetComponent<CircleCollider2D>();
        counter = fireRate;
        if (Spawner.instance.RunScriptWithObjectPool)
        {
            InitObjectPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy _;
        if (other.gameObject.TryGetComponent(out _))
        {
            if (other.bounds.Intersects(playerCollider.bounds))
            {
                endScreenPanel.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    private void InitObjectPool()
    {
        //Initializing with capacity = size;
        bulletObjectPool = new Dictionary<System.Int32, GameObject>(objectPoolSize);

        //Leverage the instance ID that is assigned autmatically to all gameObjects
        for (int i = 0; i < objectPoolSize; i++)
        {
            GameObject bullet = IstantiateBullet();
            bullet.SetActive(false);
            bulletObjectPool.Add(bullet.GetInstanceID(), bullet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawner.instance.RunScriptWithObjectPool)
        {
            WithObjectPool();
        }
        else
        {
            WithoutObjectPool();
        }
    }

    private void WithObjectPool()
    {
        counter--;

        if (Input.GetMouseButton(0))
        {

            if (counter <= 0)
            {
                AttackWithObjectPool();
                counter = fireRate;
            }

        }
    }

    private void WithoutObjectPool()
    {
        counter--;

        if (Input.GetMouseButton(0))
        {

            if (counter <= 0)
            {
                Attack();
                counter = fireRate;
            }

        }
    }

    private void AttackWithObjectPool()
    {
        System.Int32 key = -1;

        key = bulletObjectPool.Where(item => item.Value.gameObject.activeSelf == false).FirstOrDefault().Key;

        GameObject bullet;
        if (bulletObjectPool.TryGetValue(key, out bullet))
        {
            bullet = bulletObjectPool[key];
            bullet.transform.position = gameObject.transform.position;
            bullet.SetActive(true);
        }
    }

    private void Attack() => Instantiate(bullet, transform.position, Quaternion.identity);
    private GameObject IstantiateBullet() => Instantiate(bullet, transform.position, Quaternion.identity);
}
