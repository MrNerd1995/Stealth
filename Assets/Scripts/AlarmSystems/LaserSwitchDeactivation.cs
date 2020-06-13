using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitchDeactivation : MonoBehaviour
{
    public GameObject laser;
    public Material unlockedMat;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
    }

    private void OnTriggerStay(Collider other)
    {
        bool hack;
        if (player.GetComponent<PlayerMovement>().isMobile)
        {
            hack = player.GetComponent<PlayerMovement>().hackButton.pressed;
        }
        else
        {
            hack = Input.GetButton("Switch");
        }

        if (other.gameObject == player)
        {
            if (hack)
            {
                LaserDeactivation();
            }
        }
    }

    private void LaserDeactivation()
    {
        laser.SetActive(false);

        Renderer screen = transform.Find("prop_switchUnit_screen").GetComponent<Renderer>();
        screen.material = unlockedMat;
        GetComponent<AudioSource>().Play();
    }
}
