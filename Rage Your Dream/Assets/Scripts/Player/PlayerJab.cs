using UnityEngine;

public class PlayerJab : MonoBehaviour
{
    private Animator anim;
    private bool isJabbing = false;
    private bool inputBuffered = false;
    private bool isHeavyPunching = false;
    private float comboTimer = 0f;
    private float comboWindow = 0.7f;

    public bool IsJabbing => isJabbing; // 외부 이동 제어용
    public bool IsHeavyPunching => isHeavyPunching;

    [SerializeField] private float jabMaxIdleTime = 1.0f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isHeavyPunching)
        {
            // 강펀치 중에는 잽 입력 막음
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!isJabbing)
            {
                JabStart();
            }
            else
            {
                inputBuffered = true;
                comboTimer = comboWindow; // 콤보 창 재설정
            }
        }

        if (!isJabbing) return;

        comboTimer -= Time.deltaTime;

        if (comboTimer <= -jabMaxIdleTime)
        {
            isJabbing = false;
            anim.SetBool("IsJabbing", false);
            inputBuffered = false;
            return;
        }

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if ((state.IsName("Jab") || state.IsName("Jab_Combo")) &&
            state.normalizedTime >= 0.85f && inputBuffered)
        {
            inputBuffered = false;
            anim.SetTrigger("Jab_Combo");
            comboTimer = comboWindow;
        }

        if ((state.IsName("Jab") || state.IsName("Jab_Combo")) &&
            state.normalizedTime >= 1.0f && !inputBuffered)
        {
            isJabbing = false;
            anim.SetBool("IsJabbing", false);
        }
    }

    void JabStart()
    {
        isJabbing = true;
        comboTimer = comboWindow;
        inputBuffered = false;

        anim.SetBool("IsJabbing", true);
        anim.SetTrigger("Jab");
    }

    public bool CanHeavyPunch()
    {
        // 강펀치 가능 상태: 잽 콤보 중이거나 잽 중이지 않거나 (idle/walk 포함)
        return !isHeavyPunching && (!isJabbing || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"));
    }

    public void HeavyPunchStart()
    {
        if (!CanHeavyPunch()) return;

        isHeavyPunching = true;
        isJabbing = false;
        anim.SetBool("IsJabbing", false);

        anim.SetTrigger("HeavyPunch");
    }

    // 애니메이션 이벤트로 호출 예정
    public void HeavyPunchEnd()
    {
        isHeavyPunching = false;
    }
}
