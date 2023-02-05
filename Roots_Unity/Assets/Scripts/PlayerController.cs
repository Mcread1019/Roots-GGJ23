using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int crumb;
    public TextMeshProUGUI crumbText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        crumbText.text = "Crumbs: " + crumb.ToString(); 
    }

    void FixedUpdate()
    {

    }

    public void AddCrumb(int crumb_)
    {
        crumb = crumb + crumb_;
    }

    public void RemoveCrumb(int crumb_)
    {
        crumb -= crumb_;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Crumb")
        {
            Destroy(hit.gameObject);
            AddCrumb(1);
        }
    }
}
