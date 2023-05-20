using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController SelectPlayer;
    public float Speed;
    public float JumpPow;

    private float Gravity;
    private Vector3 MoveDir;
    private bool JumpButtonPressed;

    public float ForceMultiplier; // 공에 가해지는 힘의 크기를 조절하는 값
    public float JumpForceMultiplier; // 공과 부딪혔을 때 캐릭터가 뛰는 높이를 조절하는 값

    void Start()
    {
        Speed = 5.0f;
        Gravity = 10.0f;
        MoveDir = Vector3.zero;
        JumpPow = 5.0f;
        JumpButtonPressed = false;

        ForceMultiplier = 10.0f;
        JumpForceMultiplier = 2.0f;
    }

    void Update()
    {
        if (SelectPlayer == null) return;

        if (SelectPlayer.isGrounded)
        {
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            MoveDir = SelectPlayer.transform.TransformDirection(MoveDir);
            MoveDir *= Speed;

            if (JumpButtonPressed == false && Input.GetButton("Jump"))
            {
                JumpButtonPressed = true;
                MoveDir.y = JumpPow;
            }
        }
        else
        {
            MoveDir.y -= Gravity * Time.deltaTime;
        }

        if (!Input.GetButton("Jump"))
        {
            JumpButtonPressed = false;
        }

        // 점프 중 방향 전환
        if (JumpButtonPressed && !SelectPlayer.isGrounded)
        {
            MoveDir.x = Input.GetAxis("Horizontal") * Speed;
            MoveDir.z = Input.GetAxis("Vertical") * Speed;
        }

        SelectPlayer.Move(MoveDir * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            // 공의 Rigidbody 컴포넌트 가져오기
            Rigidbody ballRigidbody = col.gameObject.GetComponent<Rigidbody>();

            // 공의 Rigidbody 컴포넌트의 Is Kinematic 속성 꺼주기
            ballRigidbody.isKinematic = false;

            // 충돌 지점의 법선 벡터 구하기
            Vector3 normal = col.contacts[0].normal;

            // 공이 굴러가도록 힘을 가해주기
            ballRigidbody.AddForce(normal * ForceMultiplier, ForceMode.Impulse);

            // 공과 부딪혔을 때 캐릭터가 뛰지 않도록 수정
            MoveDir.y = 0f;
        }
    }
}
