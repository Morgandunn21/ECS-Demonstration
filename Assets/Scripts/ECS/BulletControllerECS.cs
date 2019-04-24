using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.ECS
{
    public class BulletEntity : MonoBehaviour
    {
        public float speed;
        public float lifetime;
        public GameManager gm;

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

        private void OnCollisionEnter(Collision collision)
        {
            GameObject collider = collision.gameObject;

            if(collider.tag.Equals("Enemy"))
            {
                Vector3 pos = collider.transform.position;
                Destroy(collider);
                gm.enemyDied(pos);
            }
        }
    }
}