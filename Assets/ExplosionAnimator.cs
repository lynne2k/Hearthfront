using UnityEngine;

public class ExplosionAnimator : MonoBehaviour
{
    public Sprite[] explosionSprites; // Array to hold the explosion sprite frames
    public Sprite[] maskSprites;      // Array to hold the mask sprite frames
    private SpriteRenderer explosionRenderer;
    private SpriteRenderer maskRenderer;
    private Material material;

    private int currentFrame = 0;
    public float animationSpeed = 0.1f; // How fast the animation progresses
    private float timeSinceLastFrame = 0f;

    void Start()
    {
        // Get the sprite renderers for both explosion and mask
        explosionRenderer = GetComponent<SpriteRenderer>();
        maskRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>(); // Assuming mask is a child

        // Create a new material instance using the custom shader
        material = new Material(Shader.Find("Custom/ExplosionSpriteAlphaMask"));
        explosionRenderer.material = material;

        // Set initial textures
        material.SetTexture("_ExplosionTex", explosionSprites[0].texture);
        material.SetTexture("_MaskTex", maskSprites[0].texture);
    }

    void Update()
    {
        // Handle the animation frame update
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= animationSpeed)
        {
            currentFrame = (currentFrame + 1) % explosionSprites.Length;

            // Update the explosion and mask sprites
            explosionRenderer.sprite = explosionSprites[currentFrame];
            maskRenderer.sprite = maskSprites[currentFrame];

            // Update the material with the new textures
            material.SetTexture("_ExplosionTex", explosionSprites[currentFrame].texture);
            material.SetTexture("_MaskTex", maskSprites[currentFrame].texture);

            timeSinceLastFrame = 0f;
        }
    }
}