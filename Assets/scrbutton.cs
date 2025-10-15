using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrbutton : MonoBehaviour
{
    public GameObject door;
    // Start is called before the first frame update
  public void toggleDoor()
    {
        scrdoor scr = door.GetComponent<scrdoor>();
        if (scr != null)
        {
            scr.open = !scr.open;
        }
        door.GetComponent<Collider2D>().enabled = !door.GetComponent<Collider2D>().enabled;


    AudioSource audio = GetComponent<AudioSource>();
    if (audio == null)
        audio = gameObject.AddComponent<AudioSource>();
    
    AudioClip sndDoor = Resources.Load<AudioClip>("sfx/snddoor");
    if (audio != null && sndDoor != null)
    {
        audio.PlayOneShot(sndDoor, 0.5f);
    }

    }
}
