using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Drawing;
using System;


public static class StageParser
{
    private static readonly Point mapSize = new Point(9, 9);

    public static Point MapSize => mapSize;

    public static BoxStatus[,] CreateStageGridFromFile(string path)
    {
        BoxStatus[,] mapLayer = null;

        int layerCount = 0;

        bool isDataParse = false;

        int nowLineNumber = 0;

        TextAsset txt = Resources.Load(path, typeof(TextAsset)) as TextAsset;

        string allStream = txt.text;
        
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
                        mapLayer[nowLineNumber, i].boxCount = int.Parse(dataLine[i]);
                }

                nowLineNumber += 1;

                if (nowLineNumber >= mapSize.Y - 1)
                    isDataParse = false;

                continue;
            }

            if (lineArr[lineIndex].Equals("[layer]"))
            {
                if(mapLayer == null)
                    mapLayer = new BoxStatus[mapSize.Y,mapSize.X];

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

    public static List<BoxStatus[]> CreateTestStageFromFile(FileInfo fileInfo)
    {
        if (fileInfo == null) return null;
        if (fileInfo.FullName.Length == 0) return null;

        string fullString = FTPParse.GetFTPTextFile(fileInfo.Name);

        string[] lines = fullString.Split('\n');

        List<BoxStatus[]> boxes = new List<BoxStatus[]>();

        int listCount = int.Parse(lines[0].Split('\t')[1]);

        Array.Reverse(lines);

        for(int i = 0; i < listCount; i++)
        {
            boxes.Add(new BoxStatus[9]);

            string[] lineStr = lines[i + 1].Split(' ');

            for(int iNodeData = 0; iNodeData < lineStr.Length - 1; iNodeData++)
            {
                if (lineStr[iNodeData].Equals("-1"))
                    continue;

                string[] nodeData = lineStr[iNodeData].Split('/');

                boxes[i][iNodeData].boxCount = int.Parse(nodeData[1]);
                boxes[i][iNodeData].objState = IntToTestState(int.Parse(nodeData[0]));
                boxes[i][iNodeData].rotation = int.Parse(nodeData[2]);
                boxes[i][iNodeData].pos = new Point(iNodeData, i);
                boxes[i][iNodeData].scale = new Vector2(1, 1);
            }
        }

        return boxes;
    }

    private static BoxStatus CreateMapData(int nowLine, int position, int parseResult)
    {
        BoxStatus mapData = new BoxStatus();

        mapData.pos.X = position;
        mapData.pos.Y = nowLine;
        mapData.objState = IntToMapState(parseResult);
        mapData.scale = IntToScale(parseResult);

        return mapData;
    }


    private static BoxStatus.OBJECT_STATE IntToMapState(int num)
    {
        if (num == 1)
            return BoxStatus.OBJECT_STATE.BOX;
        if (num == 2)
            return BoxStatus.OBJECT_STATE.TRIANGLE;
        if (num == 3)
            return BoxStatus.OBJECT_STATE.TRIANGLE;
        if (num == 4)
            return BoxStatus.OBJECT_STATE.TRIANGLE;
        if (num == 5)
            return BoxStatus.OBJECT_STATE.TRIANGLE;
        if (num == 21)
            return BoxStatus.OBJECT_STATE.ITEM;

        return BoxStatus.OBJECT_STATE.NONE;
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

    private static BoxStatus.OBJECT_STATE IntToTestState(int num)
    {
        if (num == 1)
            return BoxStatus.OBJECT_STATE.BOX;
        if (num == 2)
            return BoxStatus.OBJECT_STATE.TRIANGLE;
        if (num == 3)
            return BoxStatus.OBJECT_STATE.ITEM;

        return BoxStatus.OBJECT_STATE.NONE;
    }

}
