using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrbolt : MonoBehaviour
{
    public float amplitude = 0.2f;
    public float frequency = 20.0f;

    private Transform spriteTransform;
    private Vector3 initialLocalPosition;

    void Start()
    {
        // Instead of the whole object, get the transform of the sprite child
        spriteTransform = GetComponent<SpriteRenderer>().transform;
        initialLocalPosition = spriteTransform.localPosition;
    }

    void Update()
    {
        // Cosine for smooth float
        float floatVal = Mathf.Cos(Time.time * frequency);
        Vector3 offset = new Vector3(0, amplitude * floatVal, 0);

        // Move only the sprite visually, relative to parent
        spriteTransform.localPosition = initialLocalPosition + offset;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerChargesUI>() != null)
        {


               AudioSource audio = Camera.main.GetComponent<AudioSource>();
                if (audio == null)
                    audio = Camera.main.gameObject.AddComponent<AudioSource>();

                AudioClip snd = Resources.Load<AudioClip>("sfx/sndbolt");
                if (audio != null && snd != null)
                {
                    audio.PlayOneShot(snd, 0.5f);
                }
                else
                {
                    Debug.Log("Audio clip not found");
                }


            // Get the player's charge component
            PlayerChargesUI playerCharges = collision.gameObject.GetComponent<PlayerChargesUI>();

            // If the player has a charge component, add one charge
            if (playerCharges != null)
            {
                playerCharges.Recharge();
                
                
                
            }

            // Destroy this object's parent
            Destroy(transform.parent.gameObject);
        }
    }
}

