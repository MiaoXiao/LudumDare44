using UnityEngine;

public class XFlipSpriteDirection : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private float lastPosX;

    private void Awake()
    {
        lastPosX = transform.position.x;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Update()
    {
        _spriteRenderer.flipX = lastPosX > transform.position.x;
        lastPosX = transform.position.x;
    }
}
