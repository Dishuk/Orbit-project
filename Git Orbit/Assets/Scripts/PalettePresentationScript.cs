using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalettePresentationScript : MonoBehaviour
{
    [SerializeField] private int timeToStop;
    [SerializeField] private int seed;
    [SerializeField] private CircleCollider2D charcterCollider;


    void Awake()
    {
        Random.InitState(seed);
        StartCoroutine(StopTime(timeToStop));
        charcterCollider.enabled = false;
    }

    IEnumerator StopTime(int time) {
        yield return new WaitForSeconds(timeToStop);
        Time.timeScale = 0;
    }
}
