using UnityEngine;

namespace TheIslandKOD
{
    public static class Noise
    {
        /// <summary>
        /// Create NoiseMap with perlin noise
        /// </summary>
        /// <param name="mapWidth">Noise map Width</param>
        /// <param name="mapHeight">Noise map Height</param>
        /// <param name="scale"></param>
        /// <returns>noiseMap type float[,]</returns>
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            // check scale that is not zero 
            if (scale <= 0)
            {
                scale = 0.001f;
            }

            //Initialize noiseMap
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float sampleX = x / scale;
                    float sampleY = y / scale;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                    noiseMap[x, y] = perlinValue;
                }
            }

            return noiseMap;
        }
    }
}
