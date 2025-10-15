using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // Add this line
using UnityEngine;

public class scrhp : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp=3;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int maxHealth = 5;

    private Image[] hearts;
    private Canvas canvas;
    private RectTransform container;

     void Start()
    {
        // --- 1️⃣ Create a Canvas ---
        GameObject canvasGO = new GameObject("HealthCanvas");
        canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // --- 2️⃣ Create a container (to hold the hearts) ---
        GameObject containerGO = new GameObject("HealthContainer");
        container = containerGO.AddComponent<RectTransform>();
        container.SetParent(canvas.transform);
        container.anchorMin = new Vector2(1.15f, 0.9f); // top-right
        container.anchorMax = new Vector2(1.15f, 0.9f);
        container.pivot = new Vector2(1f, 0.5f);
        container.anchoredPosition = new Vector2(-20, 0);
        container.sizeDelta = new Vector2(300, 50);

        // --- 3️⃣ Create the heart icons dynamically ---
        hearts = new Image[maxHealth];
        float spacing = 44f;

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heartGO = new GameObject("Heart_" + i);
            heartGO.transform.SetParent(container);
            Image img = heartGO.AddComponent<Image>();
            img.sprite = fullHeart;
            img.rectTransform.sizeDelta = new Vector2(40, 40);
            img.rectTransform.anchoredPosition = new Vector2(-i * spacing, 0); // align right-to-left
            hearts[i] = img;
        }
    }



    public void SetHealth(int amount)
    {
        hp = Mathf.Clamp(amount, 0, maxHealth);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < hp) ? fullHeart : emptyHeart;
        }
    }
    
    public void TakeDamage(int damage)
    {
        hp -= 1;
        SetHealth(hp);
                       AudioSource audio = Camera.main.GetComponent<AudioSource>();
                if (audio == null)
                    audio = Camera.main.gameObject.AddComponent<AudioSource>();

                AudioClip snd = Resources.Load<AudioClip>("sfx/sndhurt");
                if (audio != null && snd != null)
                {
                    audio.PlayOneShot(snd, 0.5f);
                }
                else
                {
                    Debug.Log("Audio clip not found");
                }
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
