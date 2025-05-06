using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;
    private Animator animator;
    private PlayerStats stats;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        HandleMovement();
        HandleActions();
    }

    void HandleMovement()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.W)) v += 1f;
        if (Input.GetKey(KeyCode.S)) v -= 1f;
        if (Input.GetKey(KeyCode.D)) h += 1f;
        if (Input.GetKey(KeyCode.A)) h -= 1f;

        Vector3 move = new Vector3(h, 0, v).normalized;
        if (move.magnitude > 0)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
            transform.forward = move; // 이동 방향으로 회전
        }
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Guard();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            LightAttack();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            HeavyAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ultimate();
        }
    }

    void Guard()
    {
        Debug.Log("가드!");
        // animator.SetTrigger("Guard");
    }

    void LightAttack()
    {
        Debug.Log("약공격!");
        // animator.SetTrigger("LightAttack");
    }

    void HeavyAttack()
    {
        Debug.Log("강공격!");
        // animator.SetTrigger("HeavyAttack");
    }

    void Ultimate()
    {
        Debug.Log("궁극기!");
        // animator.SetTrigger("Ultimate");
    }
}
