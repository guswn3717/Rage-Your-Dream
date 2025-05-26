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

    public float dodgeForce = 12f;
    public float dodgeDrag = 6f;
    private float originalDrag;
    private Vector3 dodgeDirection;

    private int jabCount = 0;              // 잽 콤보 카운터
    private bool isJabbing = false;
    private float comboTimer = 0f;
    private float comboWindow = 0.7f;

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

        if (isJabbing)
        {
            comboTimer -= Time.deltaTime;

            // 콤보 타임 아웃 시 콤보 종료
            if (comboTimer <= 0f)
            {
                jabCount = 0;
                isJabbing = false;
                anim.SetBool("IsJabbing", false);
            }

            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

            // 잽 애니메이션 거의 끝나고, 콤보 잽이 남아있을 때 다음 콤보 연결
            if ((state.IsName("Jab") || state.IsName("Jab_Combo")) &&
                state.normalizedTime >= 0.95f && jabCount > 0)
            {
                jabCount--;
                anim.SetTrigger("Jab_Combo");
                comboTimer = comboWindow;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDodging)
            HandleMovement();

        HandleRotation();
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

        if (Input.GetKeyDown(KeyCode.F))
            anim.SetBool("IsGuarding", true);
        if (Input.GetKeyUp(KeyCode.F))
            anim.SetBool("IsGuarding", false);

        if (Input.GetKeyDown(KeyCode.K))
        {
            jabCount++;
            if (!isJabbing)
            {
                JabStart();
            }
            comboTimer = comboWindow;
        }

        if (Input.GetKeyDown(KeyCode.L))
            anim.SetTrigger("HeavyAttack");

        if (Input.GetKeyDown(KeyCode.Q))
            anim.SetTrigger("Ultimate");
    }

    void JabStart()
    {
        isJabbing = true;
        comboTimer = comboWindow;

        anim.SetBool("IsJabbing", true);  // 잽 콤보 시작 시 true 설정
        anim.SetTrigger("Jab");
    }

    IEnumerator DodgeRoutine(Vector3 dir)
    {
        isDodging = true;
        anim.SetBool("isDodging", true);

        yield return new WaitForSeconds(0.15f);

        rb.velocity = Vector3.zero;
        rb.drag = dodgeDrag;
        rb.AddForce(dir.normalized * dodgeForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.15f);

        rb.drag = originalDrag;
        isDodging = false;
        anim.SetBool("isDodging", false);
    }

    void UpdateAnimations()
    {
        anim.SetFloat("Speed", isDodging ? 0f : moveDirection.magnitude);
        anim.SetBool("isWalking", !isDodging && moveDirection.magnitude > 0.1f);
    }
}
