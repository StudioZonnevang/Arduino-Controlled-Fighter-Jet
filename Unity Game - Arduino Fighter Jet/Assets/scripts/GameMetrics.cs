using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMetrics
{
    public static int chunkCount = 7;

    public static int chunkSizeX = 400;
    public static int chunkSizeZ = 100;

    public static int cellSizeX = 10;
    public static int cellSizeZ = 10;

    public static float chunkSpawnPosZ = chunkCount * chunkSizeZ - (chunkSizeZ * 0.5f);
    public static int cellLength = (chunkSizeX / cellSizeX) * (chunkSizeZ / cellSizeZ);

    public static int ReturnRandomValue(int arrayLength)
    {
        return Random.Range(0, arrayLength);
    }
    public static int ReturnRandomValue(int arrayLength, Vector3 weights)
    {
        int val = 0;
        Random.Range(0, arrayLength);
        return val;
    }
}
