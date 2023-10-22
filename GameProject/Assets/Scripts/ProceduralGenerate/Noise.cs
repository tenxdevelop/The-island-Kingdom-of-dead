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
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed,
                                                int octaves, float persistance, float lacunarity, Vector2 offset)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            Vector2[] octavesOffsets = new Vector2[octaves];
            System.Random random = new System.Random(seed); 
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000) + offset.x;
                float offsetY = random.Next(-100000, 100000) + offset.y;
                 
                octavesOffsets[i] = new Vector2(offsetX, offsetY);
                
            }
            // check scale that is not zero 
            if (scale <= 0)
            {
                scale = 0.001f;
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;

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

                        float sampleX = (x-halfWidth) / scale * frequancy + octavesOffsets[i].x;
                        float sampleY = (y-halfHeight) / scale * frequancy + octavesOffsets[i].y;

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

            //smoothing noise map 
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }
}
