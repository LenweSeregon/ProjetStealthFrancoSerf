using UnityEngine;
using System.Collections;
using System.IO;

public class PNGToSpriteTool
{
    private static PNGToSpriteTool instance;
    public static PNGToSpriteTool Instance
    {
        get
        {
            if (instance == null)
                instance = new PNGToSpriteTool();
            return instance;
        }
    }

    public Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f)
    {
        Texture2D SpriteTexture = LoadTexture(filePath);
        Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), pixelsPerUnit);

        return newSprite;
    }

    public Texture2D LoadTexture(string filePath)
    {
        Texture2D tex2D;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex2D = new Texture2D(2, 2);
            if (tex2D.LoadImage(fileData))
                return tex2D;
        }
        return null;
    }
}
