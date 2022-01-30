using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    public AudioSource hoverSFX;
    public AudioSource hurtSFX;
    public AudioSource shootSFX;

    public void playHover()
    {
        hoverSFX.Play();
    }

    public void playHurt()
    {
        hurtSFX.Play();
    }

    public void playShoot()
    {
        shootSFX.Play();
    }
}
