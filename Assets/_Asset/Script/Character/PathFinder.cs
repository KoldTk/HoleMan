using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    private int spacing = 1;
    public List<Node> FindPath(Node start, Node end)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(start);
        while (openList.Count > 0)
        {
            Node currentGridTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentGridTile);
            closedList.Add(currentGridTile);

            if(currentGridTile.nodePos == end.nodePos)
            {
                //Create path
                return GetFinishedPath(start, end);

            }    
            var neighbourTiles = GetNeighbourTiles(currentGridTile);

            foreach (var neighbour in neighbourTiles)
            {
                if (neighbour.isBlocked || closedList.Contains(neighbour))
                {
                    continue;
                }
                neighbour.G = currentGridTile.G + GetDistance(start, neighbour);
                float newG = neighbour.G;
                neighbour.H = GetDistance(end, neighbour);
                neighbour.previous = currentGridTile;
                
                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }   
            }
        }
        return new List<Node>();
    }
    private List<Node> GetFinishedPath(Node start, Node end)
    {
        List<Node> finishedPath = new List<Node>();

        Node currentNode = end;
        while (currentNode != null && currentNode != start)
        {
            finishedPath.Add(currentNode);
            currentNode = currentNode.previous;
        }
        if (currentNode == start)
        { 
            //Add start position to path
            finishedPath.Add(start);
        }
        finishedPath.Reverse();
        return finishedPath;
    }

    private float GetDistance(Node start, Node neighbour)
    {
        return Mathf.Abs(start.nodePos.x - neighbour.nodePos.x) + Mathf.Abs(start.nodePos.z - neighbour.nodePos.z);
    }
    private List<Node> GetNeighbourTiles(Node currentNode)
    {
        var map = LevelGenerator.Instance.map;
        List<Node> neighbours = new List<Node>();
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(spacing,0,0), //Right
            new Vector3Int(-spacing,0,0), //Left
            new Vector3Int(0,0,spacing), //Top
            new Vector3Int(0,0,-spacing) //Bottom
        };
        bool IsSameColor(Node currentTile, Node nextTile)
        {
            if (currentTile.gridColor == nextTile.gridColor)
            {
                return true;
            }
            return (nextTile.gridColor == CharacterColor.None || currentTile.gridColor == CharacterColor.None);
        }

        foreach (var dir in directions)
        {
            Vector3Int location = currentNode.nodePos + dir;
            if (map.TryGetValue(location, out var nextNode) && IsSameColor(currentNode, nextNode))
            {
                neighbours.Add(nextNode);
            }
        }    
        return neighbours;
    }
}
