using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy_Kamikaze : MonoBehaviour
{
    private Animator animator;

    private bool isChasingPlayer;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isChasingPlayer)
        {
            if (SeesPlayer())
            {
                isChasingPlayer = true;
                StartChase();
                Debug.Log("Eita Porra, vi hein!");
                //start chase
            }
        }
        else
        {
            //check for explosion
        }
    }

    private bool SeesPlayer()
    {
        RaycastHit raycastHit;
        Physics.Linecast(transform.position, Camera.main.transform.position, out raycastHit);
        return (raycastHit.transform.root.tag == "Player");
    }

    private void StartChase()
    {

    }
}
