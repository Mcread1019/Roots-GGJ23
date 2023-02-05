using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MovementController : MonoBehaviour
{

    bool grounded = false;
    bool bounce = false;
    bool bounceLow = false;
    bool wet = false;
    public bool Debug = false;
    public bool title = true;
    public bool tut = false;
    public bool credits = false;
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
    public float bounceLowHeight = 1.0f;
    public float bounceLowHeight_ORG;

    public CharacterController charController;
    public MouseController mouseController;
    public Transform groundCheck;
    public Transform spawnPoint;
    public Transform crouchPoint;
    public Transform straightPoint;
    public float distanceFromGround = 0.4f;
    public LayerMask groundLayerMask;
    public LayerMask bounceLayerMask;
    public LayerMask bounceLowLayerMask;
    public LayerMask waterLayerMask;

    bool newTitles = false;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI titleTutText;
    public TextMeshProUGUI titleCredText;
    public TextMeshProUGUI tutText1;
    public TextMeshProUGUI tutText2;
    public TextMeshProUGUI tutText3;
    public TextMeshProUGUI tutText4;
    public TextMeshProUGUI credText1;
    public TextMeshProUGUI credText2;
    public TextMeshProUGUI credText3;
    public TextMeshProUGUI credText4;
    public TextMeshProUGUI infoText;

    // Start is called before the first frame update
    void Start()
    {
        bounceHeight_ORG = bounceHeight;
        bounceLowHeight_ORG = bounceLowHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (title)
        {
            titleText.enabled = true;
            infoText.enabled = true;
            newTitles = false;
        }
        else
        {
            titleText.enabled = false;
            infoText.enabled = false;
            newTitles = true;
        }

        if (tut)
        {
            if (newTitles)
            {
                titleTutText.enabled = true;
            }
            tutText1.enabled = true;
            tutText2.enabled = true;
            tutText3.enabled = true;
            tutText4.enabled = true;
        }
        else
        {
            titleTutText.enabled = false;
            tutText1.enabled = false;
            tutText2.enabled = false;
            tutText3.enabled = false;
            tutText4.enabled = false;
        }

        if (credits)
        {
            if (newTitles)
            {
                titleCredText.enabled = true;
            }
            credText1.enabled = true;
            credText2.enabled = true;
            credText3.enabled = true;
            credText4.enabled = true;
        }
        else
        {
            titleCredText.enabled = false;
            credText1.enabled = false;
            credText2.enabled = false;
            credText3.enabled = false;
            credText4.enabled = false;
        }

        grounded = Physics.CheckSphere(groundCheck.position, distanceFromGround, groundLayerMask);
        bounce = Physics.CheckSphere(groundCheck.position, distanceFromGround, bounceLayerMask);
        bounceLow = Physics.CheckSphere(groundCheck.position, distanceFromGround, bounceLowLayerMask);
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

        if (bounceLow && vel.y < 0.0f)
        {
            if (Debug)
            {
                print("Player: Bounce");
            }
            Jump(bounceLowHeight);
        }
        bounceLowHeight = bounceLowHeight_ORG;

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

        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d") || Input.GetKeyDown("space") || Input.GetKeyDown("left ctrl"))
        {
            title = false;
        }

        if (Input.GetKeyDown("t"))
        {
            if(tut)
            {
                tut = false;
            }
            else
            {
                tut = true;
                credits = false;
            }
        }

        if (Input.GetKeyDown("c"))
        {
            if (credits)
            {
                credits = false;
            }
            else
            {
                credits = true;
                tut = false;
            }
        }

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
        bounceLowHeight = bounceLowHeight * 2;
    }
}
