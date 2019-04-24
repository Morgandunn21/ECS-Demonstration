using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ECS.ECS
{
    public class PlayerControllerECS : MonoBehaviour
    {
        [Header("Movement Attributes")]
        public float speed;
        public float baseSpeed;
        public float dashSpeed;
        public float dashCooldown;

        [Header("Other Attributes")]
        public float shotCooldown;
        public float DPS;
        public float m_refreshTime = 0.5f;
        public Text fpsCounterText;

        //private attributes
        private float horizontalInput;
        private float verticalInput;
        private int floorMask;
        private float camRayLength = 100f;
        private float currentShotCooldown;
        private float currentDashCooldown;
        int m_frameCounter = 0;
        float m_timeCounter = 0.0f;
        float m_lastFramerate = 0.0f;
        

        // Start is called before the first frame update
        void Awake()
        {
            floorMask = LayerMask.GetMask("floor");
            currentShotCooldown = 0;
            fpsCounterText.text = "FPS: " + (int)m_lastFramerate;
        }

        void Update()
        {
            if (m_timeCounter < m_refreshTime)
            {
                m_timeCounter += Time.deltaTime;
                m_frameCounter++;
            }
            else
            {
                //This code will break if you set your m_refreshTime to 0, which makes no sense.
                m_lastFramerate = (float)m_frameCounter / m_timeCounter;
                m_frameCounter = 0;
                m_timeCounter = 0.0f;
                fpsCounterText.text = "FPS: " + (int)m_lastFramerate;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            turning();
            moving();

            if (Input.GetButton("Fire1") && currentShotCooldown <= 0)
            {
                GameManager.GM.spawnBullet(transform.position, transform.rotation);
                currentShotCooldown = shotCooldown;
            }
            currentShotCooldown -= Time.fixedDeltaTime;

            if (Input.GetButton("Jump") && currentDashCooldown <= 0)
            {
                speed = dashSpeed;
                currentDashCooldown = dashCooldown;
            }
            currentDashCooldown--;

            if (speed > baseSpeed)
            {
                speed--;
            }
            else if (speed < baseSpeed)
            {
                speed = baseSpeed;
            }
        }

        public void hit()
        {
            gameObject.GetComponent<PlayerHealth>().TakeDamage(DPS * Time.fixedDeltaTime);
        }

        private void moving()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            Vector3 posChange = new Vector3(horizontalInput, 0, verticalInput);
            posChange.Normalize();
            posChange *= speed * Time.deltaTime;

            transform.position = transform.position + posChange;
        }

        private void turning()
        {
            //Create ray from mouse cursor on screen in the direction of the camera
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit floorHit;

            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {   //create vector from player to point on floor the raycast from the mouse hit
                Vector3 playerToMouse = floorHit.point - transform.position;

                playerToMouse.y = 0f;

                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                transform.rotation = newRotation;
            }
        }
    }
}