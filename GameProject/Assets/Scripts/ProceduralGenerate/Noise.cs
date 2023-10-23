using UnityEngine;

namespace TheIslandKOD
{
    public static class Noise
    {

        public enum NormalizeMode
        {
            Local,
            Global
        }
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
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, NormalizeMode normalizeMode,
                                                int octaves, float persistance, float lacunarity, Vector2 offset)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            Vector2[] octavesOffsets = new Vector2[octaves];
            System.Random random = new System.Random(seed);

            float maxPossibleHeight = 0f;
            float amplitude = 1;
            float frequancy = 1;

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000) + offset.x;
                float offsetY = random.Next(-100000, 100000) - offset.y;
                 
                octavesOffsets[i] = new Vector2(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                amplitude *= persistance;
            }

            
            // check scale that is not zero 
            if (scale <= 0)
            {
                scale = 0.001f;
            }

            float maxLocalNoiseHeight = float.MinValue;
            float minLocalNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

            //Initialize noiseMap
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    amplitude = 1;
                    frequancy = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {

                        float sampleX = (x-halfWidth + octavesOffsets[i].x) / scale * frequancy;
                        float sampleY = (y-halfHeight + octavesOffsets[i].y) / scale * frequancy;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequancy *= lacunarity;
                        
                    }

                    if (maxLocalNoiseHeight < noiseHeight)
                    {
                        maxLocalNoiseHeight = noiseHeight;
                    }
                    else if (minLocalNoiseHeight > noiseHeight)
                    {
                        minLocalNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;
                }
            }

            //smoothing noise map 
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    if (normalizeMode == NormalizeMode.Local)
                    {
                        noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                    }
                    else 
                    {
                        float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 1.8f); 
                        noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                    }
                }
            }

            return noiseMap;
        }
    }
}
