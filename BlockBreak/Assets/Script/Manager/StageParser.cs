using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Drawing;
using System;

public struct MapData
{
    public enum OBJECT_STATE
    {
        NONE,
        BOX,
        TRIANGLE,
        ITEM
    }

    public Point pos;
    public Vector2 scale;
    public OBJECT_STATE objState;
    public BoxStatus status;
}

public static class StageParser
{
    private static readonly Point mapSize = new Point(9, 9);

    public static Point MapSize => mapSize;

    public static MapData[,] CreateStageGridFromFile(string path)
    {
        MapData[,] mapLayer = null;

        int layerCount = 0;

        bool isDataParse = false;

        int nowLineNumber = 0;

        string allStream = File.ReadAllText(path);

        string[] lineArr = allStream.Split('\n');

        for(int lineIndex = 0; lineIndex < lineArr.Length; lineIndex++)
        {
            lineArr[lineIndex] = lineArr[lineIndex].Split('\r')[0];

            if(isDataParse)
            {
                string[] dataLine = lineArr[lineIndex].Split(',');

                if (layerCount == 1)
                {
                    for (int i = 0; i < mapSize.X; i++)
                        mapLayer[nowLineNumber, i] = CreateMapData(nowLineNumber, i, int.Parse(dataLine[i]));
                }
                if (layerCount == 2)
                {
                    for (int i = 0; i < mapSize.X; i++)
                        mapLayer[nowLineNumber, i].status.boxCount = int.Parse(dataLine[i]);
                }

                nowLineNumber += 1;

                if (nowLineNumber >= mapSize.Y - 1)
                    isDataParse = false;

                continue;
            }

            if (lineArr[lineIndex].Equals("[layer]"))
            {
                if(mapLayer == null)
                    mapLayer = new MapData[mapSize.Y,mapSize.X];

                isDataParse = false;
                nowLineNumber = 0;

                continue;
            }

            int indexResult = lineArr[lineIndex].IndexOf('=');

            if (indexResult != -1)
            {
                string lineStr = lineArr[lineIndex];
                if (lineStr[lineStr.Length - 1] == '1')
                    layerCount = 1;

                if (lineStr[lineStr.Length - 1] == '2')
                    layerCount = 2;

                if (lineStr.Substring(0, indexResult) == "data")
                    isDataParse = true;
                continue;
            }

            
        }

        return mapLayer;
    }

    private static MapData CreateMapData(int nowLine, int position, int parseResult)
    {
        MapData mapData = new MapData();

        mapData.pos.X = position;
        mapData.pos.Y = nowLine;
        mapData.objState = IntToMapState(parseResult);
        mapData.scale = IntToScale(parseResult);

        return mapData;
    }


    private static MapData.OBJECT_STATE IntToMapState(int num)
    {
        if (num == 1)
            return MapData.OBJECT_STATE.BOX;
        if (num == 2)
            return MapData.OBJECT_STATE.TRIANGLE;
        if (num == 3)
            return MapData.OBJECT_STATE.TRIANGLE;
        if (num == 4)
            return MapData.OBJECT_STATE.TRIANGLE;
        if (num == 5)
            return MapData.OBJECT_STATE.TRIANGLE;
        if (num == 21)
            return MapData.OBJECT_STATE.ITEM;

        return MapData.OBJECT_STATE.NONE;
    }

    private static Vector2 IntToScale(int num)
    {
        if (num == 2)
            return new Vector2(-1, 1);
        if (num == 3)
            return new Vector2(1, 1);
        if (num == 4)
            return new Vector2(1, -1);
        if (num == 5)
            return new Vector2(-1, -1);

        return new Vector2(1, 1);
    }
}
