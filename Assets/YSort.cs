    using UnityEngine;

    public class YSort : MonoBehaviour
    {
        private SpriteRenderer sr;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        void LateUpdate()
        {
            // Multiply by -100 to keep it an integer and flip sign (lower Y = higher order)
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        }
    }
