using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody rb;
    private Animator anim;
    private PlayerInput input;
    private PlayerStats stats;

    private Vector3 moveDirection;
    private bool isDodging = false;

    public Transform targetEnemy;

    // 닷지 관련 변수
    public float dodgeForce = 12f;
    public float dodgeDrag = 6f;
    private float originalDrag;
    private Vector3 dodgeDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        stats = GetComponent<PlayerStats>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = true;

        originalDrag = rb.drag;
    }

    void Update()
    {
        HandleActions();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (!isDodging)
            HandleMovement();

        HandleRotation();
    }

    // 공격 판정 시 호출 (충돌체나 레이캐스트 히트 대상)
    void DealDamageToEnemy(GameObject enemyObj, float damage)
    {
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    void HandleMovement()
    {
        if (isDodging) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (targetEnemy != null)
        {
            Vector3 enemyDir = (targetEnemy.position - transform.position).normalized;
            enemyDir.y = 0;
            Quaternion enemyRot = Quaternion.LookRotation(enemyDir);
            moveDirection = (enemyRot * inputDir).normalized;
        }
        else
        {
            moveDirection = inputDir;
        }

        Vector3 moveOffset = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveOffset);
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
        // 닷지: 스페이스 + 입력방향
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
        {
            Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (targetEnemy != null)
            {
                Vector3 enemyDir = (targetEnemy.position - transform.position).normalized;
                enemyDir.y = 0;
                Quaternion enemyRot = Quaternion.LookRotation(enemyDir);
                dodgeDirection = enemyRot * inputDir;
            }
            else
            {
                dodgeDirection = inputDir;
            }

            if (dodgeDirection == Vector3.zero)
                dodgeDirection = transform.forward;

            StartCoroutine(DodgeRoutine(dodgeDirection));
        }

        // 가드
        if (Input.GetKeyDown(KeyCode.F))
            anim.SetBool("IsGuarding", true);
        if (Input.GetKeyUp(KeyCode.F))
            anim.SetBool("IsGuarding", false);

        // 공격 트리거 - 애니메이터에 맞게 트리거명 통일
        if (Input.GetKeyDown(KeyCode.K))
            anim.SetTrigger("Jab");          // 약공격 (잽)
        if (Input.GetKeyDown(KeyCode.L))
            anim.SetTrigger("HeavyAttack");  // 강공격
        if (Input.GetKeyDown(KeyCode.Q))
            anim.SetTrigger("Ultimate");     // 궁극기
    }

    IEnumerator DodgeRoutine(Vector3 dir)
    {
        isDodging = true;
        anim.SetBool("isDodging", true); // 닷지 애니메이션 시작

        // 애니메이션 선딜(닷지 모션 시작 대기)
        yield return new WaitForSeconds(0.15f);

        // 대시 이동 시작
        rb.velocity = Vector3.zero;
        rb.drag = dodgeDrag;
        rb.AddForce(dir.normalized * dodgeForce, ForceMode.Impulse);

        // 대시 지속 시간
        yield return new WaitForSeconds(0.15f);

        rb.drag = originalDrag;
        isDodging = false;
        anim.SetBool("isDodging", false); // 닷지 애니메이션 종료
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", isDodging ? 0f : moveDirection.magnitude);
        anim.SetBool("isWalking", !isDodging && moveDirection.magnitude > 0.1f);
    }
}
