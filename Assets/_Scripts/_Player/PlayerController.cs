using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public bool isPc = true;
    public bool canMove = true;
    private bool _isInDialogue = false;
    private Vector2 move, mouseLook, joystickLook;
    private Vector3 rotationTarget;

    public bool _canSlowDash = false;
    private bool _isDashing = false;
    [SerializeField]private bool _canDash = true;
    private bool _lockOn = false;

    private float _originalSpeed;
    [SerializeField]private float _speed = 8f;
    [SerializeField]private float _dashSpeed = 20f;
    private float _dashDuration = 0.15f;
    private float _slowDuration = 0.35f;
    private float _slowTimeScale = 0.18f;
    private float _dashCoolDown = 1.3f;
    [SerializeField] private AnimationCurve dashCurve; // 대쉬 속도 제어 애니메이션    

    private Transform cameraTransform;
    private LockOn lockOn;
    private Animator playerAnimator;

    public GameObject minimap;    

    // Start is called before the first frame update
    void Awake()
    {        
        isPc = true;
        canMove = false;
        _originalSpeed = _speed;        
        cameraTransform = Camera.main.transform;

        lockOn = GetComponent<LockOn>();
        playerAnimator = GetComponent<Animator>();        
    }
    private void Start()
    {
        GameManager.Instance.SetPlayerController(this);
    }

    private void OnEnable()
    {
        _canDash = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 대화 중이면 입력 무시
        if (_isInDialogue) return;

        move = context.ReadValue<Vector2>();
        playerAnimator.SetFloat("x", move.x);
        playerAnimator.SetFloat("y", move.y);
    }
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }
    public void OnJoysickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        // 대화 중이면 대쉬 입력 무시
        if (_isInDialogue) return;

        if (context.performed && !_isDashing && _canDash)
        {
            StartCoroutine(Dash());
        }
        _isDashing = false;
    }

    public void LockOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _lockOn = !_lockOn;

            if (_lockOn)
            {
                lockOn.ResetTarget();
                lockOn.FindLockOnTarget();
            }
            else
            {
                lockOn.ResetTarget();
            }
        }
    }

    private IEnumerator Dash()
    {
        _isDashing = true;
        _canDash = false;
        playerAnimator.SetBool("isDash", true);
        float elapsedTime = 0f;

        while (elapsedTime < _dashDuration)
        {
            float t = elapsedTime / _dashDuration;
            _speed = Mathf.Lerp(_dashSpeed, _originalSpeed, dashCurve.Evaluate(t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_canSlowDash)
        {
            StartCoroutine(SlowTime());
        }
        _speed = _originalSpeed;

        playerAnimator.SetBool("isDash", false);
        yield return new WaitForSeconds(_dashCoolDown);
        _canDash = true;
        
        //_speed = _dashSpeed;
        //yield return new WaitForSeconds(_dashDuration);
        //_speed = _originalSpeed;

    }

    public void CanSlowDash()
    {
        _canSlowDash = true;
        //DataManager.Instance.CanSlowDash = true;
        //DataManager.Instance.SaveData();
    }
    private IEnumerator SlowTime()
    {
        Time.timeScale = _slowTimeScale;
        yield return new WaitForSecondsRealtime(_slowDuration);
        Time.timeScale = 1f;
    }

    // 대화 상태 설정 메서드 추가
    public void SetDialogueState(bool isInDialogue)
    {
        _isInDialogue = isInDialogue;
    }
    // ----------------------------------------------------------------------------------------------

    public void AddSpeed(float amount)
    {
        _speed += amount;
        _originalSpeed = _speed;
    }

    // ----------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    삭제해야함 테스트
        //    PlayerAttributesManager.Instance.LevelUp();
        //    PlayerAttributesManager.Instance.UpdateXPUI();
        //}

        if (isPc && canMove && !PauseMenu.isPaused)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);
            if (Physics.Raycast(ray, out hit))
            {
                rotationTarget = hit.point;
            }
            movePlayerWithAim();
        }
        else
        {
            if (joystickLook.x == 0 && joystickLook.y == 0)
            {
                //if (canMove) movePlayer();
            }
            else
            {
                //if (canMove) movePlayerWithAim();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMinimap();
        }
    }

    // Controller or JoyStick Movement (기존 pc는 movePlayerWithAim()로 수정
    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        //카메라 Y축 회전 기반 이동방향 전환
        movement = cameraTransform.TransformDirection(movement);
        movement.y = 0f;

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.6f);
        }
        //transform.Translate(movement * _speed * Time.deltaTime, Space.World);
        gameObject.GetComponent<Rigidbody>().velocity = movement * _speed;
    }

    // PC Ver
    public void movePlayerWithAim()
    {
        if (isPc)
        {
            Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

            //카메라 Y축 회전 기반 이동방향 전환
            movement = cameraTransform.TransformDirection(movement);
            movement.y = 0f;

            //transform.Translate(movement * _speed * Time.deltaTime, Space.World);
            gameObject.GetComponent<Rigidbody>().velocity = movement * _speed;

            // 이게 1버전 with out Ray
            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.03f); // 빠르게 회전하게 설정
            }


            // 이게 2버전 with Ray 갠취가 1버전이라 잠시 주석처리.
            // 캐릭터 회전, 마우스 방향(rotationTarget) 바라보게 설정
            //var lookPos = rotationTarget - transform.position;
            //lookPos.y = 0f;
            //if (lookPos != Vector3.zero)
            //{
            //    var rotation = Quaternion.LookRotation(lookPos);
            //    Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);
            //    if (aimDirection != Vector3.zero)
            //    {
            //        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.6f);
            //    }
            //}



            //transform.Translate(movement * _speed * Time.deltaTime, Space.World);

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
        //Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;
        ////카메라 Y축 회전 기반 이동방향 전환
        //movement = cameraTransform.TransformDirection(movement);
        //movement.y = 0f;

        //transform.Translate(movement * _speed * Time.deltaTime, Space.World);
    }

    private void ToggleMinimap()
    {
        bool isActive = minimap.activeSelf;
        minimap.SetActive(!isActive);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}