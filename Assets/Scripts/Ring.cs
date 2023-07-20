using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Got Colliosion");
        Destroy(transform.parent.gameObject);
    }
}
