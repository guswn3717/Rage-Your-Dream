using UnityEngine;

public class PlayerJab : MonoBehaviour
{
    [Header("Combo Settings")]
    public float comboWindow = 0.7f; // 입력 가능한 시간
    public float comboGap = 0.15f;   // 콤보 사이 약간의 텀

    private Animator anim;
    private bool isJabbing = false;
    private bool inputBuffered = false;
    private float comboTimer = 0f;
    private float gapTimer = 0f;

    public bool IsJabbing => isJabbing;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        HandleCombo();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!isJabbing)
            {
                StartJab();
            }
            else if (gapTimer <= 0f) // 콤보 입력 간 텀 체크
            {
                inputBuffered = true;
                comboTimer = comboWindow;
            }
        }
    }

    void HandleCombo()
    {
        if (!isJabbing) return;

        comboTimer -= Time.deltaTime;
        gapTimer -= Time.deltaTime;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // 잽 끝나기 직전에 입력됐을 경우
        if ((state.IsName("Jab") || state.IsName("Jab_Combo")) &&
            state.normalizedTime >= 0.85f && inputBuffered)
        {
            inputBuffered = false;
            anim.SetTrigger("Jab_Combo");
            comboTimer = comboWindow;
            gapTimer = comboGap; // 콤보 간 딜레이 부여
        }

        // 입력 없고 애니 끝났으면 종료
        if ((state.IsName("Jab") || state.IsName("Jab_Combo")) &&
            state.normalizedTime >= 1.0f && !inputBuffered)
        {
            isJabbing = false;
            anim.SetBool("IsJabbing", false);
        }

        // 입력 안 하고 시간 초과되면 종료
        if (comboTimer <= 0f && !inputBuffered)
        {
            isJabbing = false;
            anim.SetBool("IsJabbing", false);
        }
    }

    void StartJab()
    {
        isJabbing = true;
        inputBuffered = false;
        comboTimer = comboWindow;
        gapTimer = comboGap;

        anim.SetBool("IsJabbing", true);
        anim.SetTrigger("Jab");
    }
}
