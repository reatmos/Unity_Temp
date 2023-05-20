using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyboardScript : MonoBehaviour
{
    private Rigidbody myRigid;
    public Camera theCamera;

    public float walkSpeed = 120f;
    public float lookSensitivity = 10f;
    public float cameraRotationLimit = 35f;
    private float currentCameraRotationX;
    public float jumpForce = 6f;
    private bool isGround = true;

    public int itemCount;
    public Text itemCountText;
    public float minHeight = -1f;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        itemCountText = GameObject.Find("itemCount").GetComponent<Text>();
    }

    void Update()
    {
        Move();
        Jump();
        CameraRotation();
        CharacterRotation();
        CheckFallDown();
        UpdateItemCountText();
    }

    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * walkSpeed;
        float zSpeed = zInput * walkSpeed;

        transform.Translate(Vector3.forward * zSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * xSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if(Input.GetKey(KeyCode.Space) && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    /*
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Item")
        {
            itemCount++;
            collider.gameObject.SetActive(false);
        }
    }
    */

    void CheckFallDown()
    {
        if(transform.position.y < minHeight)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateItemCountText()
    {
        itemCountText.text = "[itemCount] : " + itemCount.ToString();
    }
}
