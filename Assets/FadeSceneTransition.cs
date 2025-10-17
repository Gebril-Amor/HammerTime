using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeSceneTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    
    [Header("Game Over Settings")]
    public Sprite gameOverSprite;
    
    private Image fadeImage;
    private Canvas fadeCanvas;
    private bool isTransitioning = false;
    private bool isGameOver = false;
    private Image gameOverImage;

    public static FadeSceneTransition Instance;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateFadeSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void CreateFadeSystem()
    {
        // Create Canvas
        fadeCanvas = new GameObject("FadeCanvas").AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 9999; // Highest order to be on top of everything
        
        // Add Graphic Raycaster and disable it (we don't need UI interaction)
        GraphicRaycaster raycaster = fadeCanvas.gameObject.AddComponent<GraphicRaycaster>();
        raycaster.enabled = false;
        
        // Create Image for fading
        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(fadeCanvas.transform);
        
        fadeImage = imageObject.AddComponent<Image>();
        fadeImage.color = fadeColor;
        
        // Setup RectTransform for full screen coverage
        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        
        // Initially hidden
        fadeImage.gameObject.SetActive(false);
        
        DontDestroyOnLoad(fadeCanvas.gameObject);
    }

    void Start()
    {
        // Fade in when game starts
        BringToFront();
        StartCoroutine(FadeIn());
    }
    
    public void BringToFront()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.sortingOrder = 9999; // Ensure it's on top
        }
    }
    
    // ========== GAME OVER SYSTEM ==========
    public void TriggerGameOver()
    {
        if (!isGameOver && !isTransitioning)
        {
            isGameOver = true;
            StartCoroutine(GameOverSequence());
        }
    }
    
    private IEnumerator GameOverSequence()
    {
        // Fade to black
        yield return StartCoroutine(FadeOutCoroutine(null));
        
        // Show Game Over sprite
        ShowGameOverSprite();
        
        // Wait for any input
        yield return WaitForAnyInput();
        
        // Hide Game Over sprite and fade in
        HideGameOverSprite();
        yield return StartCoroutine(FadeInCoroutine(null));
        
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        isGameOver = false;
    }
    
    private void ShowGameOverSprite()
    {
        if (gameOverSprite != null && fadeCanvas != null)
        {
            GameObject gameOverObj = new GameObject("GameOverImage");
            gameOverObj.transform.SetParent(fadeCanvas.transform);
            
            gameOverImage = gameOverObj.AddComponent<Image>();
            gameOverImage.sprite = gameOverSprite;
            gameOverImage.preserveAspect = true;
            
            RectTransform rect = gameOverObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(400, 200);
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
        }
    }
    
    private void HideGameOverSprite()
    {
        if (gameOverImage != null)
        {
            Destroy(gameOverImage.gameObject);
            gameOverImage = null;
        }
    }
    
    private IEnumerator WaitForAnyInput()
    {
        Debug.Log("Game Over - Press any key or click to restart...");
        
        bool inputReceived = false;
        
        while (!inputReceived)
        {
            // Check for any key press
            if (Input.anyKeyDown)
            {
                inputReceived = true;
            }
            
            // Check for mouse clicks
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                inputReceived = true;
            }
            
            yield return null;
        }
    }
    // ========== END GAME OVER SYSTEM ==========
    
    public void TransitionToScene(string sceneName)
    {
        if (!isTransitioning && !isGameOver)
        {  
            StartCoroutine(FadeOutAndLoad(sceneName));
        }
    }
    
    public void TransitionToScene(int sceneName)
    {
        if (!isTransitioning && !isGameOver)
        { 
            StartCoroutine(FadeOutAndLoad(sceneName));
        }
    }
    
    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;
        
        fadeImage.gameObject.SetActive(true);
        
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));
            SetFadeAlpha(alpha);
            yield return null;
        }
        
        fadeImage.gameObject.SetActive(false);
    }
    
    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isTransitioning = true;
        
        // Fade out
        fadeImage.gameObject.SetActive(true);
        
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            SetFadeAlpha(alpha);
            yield return null;
        }
        
        // Load scene
        SceneManager.LoadScene(sceneName);
        
        // Fade in after scene load
        yield return StartCoroutine(FadeIn());
        
        isTransitioning = false;
    }
    
    private IEnumerator FadeOutAndLoad(int sceneIndex)
    {
        yield return FadeOutAndLoad(SceneManager.GetSceneByBuildIndex(sceneIndex).name);
    }
    
    private void SetFadeAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
        }
    }
    
    // Optional: Public method to fade without changing scene
    public void FadeOut(System.Action onFadeComplete = null)
    {
        StartCoroutine(FadeOutCoroutine(onFadeComplete));
    }
    
    public void FadeIn(System.Action onFadeComplete = null)
    {
        StartCoroutine(FadeInCoroutine(onFadeComplete));
    }
    
    private IEnumerator FadeOutCoroutine(System.Action onFadeComplete)
    {
        fadeImage.gameObject.SetActive(true);
        
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            SetFadeAlpha(alpha);
            yield return null;
        }
        
        onFadeComplete?.Invoke();
    }
    
    private IEnumerator FadeInCoroutine(System.Action onFadeComplete)
    {
        fadeImage.gameObject.SetActive(true);
        
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));
            SetFadeAlpha(alpha);
            yield return null;
        }
        
        fadeImage.gameObject.SetActive(false);
        onFadeComplete?.Invoke();
    }
}