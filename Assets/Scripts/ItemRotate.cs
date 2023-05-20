using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float rotateSpeed = 30;
    public float floatSpeed = 2.5f;
    public float floatHeight = 0.3f;

    private Vector3 startPos;
    private Quaternion startRot;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 회전 코드
        transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime, Space.World);

        // 움직임 코드
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight + startPos.y;
        Vector3 newPosition = new Vector3(startPos.x, newY, startPos.z);
        transform.position = newPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyboardScript player = other.GetComponent<KeyboardScript>();
            player.itemCount++;
            gameObject.SetActive(false);
        }
    }
}
