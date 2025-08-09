using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    private float spacing = 0.3f;
    public List<GridTile> FindPath(GridTile start, GridTile end)
    {
        List<GridTile> openList = new List<GridTile>();
        List<GridTile> closedList = new List<GridTile>();

        openList.Add(start);
        while (openList.Count > 0)
        {
            GridTile currentGridTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentGridTile);
            closedList.Add(currentGridTile);

            if(currentGridTile == end)
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
                neighbour.G = GetDistance(start, neighbour);
                neighbour.H = GetDistance(end, neighbour);
                neighbour.previous = currentGridTile;
                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }    

            }
        }
        return new List<GridTile>();
    }
    private List<GridTile> GetFinishedPath(GridTile start, GridTile end)
    {
        List<GridTile> finishedPath = new List<GridTile>();

        GridTile currentTile = end;
        while (currentTile != start)
        {
            finishedPath.Add(currentTile);
            currentTile = currentTile.previous;
        }
        
        finishedPath.Reverse();
        return finishedPath;
    }

    private float GetDistance(GridTile start, GridTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }
    private List<GridTile> GetNeighbourTiles(GridTile currentGridTile)
    {
        var map = LevelGenerator.Instance.map;
        List<GridTile> neighbours = new List<GridTile>();

        bool IsSameColor(GridTile currentTile, GridTile nextTile)
        {
            if (nextTile.gridColor == CharacterColor.None || currentGridTile.gridColor == CharacterColor.None)
            {
                return true;
            }

            return currentTile.gridColor == nextTile.gridColor; //Check next tile's color
        }
        // Top
        Vector2 loc = new Vector2(currentGridTile.gridLocation.x, currentGridTile.gridLocation.y + spacing);
        if (map.TryGetValue(loc, out var top) && IsSameColor(currentGridTile, top))
            neighbours.Add(top);

        // Bottom
        loc = new Vector2(currentGridTile.gridLocation.x, currentGridTile.gridLocation.y - spacing);
        if (map.TryGetValue(loc, out var bottom) && IsSameColor(currentGridTile, bottom))
            neighbours.Add(bottom);

        // Right
        loc = new Vector2(currentGridTile.gridLocation.x + spacing, currentGridTile.gridLocation.y);
        if (map.TryGetValue(loc, out var right) && IsSameColor(currentGridTile, right))
            neighbours.Add(right);

        // Left
        loc = new Vector2(currentGridTile.gridLocation.x - spacing, currentGridTile.gridLocation.y);
        if (map.TryGetValue(loc, out var left) && IsSameColor(currentGridTile, left))
            neighbours.Add(left);
        return neighbours;
    }
}
