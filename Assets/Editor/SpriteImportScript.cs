using UnityEditor;
using UnityEngine;

public class SpriteImportSettings : AssetPostprocessor
{
    // This method is called automatically when an asset is imported.
    void OnPreprocessTexture()
    {
        // Ensure the asset is a texture and is being imported as a sprite
        TextureImporter importer = (TextureImporter)assetImporter;
        if (importer.textureType == TextureImporterType.Sprite)
        {
            // Set Pixels Per Unit to 32
            importer.spritePixelsPerUnit = 32;

            // Optional: Set additional default settings
            importer.filterMode = FilterMode.Point; // Set filter mode to Point (useful for pixel art)
            importer.textureCompression = TextureImporterCompression.Uncompressed; // No compression for sprites
        }
    }
}
