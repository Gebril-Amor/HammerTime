using UnityEngine;
using UnityEngine.UI;

public class PlayerChargesUI : MonoBehaviour
{
    public  Sprite fullBolt;
    public  Sprite emptyBolt;

    public int maxCharges = 3;
    public int charges = 3;
    private Image[] bolts;

    private Canvas canvas;
    private RectTransform container;

    void Start()
    {
        // --- 1️⃣ Create a Canvas ---
        GameObject canvasGO = new GameObject("ChargeCanvas");
        canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // --- 2️⃣ Create a container (to hold the bolts) ---
        GameObject containerGO = new GameObject("ChargeContainer");
        container = containerGO.AddComponent<RectTransform>();
        container.SetParent(canvas.transform);
        container.anchorMin = new Vector2(0f, 0.9f); // top center
        container.anchorMax = new Vector2(0f, 0.9f);
        container.pivot = new Vector2(0f, 0.5f);
        container.anchoredPosition = Vector2.zero;
        container.sizeDelta = new Vector2(200, 50);

        // --- 3️⃣ Create the bolts dynamically ---
        bolts = new Image[maxCharges];
        float spacing = 44f;

        for (int i = 0; i < maxCharges; i++)
        {
            GameObject boltGO = new GameObject("Bolt_" + i);
            boltGO.transform.SetParent(container);
            Image img = boltGO.AddComponent<Image>();
            img.sprite = fullBolt;
            img.rectTransform.sizeDelta = new Vector2(40, 40);
            img.rectTransform.anchoredPosition = new Vector2(i * spacing - (maxCharges - 1) * spacing / 2, 0);
            bolts[i] = img;
        }
        SetCharges(charges);
    }

    void Update()
    {
        // Test keys
        if (Input.GetKeyDown(KeyCode.Space)) UseCharge();
        if (Input.GetKeyDown(KeyCode.R)) Recharge();
    }

    public void SetCharges(int amount)
    {
        charges = Mathf.Clamp(amount, 0, maxCharges);
        for (int i = 0; i < bolts.Length; i++)
        {
            bolts[i].sprite = (i < charges) ? fullBolt : emptyBolt;
        }
    }

    public void UseCharge()
    {
        if (charges > 0)
        {
            charges--;
            SetCharges(charges);
        }
    }

    public void Recharge()
    {
        if (charges < maxCharges)
        {
            charges++;
            SetCharges(charges);
        }
    }
}
