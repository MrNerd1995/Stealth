using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float maxDamage = 120f;
    public float minDamage = 45f;
    public AudioClip shotClip;
    public float flashIntensity = 3f;
    public float fadeSpeed = 10f;

    private Animator animator;
    private HashIDs hash;
    private LineRenderer laserShotLine;
    private Light laserShotLight;
    private SphereCollider sphereCollider;
    private Transform player;
    private PlayerHealth playerHealth;
    private bool shooting;
    private float scaledDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        laserShotLine = GetComponentInChildren<LineRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
    }

    private void Start()
    {
        laserShotLight = laserShotLine.gameObject.GetComponent<Light>();
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();

        laserShotLine.enabled = false;
        laserShotLight.intensity = 0f;

        scaledDamage = maxDamage - minDamage;
    }

    private void Update()
    {
        float shot = animator.GetFloat(hash.shotFloat);

        if (shot > 0.5f && !shooting)
        {
            Shoot();
        }

        if (shot < 0.5f)
        {
            shooting = false;
            laserShotLine.enabled = false;
        }

        laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float aimWeight = animator.GetFloat(hash.aimWeightFloat);
        animator.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
    }

    private void Shoot()
    {
        shooting = true;
        float fracDistance = (sphereCollider.radius - Vector3.Distance(transform.position, player.position)) / sphereCollider.radius;
        float damage = scaledDamage * fracDistance + minDamage;
        playerHealth.TakeDamage(damage);
        ShotEffects();
    }

    private void ShotEffects()
    {
        laserShotLine.SetPosition(0, laserShotLine.transform.position);
        laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);
        laserShotLine.enabled = true;
        laserShotLight.intensity = flashIntensity;
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
    }
}
