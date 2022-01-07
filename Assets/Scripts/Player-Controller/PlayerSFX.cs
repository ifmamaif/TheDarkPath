using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{

    public AudioSource rollSFX;
    public AudioSource hurtSFX;
    public AudioSource shootSFX;

    public void playRoll()
    {
        rollSFX.Play();
    }

    public void stopRoll()
    {
        rollSFX.Stop();
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
