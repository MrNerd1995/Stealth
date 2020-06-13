using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVPlayerDetection : MonoBehaviour
{
    #region Fields

    private GameObject player;
    private LastPlayerSighting lastPlayerSighting;

    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            Vector3 relPlayerPosition = player.transform.position - transform.position;

            if (Physics.Raycast(transform.position, relPlayerPosition, out RaycastHit hit))
            {
                if (hit.collider.gameObject == player)
                {
                    lastPlayerSighting.position = player.transform.position;
                }
            }
        }
    }
}
