using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public float resetAfterDeathTime = 5f;
    public AudioClip deathClip;

    private Animator animator;
    private PlayerMovement playerMovement;
    private HashIDs hash;
    private SceneFadeInOut sceneFadeInOut;
    private LastPlayerSighting lastPlayerSighting;
    private float timer;
    private bool playerDead;

    private new AudioSource audio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }

    private void Update()
    {
        if (health <= 0f)
        {
            if (!playerDead)
            {
                PlayerDying();
            }
            else
            {
                PlayerDead();
                LevelReset();
            }
        }
    }

    private void PlayerDying()
    {
        playerDead = true;
        animator.SetBool(hash.deadBool, true);
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    private void PlayerDead()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == hash.dyingState)
        {
            animator.SetBool(hash.deadBool, false);
        }

        animator.SetFloat(hash.speedFloat, 0f);
        playerMovement.enabled = false;
        lastPlayerSighting.position = lastPlayerSighting.resetPosition;
        audio.Stop();
    }

    private void LevelReset()
    {
        timer += Time.deltaTime;

        if (timer >= resetAfterDeathTime)
        {
            StartCoroutine(sceneFadeInOut.EndScene());
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
