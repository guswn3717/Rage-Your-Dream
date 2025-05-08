using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Animator anim;
    private PlayerInput input;
    private PlayerStats stats;

    private Vector3 moveDirection;

    public Transform targetEnemy; // 인스펙터에서 적 할당

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        stats = GetComponent<PlayerStats>();

        // 넘어짐 방지 - X, Z 축 회전 고정 (Y축만 회전 가능)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = true;
    }

    void Update()
    {
        HandleActions();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
{
    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");

    Vector3 inputDir = new Vector3(h, 0, v).normalized;

    if (targetEnemy != null)
    {
        // 적을 바라보는 방향을 기준으로 입력 방향 변환
        Vector3 enemyDir = (targetEnemy.position - transform.position).normalized;
        enemyDir.y = 0;

        Quaternion enemyRot = Quaternion.LookRotation(enemyDir);
        Vector3 moveDir = enemyRot * inputDir;

        moveDirection = moveDir.normalized;
    }
    else
    {
        moveDirection = inputDir;
    }

    Vector3 moveOffset = moveDirection * moveSpeed * Time.fixedDeltaTime;
    Vector3 newPos = rb.position + moveOffset;

    rb.MovePosition(newPos);
}


    void HandleRotation()
    {
        if (targetEnemy != null)
        {
            Vector3 lookPos = targetEnemy.position - transform.position;
            lookPos.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lookPos);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("IsGuarding", true);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            anim.SetBool("IsGuarding", false);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("Ultimate");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("LightAttack");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.SetTrigger("HeavyAttack");
        }
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", moveDirection.magnitude);
        anim.SetBool("isWalk", moveDirection.magnitude > 0.1f);
    }
}
