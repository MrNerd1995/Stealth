using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioClip shoutingClip;
    public float playerSpeed = 3.5f;
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.1f;
    public bool isMobile = true;
    public JoyButton sneakButton;
    public JoyButton hackButton;
    public JoyButton shoutButton;

    private Animator animator;
    private HashIDs hash;
    private Joystick joystick;

    private new Rigidbody rigidbody;
    private new AudioSource audio;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        joystick = FindObjectOfType<Joystick>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

    }

    private void Start()
    {
        animator.SetLayerWeight(1, 1f);
    }

    private void Update()
    {
        bool shout;
        if (isMobile)
        {
            shout = shoutButton.pressed;
        }
        else
        {
            shout = Input.GetButtonDown("Attract");
        }
        animator.SetBool(hash.shoutingBool, shout);
        AudioManagement(shout);
    }

    private void LateUpdate()
    {
        float h;
        float v;
        bool sneak;
        if (isMobile)
        {
            h = joystick.Horizontal;
            v = joystick.Vertical;
            sneak = sneakButton.pressed;
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            sneak = Input.GetButton("Sneak");
        }
        MovementManagement(h, v, sneak);
    }

    private void MovementManagement(float horizontal, float vertical, bool sneaking)
    {
        animator.SetBool(hash.sneakingBool, sneaking);

        if (horizontal != 0f || vertical != 0f)
        {
            Rotating(horizontal, vertical);
            animator.SetFloat(hash.speedFloat, playerSpeed, speedDampTime, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(hash.speedFloat, 0f);
        }
    }

    private void Rotating(float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        rigidbody.MoveRotation(newRotation);
    }

    private void AudioManagement(bool shout)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == hash.locomotionState)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
        }

        if (shout)
        {
            AudioSource.PlayClipAtPoint(shoutingClip, transform.position);
        }
    }
}
