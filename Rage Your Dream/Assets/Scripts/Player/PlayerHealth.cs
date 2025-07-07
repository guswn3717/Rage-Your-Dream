using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 1000f;
    public float currentHP;

    public float maxHL = 1000f;
    public float currentHL;

    public float maxGuardHP = 300f;
    public float currentGuardHP;

    public float guardRecoveryPercent = 0.66f;

    public float guardBreakStunDuration = 2f;

    private bool isGuardBroken = false;

    void Start()
    {
        currentHP = maxHP;

        currentHL = maxHL;

        currentGuardHP = maxGuardHP;
    }

    void Update()
    {
        // HL 바 천천히 회복 (가드 깨진 후 회복 시뮬레이션 등은 따로 처리)
        if(currentHL < maxHL && !isGuardBroken)
        {
            currentHL += Time.deltaTime * 5f; // 회복 속도 조절

            currentHL = Mathf.Min(currentHL, maxHL);
        }
    }

    public void TakeDamage(float damage, bool isStrongAttack, bool isGuarding)
    {
        if (isGuarding)
        {
            // 가드 중일 때 가드 HP에 트루 데미지
            currentGuardHP -= damage;

            // HP도 깎이지만 데미지 배수 적용 (예: 0.2배)
            float guardDamageMultiplier = 0.2f;

            currentHP -= damage * guardDamageMultiplier;

            if(currentGuardHP <= 0 && !isGuardBroken)
            {
                StartCoroutine(GuardBreak());
            }
        }
        else
        {
            // 일반 공격 맞음
            currentHP -= damage;

            if (isStrongAttack)
            {
                // 강공격 맞으면 HL도 깎음
                currentHL -= damage;

                if (currentHL < 0) currentHL = 0;
            }
        }

        if(currentHP <= 0)
        {
            // 다운 처리
            Debug.Log("Player Down!");

            currentHP = 0;
        }
    }

    IEnumerator GuardBreak()
    {
        isGuardBroken = true;

        Debug.Log("Guard Broken! Stunned!");

        // 스턴 상태 처리 (이동 불가 등)
        yield return new WaitForSeconds(guardBreakStunDuration);

        // 가드 HP 즉시 66% 회복
        currentGuardHP = maxGuardHP * guardRecoveryPercent;

        isGuardBroken = false;
        
        Debug.Log("Guard Recovered");
    }
}
