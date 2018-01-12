using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Static class that does not inherit from monobehaviour as we will be calling the function from another (CityGenerator) script.
public static class NoiseGenerator
{
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random rng = new System.Random(seed);

        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            //Creating a random base value for our offset based on seed. This will ensure that different seeds produce different results.
            float offsetX = rng.Next(-1000, 1000) + offset.x;
            float offsetY = rng.Next(-1000, 1000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(scale < -0)
        {   //Ensuring our scale is not < 0.
            scale = 0.001f;
        }

        //Setting the max/min heights as the lowest/highest possibe float. This ensures that the later comparisons we do, work without fail the first time.
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //For every tile in the map (mapWidth * mapHeight)...
                float freq = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    //Generating the inital sample value based on octaves, scale, offset.
                    float sampleX = (x - halfWidth) / scale * freq + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * freq + octaveOffsets[i].y;

                    //Generate the height of our tile.
                    float perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlin;
                }

                //If the generated tile has a value larger than the current max, or lower than the current min, update these bounds.
                //The 1st values to pass through here will 100% update the bounds, as our current min/max are at the extremes.
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                //Finally set the height of our current tile.
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {   //Once we have gone through every tile, to ensure we have values at 0 and 1, lerp between the max/min height.
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        //Return the final noisemap, complete with (mapHeight*mapWidth) no. of tiles.
        return noiseMap;
    }
}
