using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrfloat : MonoBehaviour
{
    // Start is called before the first frame update    public float amplitude = 0.2f;    public float amplitude = 0.2f;
    public float frequency = 20.0f;
    public float amplitude = 0.2f;
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

}
