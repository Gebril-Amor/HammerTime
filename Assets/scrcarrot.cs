using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrcarrot : MonoBehaviour
{
  public bool open = false;
    public Sprite openSprite;
    public Sprite closedSprite;
    public float timer = 2f;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        col.enabled = !open;
        sr.sprite = open ? openSprite : closedSprite;
    }

    public void openCarrot()
    {
        open = true;
        Invoke(nameof(closeCarrot), timer);
        AudioSource audio = Camera.main.GetComponent<AudioSource>();
         if (audio == null)
         audio = Camera.main.gameObject.AddComponent<AudioSource>();

         AudioClip snd = Resources.Load<AudioClip>("sfx/sndcarrot");
         if (audio != null && snd != null)
                {
                    audio.PlayOneShot(snd, 0.2f);
                }
                else
                {
                    Debug.Log("Audio clip not found");
                }
    }

    private void closeCarrot()
    {
        open = false;

               AudioSource audio = Camera.main.GetComponent<AudioSource>();
                if (audio == null)
                    audio = Camera.main.gameObject.AddComponent<AudioSource>();

                AudioClip snd = Resources.Load<AudioClip>("sfx/sndcarrot");
                if (audio != null && snd != null)
                {
                    audio.PlayOneShot(snd, 0.2f);
                }
                else
                {
                    Debug.Log("Audio clip not found");
                }
    }
}
