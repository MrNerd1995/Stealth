using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public bool requireKey;
    public AudioClip doorSwishClip;
    public AudioClip accessDeniedClip;

    private Animator animator;
    private HashIDs hash;
    private GameObject player;
    private PlayerInventory playerInventory;
    private int count;

    private new AudioSource audio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerInventory = player.GetComponent<PlayerInventory>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        animator.SetBool(hash.openBool, count > 0);

        if (animator.IsInTransition(0) && !audio.isPlaying)
        {
            audio.clip = doorSwishClip;
            audio.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (requireKey)
            {
                if (playerInventory.hasKey)
                {
                    count++;
                }
                else
                {
                    audio.clip = accessDeniedClip;
                    audio.Play();
                }
            }
            else
            {
                count++;
            }
        }
        else if (other.gameObject.tag == Tags.enemy)
        {
            if (other is CapsuleCollider)
            {
                count++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player || (other.gameObject.tag == Tags.enemy && other is CapsuleCollider))
        {
            count = Mathf.Max(0, count - 1);
        }
    }
}
