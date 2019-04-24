using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float baseSpeed;
    public float dashSpeed;
    public GameObject bulletPrefab;
    public float shotCooldown;
    public float dashCooldown;
    public float DPS;


    private float horizontalInput;
    private float verticalInput;
    private int floorMask;
    private float camRayLength = 100f;
    private Rigidbody rb;
    private float currentShotCooldown;
    private float currentDashCooldown;
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    public Text fpsCounterText;

    // Start is called before the first frame update
    void Awake()
    {
        floorMask = LayerMask.GetMask("floor");

        rb = GetComponent<Rigidbody>();
        currentShotCooldown = 0;
        fpsCounterText.text = "FPS: " + (int) m_lastFramerate;
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
            fpsCounterText.text = "FPS: " + (int) m_lastFramerate;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rb.velocity != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
        }

        turning();
        moving();

        if (Input.GetButton("Fire1") && currentShotCooldown <= 0)
        {
            shoot();
        }
        currentShotCooldown -= Time.fixedDeltaTime;

        if( Input.GetButton("Jump") && currentDashCooldown <= 0)
        {
            speed = dashSpeed;
            currentDashCooldown = dashCooldown;
        }
        currentDashCooldown --;

        if(speed > baseSpeed)
        {
            speed--;
        } else if (speed < baseSpeed)
        {
            speed = baseSpeed;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            gameObject.GetComponent<PlayerHealth>().TakeDamage(DPS * Time.fixedDeltaTime);
        }
    }

    private void shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        currentShotCooldown = shotCooldown;
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

            rb.MoveRotation(newRotation);
        }
    }
}
