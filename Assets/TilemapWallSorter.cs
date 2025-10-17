using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapWallSorter : MonoBehaviour
{
    [Header("Sorting Settings")]
    public float yOffset = 10f; // Adjust this based on your scene
    public int precision = 100;
    
    void Start()
    {
        TilemapRenderer renderer = GetComponent<TilemapRenderer>();
        
        // Add offset to ensure positive values
        float sortingY = transform.position.y + yOffset;
        renderer.sortingOrder = Mathf.RoundToInt(-sortingY * precision);
        
        renderer.sortingLayerName = "Walls";
    }
}