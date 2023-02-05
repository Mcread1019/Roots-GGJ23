using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{

    bool grounded = false;
    bool bounce = false;
    bool wet = false;
    public bool Debug = false;
    public bool resetSceneOnDeath = false;
    public GameObject camera;

    Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);
    public float velRest = -2.0f;
    public float speed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    public float bounceHeight = 1.0f;
    public float bounceHeight_ORG;

    public CharacterController charController;
    public MouseController mouseController;
    public Transform groundCheck;
    public Transform spawnPoint;
    public Transform crouchPoint;
    public Transform straightPoint;
    public float distanceFromGround = 0.4f;
    public LayerMask groundLayerMask;
    public LayerMask bounceLayerMask;
    public LayerMask waterLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        bounceHeight_ORG = bounceHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Debug)
            {
                print("Player: Quit Game");
            }
            Application.Quit();
        }
        if (Input.GetKeyDown("r"))
        {
            if (Debug)
            {
                print("Player: Reset Game");
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        grounded = Physics.CheckSphere(groundCheck.position, distanceFromGround, groundLayerMask);
        bounce = Physics.CheckSphere(groundCheck.position, distanceFromGround, bounceLayerMask);
        wet = Physics.CheckSphere(groundCheck.position, distanceFromGround, waterLayerMask);

        if (grounded && vel.y < 0.0f)
        {
            if (Debug)
            {
                print("Player: Grounded");
            }
            vel.y = velRest;
        }

        if (bounce && vel.y < 0.0f)
        {
            if (Debug)
            {
                print("Player: Bounce");
            }
            Jump(bounceHeight);
        }
        bounceHeight = bounceHeight_ORG;
        camera.transform.position = new Vector3(straightPoint.position.x, straightPoint.position.y, straightPoint.position.z);

        if (wet)
        {
            if (Debug)
            {
                print("Player: Wet");
            }
            if (resetSceneOnDeath)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                charController.enabled = false;
                transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
                transform.rotation = Quaternion.identity;

                mouseController.enabled = false;
                camera.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
                camera.transform.rotation = Quaternion.identity;

                mouseController.enabled = true;
                charController.enabled = true;

            }

        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        

        if (Input.GetButton("Crouch"))
        {
            if (Debug)
            {
                print("Player: Crouch");
            }
            Crouch();
            
        }

        if (Input.GetButton("Sprint"))
        {
            if (Debug)
            {
                print("Player: Sprint");
            }
            Move(moveHorizontal, moveVertical, sprintSpeed);
        }
        else
        {
            Move(moveHorizontal, moveVertical, speed);
        }


        if (Input.GetButtonDown("Jump") && grounded)
        {
            if (Debug)
            {
                print("Player: Jump");
            }
            Jump(jumpHeight);
        }

        vel.y += gravity * Time.deltaTime;
        charController.Move(vel * Time.deltaTime);
    }

    public void Move(float moveHorizontal_, float moveVertical_, float speed_)
    {
        if (Debug)
        {
            print("Player: Move");
        }
        charController.Move((transform.right * moveHorizontal_ + transform.forward * moveVertical_) * speed_ * Time.deltaTime);
    }

    public void Jump(float height_)
    {
        vel.y = Mathf.Sqrt(height_ * -2.0f * gravity);
    }

    public void Crouch()
    {
        camera.transform.position = new Vector3(crouchPoint.transform.position.x, crouchPoint.transform.position.y, crouchPoint.transform.position.z);
        vel.y += (gravity * 2) * Time.deltaTime;
        bounceHeight = bounceHeight * 2;
    }
}
