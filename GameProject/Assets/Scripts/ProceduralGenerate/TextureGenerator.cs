using UnityEngine;

namespace TheIslandKOD
{
    public static class TextureGenerator
    {
        public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(colorMap);
            texture.Apply();
            return texture;
        }

        public static Texture2D TextureFromHeightMap(float[,] heightMap)
        {
            int mapWidth = heightMap.GetLength(0);
            int mapHeight = heightMap.GetLength(1);

            Color[] colorMap = new Color[mapHeight * mapWidth];

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    colorMap[y * mapWidth + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                }
            }

            return TextureFromColorMap(colorMap, mapWidth, mapHeight);
        }
    }
}
