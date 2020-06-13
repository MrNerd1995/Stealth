using UnityEngine;

public class AnimatorSetup
{
    public float speedDampTime = 0.1f;
    public float angularSpeedDampTime = 0.7f;
    public float angleResponseTime = 0.6f;

    private Animator animator;
    private HashIDs hash;

    public AnimatorSetup(Animator anim, HashIDs hashIDs)
    {
        animator = anim;
        hash = hashIDs;
    }

    public void Setup(float speed, float angle)
    {
        float angularSpeed = angle / angleResponseTime;

        animator.SetFloat(hash.speedFloat, speed, speedDampTime, Time.deltaTime);
        animator.SetFloat(hash.angularSpeedFloat, angularSpeed, angularSpeedDampTime, Time.deltaTime);
    }
}
