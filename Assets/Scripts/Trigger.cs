using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _director;
    private void OnTriggerEnter(Collider other)
    {
        _director.Play();
        this.gameObject.SetActive(false);
    }
}
