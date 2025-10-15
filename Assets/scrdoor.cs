using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrdoor : MonoBehaviour
{
    public bool open = false;
    public Sprite openSprite;
    public Sprite closedSprite;

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

}