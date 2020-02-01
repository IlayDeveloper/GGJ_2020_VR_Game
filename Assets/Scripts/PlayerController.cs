using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public bool controllable = false;
    [SerializeField] private float mouseSensitivity = 100.0f;
    [SerializeField] private float clampAngle = 80.0f;
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float jumpSpeed = 6.0f;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private float raycastDistance=1.0f;
    [SerializeField] private ProgressBarCircle progressBar;
    [SerializeField] private TextMeshProUGUI text;
    
    private Ray ray;
    private RaycastHit hit;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    private float rotY = 0.0f;
    private float rotX = 0.0f; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        hit = new RaycastHit();
    }

    void Update()
    {
        Move();
        MouseLook();
        Construction construction = FindConstruction();
        if (construction != null)
        {
            text.gameObject.SetActive(true);


            if (Input.GetKey(KeyCode.E))
            {
                text.gameObject.SetActive(false);
                float durability = construction.Break();
                progressBar.gameObject.SetActive(true);
                progressBar.BarValue = (int)(durability * 100);

            }
            else
            {
                progressBar.gameObject.SetActive(false);
            }
        }
        else
        {
            text.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
        }
        

    }

    private void Move()
    {
        if (controller.isGrounded && controllable)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    private Construction FindConstruction()
    {
        ray = new Ray(transform.position, transform.forward);

        if(Physics.Raycast(ray, out hit, raycastDistance))
        {
            Construction construction = hit.transform.GetComponent<Construction>();
            if (construction != null)
            {
                return construction;
            }
            
        }

        return null;
    }

}