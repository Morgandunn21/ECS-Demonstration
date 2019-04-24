using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float directionChangeCooldown;

    public float deathDelay;

    private bool move = true;
    private float currentCooldown;
    private Vector3 randDir;
    private Vector3 forward;
    private GameObject target;
    private EnemySpawner es;

    private void Awake()
    {
        randDir = Vector3.zero;
        forward = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move)
        {
            transform.LookAt(target.GetComponent<Transform>().position);
            if (currentCooldown <= 0)
            {
                randDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(.5f, 2f));
                currentCooldown = directionChangeCooldown;
            }
            forward = new Vector3(0, 0, 1);
            transform.Translate((forward + randDir) * Time.fixedDeltaTime * speed);
            currentCooldown -= Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider bullet)
    {
        Debug.Log("test");
        die();
    }

    private void die()
    {
        transform.Rotate(-90, 0, 0);
        move = false;
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<Rigidbody>());
        StartCoroutine(wait());
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(deathDelay);
        es.enemyDied(transform.position);
        Destroy(gameObject);
    }

    public void setTarget(GameObject t)
    {
        target = t;
    }

    public void setSpawnner(EnemySpawner s)
    {
        es = s;
    }
}
