using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            anim.SetTrigger("HeavyAttack");

        if (Input.GetKeyDown(KeyCode.Q))
            anim.SetTrigger("Ultimate");
    }
}
