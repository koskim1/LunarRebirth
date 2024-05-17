using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.FilePathAttribute;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;
    public bool isPc;

    Animator animator;
    

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnJoysickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPc)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);

            if(Physics.Raycast(ray, out hit))
            {
                rotationTarget = hit.point;
            }

            movePlayerWithAim();
        }
        else
        {
            if(joystickLook.x == 0 && joystickLook.y == 0)
            {
                movePlayer();
            }
            else
            {
                movePlayerWithAim();
            }
        }
    }
    // Controller or JoyStick Movement (기존 pc는 movePlayerWithAim()로 수정
    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.6f);
        }

        transform.Translate(movement*speed*Time.deltaTime, Space.World);
    }

    public void movePlayerWithAim()
    {
        if (isPc)
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0f;
            if(lookPos != Vector3.zero)
            {
               var rotation = Quaternion.LookRotation(lookPos);

               Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

               if (aimDirection != Vector3.zero)
               {
                   transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.6f);
               }
            }
        }
        // If Controller or Mobile
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);
            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.6f);
            }
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // TODO
    // 공격범위 내의 피격시스템
    // Health 스크립트 관리 ( 하기전에 그 Code Monkey에서 했던 기법 한번 연구하기 )
    // 대화시스템 관리

}
