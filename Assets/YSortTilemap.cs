using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapYSort : MonoBehaviour
{
    private TilemapRenderer tr;

    void Awake()
    {
        tr = GetComponent<TilemapRenderer>();
    }

    void LateUpdate()
    {
        tr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }
}
