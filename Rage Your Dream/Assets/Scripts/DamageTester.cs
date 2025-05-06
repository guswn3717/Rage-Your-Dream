using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public PlayerStats stats;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // 약공격
        {
            stats.TakeDamage(10f, false); // HP만 감소
        }

        if (Input.GetKeyDown(KeyCode.X)) // 강공격
        {
            stats.TakeDamage(20f, true); // HP + HL 감소
        }
    }
}
