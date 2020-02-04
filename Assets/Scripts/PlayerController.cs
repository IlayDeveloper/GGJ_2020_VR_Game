using UnityEngine;
using System.Collections;
using TMPro;
using System;
using Random = System.Random;

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
    [SerializeField] private TextMeshProUGUI textSpawn;
    [SerializeField] private float m_RespawnTime;
    [SerializeField] private Transform[] m_SpawnPoints;
    
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
        textSpawn.gameObject.SetActive(false);
        Respawn();
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

    public void Respawn()
    {
        controllable = false;
        StartCoroutine(WaitSpawn(m_RespawnTime));
    }

    public void GameOver()
    {
        controllable = false;
        textSpawn.gameObject.SetActive(true);
        textSpawn.SetText("You lose!");
        //StartCoroutine(WaitSpawn(10));
    }

    public void GameWin()
    {
        controllable = false;
        textSpawn.gameObject.SetActive(true);
        textSpawn.SetText("You Win!");
        //StartCoroutine(WaitSpawn(10));
    }

    IEnumerator WaitSpawn(float timeSpawn)
    {
        float timer = Time.time;
        transform.position = m_SpawnPoints [(int)UnityEngine.Random.Range(0, m_SpawnPoints.Length - 1)].position;
        textSpawn.gameObject.SetActive(true);
        while ( (Time.time - timer) < m_RespawnTime)
        {
            textSpawn.SetText("Game start in " + (m_RespawnTime - (int)(Time.time - timer)).ToString());
            yield return null;
        }

        controllable = true;
        textSpawn.gameObject.SetActive(false);
    }
    
    

}