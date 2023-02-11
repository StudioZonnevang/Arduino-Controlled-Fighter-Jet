using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMetrics
{
    public static int chunkCount = 7;

    public static int chunkSizeX = 400;
    public static int chunkSizeZ = 100;

    public static int cellSizeX = 20;
    public static int cellSizeZ = 20;

    public static float chunkSpawnPosZ = chunkCount * chunkSizeZ - (chunkSizeZ * 0.5f);
}
