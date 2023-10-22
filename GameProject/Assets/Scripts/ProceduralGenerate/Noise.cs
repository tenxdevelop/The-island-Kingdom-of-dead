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
        /// <param name="scale">scale noise map</param>
        /// <param name="octaves">count perlin layers</param>
        /// <param name="persistance">Height noise map</param>
        /// <param name="lacunarity">coordinate scale</param>
        /// <returns>noiseMap type float[,]</returns>
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            // check scale that is not zero 
            if (scale <= 0)
            {
                scale = 0.001f;
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            //Initialize noiseMap
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequancy = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {

                        float sampleX = x / scale * frequancy;
                        float sampleY = y / scale * frequancy;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequancy *= lacunarity;
                        
                    }

                    if (maxNoiseHeight < noiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (minNoiseHeight > noiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[y, x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[y, x]);
                }
            }

            return noiseMap;
        }
    }
}
