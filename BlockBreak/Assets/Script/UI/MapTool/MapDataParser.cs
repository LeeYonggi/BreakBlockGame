using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;


public static class MapDataParser 
{
    public static void MapDataSave(string path, List<Node[]> nodes)
    {
        if (path.Length == 0) return;

        using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            AddText(file, "Line_Length" + "\t" + nodes.Count + "\n");
            for(int i = nodes.Count - 1; i >= 0; i--)
            {
                for(int n = 0; n < nodes[i].Length; n++)
                {
                    if (nodes[i][n] == null)
                    {
                        AddText(file, $"{-1} ");
                        continue;
                    }
                    var nodeData = nodes[i][n].NodeData;

                    AddText(file, $"{(int)nodeData.state}/{nodeData.count}/{(int)nodeData.dirState} ");
                }

                AddText(file, "\n");
            }
        }
    }

    public static void FTPMapDataSave(string path, List<Node[]> nodes)
    {
        if (path.Length == 0) return;

        string strData = $"Line_Length\t{nodes.Count}\n";

        for (int i = nodes.Count - 1; i >= 0; i--)
        {
            for (int n = 0; n < nodes[i].Length; n++)
            {
                if (nodes[i][n] == null)
                {
                    strData += $"{-1} ";
                    continue;
                }
                var nodeData = nodes[i][n].NodeData;

                strData += $"{(int)nodeData.state}/{nodeData.count}/{(int)nodeData.dirState} ";
            }

            strData += "\n";
        }

        FTPParse.FtpUpload(path, Encoding.UTF8.GetBytes(strData));
    }

    private static void AddText(FileStream fs, string value)
    {
        byte[] info = Encoding.UTF8.GetBytes(value);

        fs.Write(info, 0, info.Length);
    }

    public static List<NodeData[]> GetMapData(string path)
    {
        if (path.Length == 0) return null;

        string fullString = File.ReadAllText(path);

        string[] lines = fullString.Split('\n');

        List<NodeData[]> nodes = new List<NodeData[]>();

        int listCount = int.Parse(lines[0].Split('\t')[1]);

        Array.Reverse(lines);

        for(int i = 0; i < listCount; i++)
        {
            nodes.Add(new NodeData[9]);

            string[] lineStr = lines[i + 1].Split(' ');

            for(int iNodeData = 0; iNodeData < lineStr.Length - 1; iNodeData++)
            {
                if (lineStr[iNodeData].Equals("-1"))
                    continue;

                string[] nodeData = lineStr[iNodeData].Split('/');

                nodes[i][iNodeData] = new NodeData(int.Parse(nodeData[1]), 
                                        (NodeData.NODE_STATE)int.Parse(nodeData[0]), 
                                        (NodeData.DIRECTION_STATE)int.Parse(nodeData[2]));
            }
        }

        return nodes;
    }

    public static List<NodeData[]> GetFTPMapData(string path)
    {
        if (path.Length == 0) return null;

        string fullString = FTPParse.GetFTPTextFile(path);

        string[] lines = fullString.Split('\n');

        List<NodeData[]> nodes = new List<NodeData[]>();

        int listCount = int.Parse(lines[0].Split('\t')[1]);

        Array.Reverse(lines);

        for (int i = 0; i < listCount; i++)
        {
            nodes.Add(new NodeData[9]);

            string[] lineStr = lines[i + 1].Split(' ');

            for (int iNodeData = 0; iNodeData < lineStr.Length - 1; iNodeData++)
            {
                if (lineStr[iNodeData].Equals("-1"))
                    continue;

                string[] nodeData = lineStr[iNodeData].Split('/');

                nodes[i][iNodeData] = new NodeData(int.Parse(nodeData[1]),
                                        (NodeData.NODE_STATE)int.Parse(nodeData[0]),
                                        (NodeData.DIRECTION_STATE)int.Parse(nodeData[2]));
            }
        }

        return nodes;
    }

}
