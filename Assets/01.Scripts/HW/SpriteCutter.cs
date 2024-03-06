using System.Collections;
using UnityEngine;

public class SpriteCutter : MonoBehaviour
{
    public GameObject spritePrefab;
    public int numberOfPieces = 6;
    public float fallSpeed = 5f;

    void Start()
    {
        StartCoroutine(CutSprite());
    }

    IEnumerator CutSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Texture2D texture = spriteRenderer.sprite.texture;

        // Ensure the texture is readable
        var backupTexture = texture;
        if (!texture.isReadable)
        {
            // Create a copy of the texture that is readable
            var tempTexture = new Texture2D(texture.width, texture.height);
            tempTexture.SetPixels(texture.GetPixels());
            tempTexture.Apply();
            texture = tempTexture;
        }

        for (int i = 0; i < numberOfPieces; i++)
        {
            // Use a coroutine to avoid blocking the main thread
            yield return StartCoroutine(CreatePiece(texture));
        }

        // Restore the original texture if it was replaced
        if (texture != backupTexture)
        {
            Destroy(texture);
        }
    }

    IEnumerator CreatePiece(Texture2D texture)
    {
        int width = Random.Range(50, 100);
        int height = Random.Range(50, 100);

        int x = Random.Range(0, texture.width - width);
        int y = Random.Range(0, texture.height - height);

        Color[] pixels = texture.GetPixels(x, y, width, height);
        Texture2D newTexture = new Texture2D(width, height);
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        GameObject piece = Instantiate(spritePrefab, transform.position, Quaternion.identity);
        piece.GetComponent<SpriteRenderer>().sprite = Sprite.Create(newTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));

        Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
        rb.gravityScale = 1f;

        // Use a coroutine to delay the fall
        yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
        rb.velocity = Vector2.down * fallSpeed;
    }
}
