using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody rb;
    private Vector3 moveDirection;

    public Transform targetEnemy;
    private PlayerJab jab;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jab = GetComponent<PlayerJab>();
        anim = GetComponent<Animator>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        if (jab != null && jab.IsJabbing)
        {
            // 잽 중엔 살짝 앞으로 밀어주고 걷기 애니메이션은 끔
            rb.MovePosition(rb.position + transform.forward * 0.5f * Time.fixedDeltaTime);
            anim.SetFloat("MoveSpeed", 0f);
        }
        else
        {
            HandleMovement();
        }

        HandleRotation();
    }

    void HandleMovement()
    {
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

        // 이동 속도 값으로 애니메이터 파라미터 세팅 (0~1 범위)
        anim.SetFloat("MoveSpeed", moveDirection.magnitude);
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
}
