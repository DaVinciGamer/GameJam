using UnityEngine;

public class SpriteAnimatorController : MonoBehaviour
{
    public Texture2D spriteSheet;    // The sprite sheet texture
    public int columns;              // Number of columns in the sprite sheet
    public int rows;                 // Number of rows in the sprite sheet
    public float framesPerSecond = 10f; // Frame rate
    public int id;                   // ID for the instance

    private Sprite[] frames;         // Array of sprite frames
    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;
    private bool isAnimating = true; // To control animation state

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
            return;
        }

        frames = SliceSpriteSheet(spriteSheet, columns, rows);
        if (frames.Length > 0)
        {
            spriteRenderer.sprite = frames[0]; // Initialize with the first frame
        }
    }

    void Update()
    {
        if (isAnimating && frames.Length > 0)
        {
            timer += Time.deltaTime;
            if (timer >= 1f / framesPerSecond)
            {
                timer -= 1f / framesPerSecond;
                currentFrame = (currentFrame + 1) % frames.Length;
                spriteRenderer.sprite = frames[currentFrame];
            }
        }
    }

    public void Hide()
    {
        if (spriteRenderer != null)
        {
            isAnimating = false;
            spriteRenderer.enabled = false; // Hide the sprite
            Debug.Log($"Hiding instance with ID: {id}"); // Debug log to verify the method call
        }
    }

    public void Show()
    {
        if (spriteRenderer != null)
        {
            isAnimating = true;
            spriteRenderer.enabled = true; // Show the sprite
            Debug.Log($"Showing instance with ID: {id}"); // Debug log to verify the method call
        }
    }

    public void HideInstanceById(int instanceId)
    {
        if (id == instanceId)
        {
            Hide();
        }
    }

    private Sprite[] SliceSpriteSheet(Texture2D spriteSheet, int columns, int rows)
    {
        if (spriteSheet == null)
        {
            Debug.LogError("Sprite sheet texture is not assigned.");
            return new Sprite[0];
        }

        int spriteWidth = spriteSheet.width / columns;
        int spriteHeight = spriteSheet.height / rows;
        Sprite[] sprites = new Sprite[columns * rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Rect rect = new Rect(x * spriteWidth, spriteSheet.height - (y + 1) * spriteHeight, spriteWidth, spriteHeight);
                sprites[y * columns + x] = Sprite.Create(spriteSheet, rect, new Vector2(0.5f, 0.5f));
            }
        }

        return sprites;
    }
}
