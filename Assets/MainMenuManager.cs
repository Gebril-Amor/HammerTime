using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Settings")]
    public string gameStartScene = "level1";
    
    [Header("UI Sprites")]
    public Sprite titleSprite;
    public Sprite startButtonSprite;
    public Sprite exitButtonSprite;
    public Sprite buttonHoverSprite; // Optional hover sprite

    
    
    [Header("Button Animation")]
    public float hoverScaleAmount = 1.1f;
    public float hoverColorMultiplier = 1.2f;
    public float animationDuration = 0.2f;
    
    private GameObject menuCanvas;
    private Image titleImage;
    private Button startButton;
    private Button exitButton;
    private EventSystem eventSystem;

    void Start()
    {
        // Check if we're in the main menu scene
        if (SceneManager.GetActiveScene().name.ToLower().Contains("menu"))
        {
            CreateMainMenu();
        }
    }

    void CreateMainMenu()
    {
        Debug.Log("Creating main menu...");
        
        // Create EventSystem if it doesn't exist
        CreateEventSystem();
        
        // Create Canvas
        menuCanvas = new GameObject("MainMenuCanvas");
        Canvas canvas = menuCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        // Add Canvas Scaler for proper UI scaling
        CanvasScaler scaler = menuCanvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Add Graphic Raycaster
        menuCanvas.AddComponent<GraphicRaycaster>();

        // Create Title Image (if sprite is provided)
        if (titleSprite != null)
        {
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(menuCanvas.transform);
            titleImage = titleObj.AddComponent<Image>();
            titleImage.sprite = titleSprite;
            titleImage.preserveAspect = true;
            
            // Setup title position (top of screen)
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.sizeDelta = new Vector2(1280, 256);
            titleRect.anchoredPosition = new Vector2(0, 200);
            titleRect.localScale = Vector3.one;
        }

        // Create Start Button
        GameObject startObj = new GameObject("StartButton");
        startObj.transform.SetParent(menuCanvas.transform);
        startButton = startObj.AddComponent<Button>();
        
        // Add hover animation component
        ButtonHoverAnimation startHover = startObj.AddComponent<ButtonHoverAnimation>();
        startHover.hoverScaleAmount = hoverScaleAmount;
        startHover.hoverColorMultiplier = hoverColorMultiplier;
        startHover.animationDuration = animationDuration;
        
        // Setup button image
        Image startImage = startObj.AddComponent<Image>();
        if (startButtonSprite != null)
        {
            startImage.sprite = startButtonSprite;
            startImage.type = Image.Type.Sliced;
        }
        else
        {
            startImage.color = Color.green;
        }
        
        // Setup button transition
        startButton.transition = Selectable.Transition.None; // We'll handle transitions manually
        
        // Add button text (fallback if no sprite)
        if (startButtonSprite == null)
        {
            GameObject startTextObj = new GameObject("ButtonText");
            startTextObj.transform.SetParent(startObj.transform);
            Text startText = startTextObj.AddComponent<Text>();
            startText.text = "START GAME";
            startText.color = Color.white;
            startText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            startText.alignment = TextAnchor.MiddleCenter;
            startText.fontSize = 24;

            RectTransform startTextRect = startTextObj.GetComponent<RectTransform>();
            startTextRect.anchorMin = Vector2.zero;
            startTextRect.anchorMax = Vector2.one;
            startTextRect.offsetMin = Vector2.zero;
            startTextRect.offsetMax = Vector2.zero;
        }

        // Setup start button position
        RectTransform startRect = startObj.GetComponent<RectTransform>();
        startRect.sizeDelta = new Vector2(80*6, 16*6);
        startRect.anchoredPosition = new Vector2(0, -50);
        startRect.localScale = Vector3.one;

        // Create Exit Button
        GameObject exitObj = new GameObject("ExitButton");
        exitObj.transform.SetParent(menuCanvas.transform);
        exitButton = exitObj.AddComponent<Button>();
        
        // Add hover animation component
        ButtonHoverAnimation exitHover = exitObj.AddComponent<ButtonHoverAnimation>();
        exitHover.hoverScaleAmount = hoverScaleAmount;
        exitHover.hoverColorMultiplier = hoverColorMultiplier;
        exitHover.animationDuration = animationDuration;
        
        // Setup exit button image
        Image exitImage = exitObj.AddComponent<Image>();
        if (exitButtonSprite != null)
        {
            exitImage.sprite = exitButtonSprite;
            exitImage.type = Image.Type.Sliced;
        }
        else
        {
            exitImage.color = Color.red;
        }
        
        // Setup button transition
        exitButton.transition = Selectable.Transition.None;
        
        // Add exit button text (fallback if no sprite)
        if (exitButtonSprite == null)
        {
            GameObject exitTextObj = new GameObject("ButtonText");
            exitTextObj.transform.SetParent(exitObj.transform);
            Text exitText = exitTextObj.AddComponent<Text>();
            exitText.text = "EXIT GAME";
            exitText.color = Color.white;
            exitText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            exitText.alignment = TextAnchor.MiddleCenter;
            exitText.fontSize = 24;

            RectTransform exitTextRect = exitTextObj.GetComponent<RectTransform>();
            exitTextRect.anchorMin = Vector2.zero;
            exitTextRect.anchorMax = Vector2.one;
            exitTextRect.offsetMin = Vector2.zero;
            exitTextRect.offsetMax = Vector2.zero;
        }

        // Setup exit button position
        RectTransform exitRect = exitObj.GetComponent<RectTransform>();
        exitRect.sizeDelta = new Vector2(80*6, 16*6);
        exitRect.anchoredPosition = new Vector2(0, -250);
        exitRect.localScale = Vector3.one;

        // Add button click events
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);

        Debug.Log("Main menu created successfully!");
    }

    void CreateEventSystem()
    {
        // Check if EventSystem already exists
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystem = eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }
    }

    void StartGame()
    {
        Debug.Log("Starting game...");
        
        // Disable buttons to prevent multiple clicks
        startButton.interactable = false;
        exitButton.interactable = false;
        
        // Use your fade transition
        if (FadeSceneTransition.Instance != null)
        {
            FadeSceneTransition.Instance.TransitionToScene(gameStartScene);
        }
        else
        {
            SceneManager.LoadScene(gameStartScene);
        }
    }

    void ExitGame()
    {
        Debug.Log("Exiting game...");
        
        startButton.interactable = false;
        exitButton.interactable = false;
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void Update()
    {
        // Keyboard controls
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void PressStartButton()
    {
        StartGame();
    }

    public void PressExitButton()
    {
        ExitGame();
    }
}

// Button Hover Animation Component
public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float hoverScaleAmount = 1.1f;
    public float hoverColorMultiplier = 1.2f;
    public float animationDuration = 0.2f;
    
    private Vector3 originalScale;
    private Color originalColor;
    private Image buttonImage;
    private Button button;
    private bool isHovering = false;
    
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        originalScale = transform.localScale;
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            isHovering = true;
            StopAllCoroutines();
            StartCoroutine(AnimateScale(originalScale * hoverScaleAmount));
            StartCoroutine(AnimateColor(originalColor * hoverColorMultiplier));
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            isHovering = false;
            StopAllCoroutines();
            StartCoroutine(AnimateScale(originalScale));
            StartCoroutine(AnimateColor(originalColor));
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateScale(originalScale * 0.9f));
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            StopAllCoroutines();
            if (isHovering)
            {
                StartCoroutine(AnimateScale(originalScale * hoverScaleAmount));
            }
            else
            {
                StartCoroutine(AnimateScale(originalScale));
            }
        }
    }
    
    private IEnumerator AnimateScale(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;
        
        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        transform.localScale = targetScale;
    }
    
    private IEnumerator AnimateColor(Color targetColor)
    {
        if (buttonImage == null) yield break;
        
        Color startColor = buttonImage.color;
        float time = 0f;
        
        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;
            buttonImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        
        buttonImage.color = targetColor;
    }
}