using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동 속도
    public float moveSpeed = 5f;

    // 컴포넌트 참조
    private Rigidbody rb;
    private Animator anim;
    private PlayerInput input;
    private PlayerStats stats;

    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        HandleMovement();
        HandleActions();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        // WASD 입력 받기
        float h = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float v = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        moveDirection = new Vector3(h, 0, v).normalized;

        // 이동
        Vector3 velocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 가드 시작
            anim.SetBool("IsGuarding", true);
            // stats.SetGuarding(true); // 가드 상태 세팅 (예시)
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            // 가드 종료
            anim.SetBool("IsGuarding", false);
            // stats.SetGuarding(false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 궁극기 발동
            anim.SetTrigger("Ultimate");
            // stats.UseUltimate(); // 궁극기 사용 처리
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            // 약공격
            anim.SetTrigger("LightAttack");
            // stats.DoLightAttack();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            // 강공격
            anim.SetTrigger("HeavyAttack");
            // stats.DoHeavyAttack();
        }
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", moveDirection.magnitude);
    }
}
