using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] pickupSoundsEffects;
    public AudioSource[] audioSources;
    public float serieEffect;

    private void Start()
    {
    }

    public void PickupSound() {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying == false)
            {
                audioSources[i].pitch = Mathf.Lerp(1, 3, serieEffect / 2) + Random.Range (-0.1f, 0.1f);
                audioSources[i].clip = pickupSoundsEffects[0];
                audioSources[i].Play();
                serieEffect += 5 * Time.deltaTime;
                return;
            }
        }
    }


    private void Update()
    {
        if (serieEffect > 0)
        {
            serieEffect -= Time.deltaTime * 0.1f;
        }
    }
}
