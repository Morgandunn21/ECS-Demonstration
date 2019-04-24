using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float lifetime;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
        lifetime -= Time.fixedDeltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}
