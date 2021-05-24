using System;
using PlayerColor;

public class Edge
{
    private int[] adjacentNodesPos = new int[2];
    private int[] adjacentEdgesPos = new int[4];
    private PLAYERCOLOR occupant = PLAYERCOLOR.NONE;

    public Edge()
    {

    }

    public void setAdjacentNodePos(int nodePos, int index)
    {
        adjacentNodesPos[index] = nodePos;
    }

    public int[] getAdjacentNodesPos()
    {
        return adjacentNodesPos;
    }

    public void setAdjacentEdge(int edge, int index)
    {
        adjacentEdgesPos[index] = edge;
    }

    public int[] getAdjacentEdges()
    {
        return adjacentEdgesPos;
    }
    
    public void setOccupant(PLAYERCOLOR occupant)
    {
        this.occupant = occupant;
    }

    public PLAYERCOLOR getOccupant()
    {
        return occupant;
    }
}