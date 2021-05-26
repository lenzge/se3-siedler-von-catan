using Enums;
using PlayerColor;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject robber;

    private const float r = 0.866f;
    private const float a = 1f;
    private const float d = 2f;
    private const float s = 1.73205f;
    private const float offset = 3f / 2f * a;

    public void instantiateGameBoard(Hexagon[][] gameBoard)
    {
        for (int row = 0; row < gameBoard.Length; row++)
        {
            for (int col = 0; col < gameBoard[row].Length; col++)
            {
                Hexagon hexagon = gameBoard[row][col];
                string hexagonPath = "Tiles3D/" + findPrefabByHexagonType(hexagon.getType()) + "Prefab";
                GameObject prefab = (GameObject) Resources.Load(hexagonPath);

                int offsetX = row switch
                {
                    0 => 2,
                    1 => 1,
                    2 => 1,
                    3 => 0,
                    4 => 1,
                    5 => 1,
                    6 => 2,
                    _ => 0,
                };
                
                GameObject newHexagon;
                if (row % 2 == 0)
                {
                    newHexagon = Instantiate(prefab, new Vector3(col * s + offsetX * s, 0, row / 2f * (d + a)), Rotation(hexagon, row, col));
                }
                else
                {
                    newHexagon = Instantiate(prefab, new Vector3(col * s + r + offsetX * s, 0, (row - 1) / 2f * (d + a) + offset), Rotation(hexagon, row, col));
                }

                // change hexagon numbers
                if (hexagon.isLand()) {
                    GameObject text = newHexagon.transform.GetChild(1).GetChild(0).gameObject;

                    text.GetComponent<TextMesh>().text = hexagon.getFieldNumber().ToString();
                }
            }
        }
    }
    
    private Quaternion Rotation(Hexagon hexagon, int row, int col)
    {
        //Ports need to be rotated
        if (hexagon.isPort())
        {
            switch (z: row, x: col)
            {
                case (0,0): return Quaternion.Euler(0,-120,0);
                case (0,2): return Quaternion.Euler(0,180,0);
                case (1,4): return Quaternion.Euler(0,180,0);
                case (2,0): return Quaternion.Euler(0,-60,0);
                case (3,6): return Quaternion.Euler(0,120,0);
                case (4,0): return Quaternion.Euler(0,-60,0);
                case (5,4): return Quaternion.Euler(0,60,0);
                case (6,0): return Quaternion.Euler(0,0,0);
                case (6,2): return Quaternion.Euler(0,60,0);
            }
            
        }
        return Quaternion.identity;
    }
    
    private string findPrefabByHexagonType(HEXAGON_TYPE hexagonType)
    {
        switch (hexagonType)
        {
            case HEXAGON_TYPE.WATER: return "water";
            case HEXAGON_TYPE.DESERT: return "desert";
            
            case HEXAGON_TYPE.SHEEP: return "sheep";
            case HEXAGON_TYPE.WOOD: return "wood";
            case HEXAGON_TYPE.BRICK: return "brick";
            case HEXAGON_TYPE.ORE: return "ore";
            case HEXAGON_TYPE.WHEAT: return "wheat";
            
            case HEXAGON_TYPE.PORTNORMAL: return "randomPort";
            case HEXAGON_TYPE.PORTSHEEP: return "sheepPort";
            case HEXAGON_TYPE.PORTWOOD: return "woodPort";
            case HEXAGON_TYPE.PORTBRICK: return "brickPort";
            case HEXAGON_TYPE.PORTORE: return "orePort";
            case HEXAGON_TYPE.PORTWHEAT: return "wheatPort";
            
            default: //Debug.LogError("BoardGenerator: no HexagonType set in current Hexagon");
                return null;
        }
    }

    public void placeBuilding(BUYABLES buildType, int buildId, PLAYERCOLOR buildColor)
    {
        if (buildType == BUYABLES.VILLAGE)
        {
            GameObject node = GameObject.Find(buildId.ToString());
            Vector3 nodePos = node.transform.position;
            
            buildVillage(nodePos, buildColor);
        }
        else if (buildType == BUYABLES.CITY)
        {
            GameObject node = GameObject.Find(buildId.ToString());
            Vector3 nodePos = node.transform.position;
            
            buildCity(nodePos, buildColor);
        }
        else if (buildType == BUYABLES.ROAD)
        {
            GameObject edge = GameObject.Find(buildId.ToString());
            Vector3 edgePos = edge.transform.position;
            Quaternion edgeRotation = edge.transform.rotation;
            
            BuildRoad(edgePos, edgeRotation, buildColor);
        }
    }
    
    public void buildVillage(Vector3 position, PLAYERCOLOR buildColor)
    {
        GameObject prefab;
        switch (buildColor)
        {
            case PLAYERCOLOR.RED: prefab = (GameObject) Resources.Load("PlayerObjects3D/redVillagePrefab"); break;
            case PLAYERCOLOR.BLUE: prefab = (GameObject) Resources.Load("PlayerObjects3D/blueVillagePrefab"); break;
            case PLAYERCOLOR.WHITE: prefab = (GameObject) Resources.Load("PlayerObjects3D/whiteVillagePrefab"); break;
            case PLAYERCOLOR.YELLOW: prefab = (GameObject) Resources.Load("PlayerObjects3D/yellowVillagePrefab"); break;
            default: Debug.Log("buildVillage: wrong player color"); return;
        }
        
        Instantiate(prefab, position, Quaternion.identity);
    }

    public void buildCity(Vector3 position, PLAYERCOLOR buildColor)
    {
        GameObject prefab;
        switch (buildColor)
        {
            case PLAYERCOLOR.RED: prefab = (GameObject) Resources.Load("PlayerObjects3D/redCityPrefab"); break;
            case PLAYERCOLOR.BLUE: prefab = (GameObject) Resources.Load("PlayerObjects3D/blueCityPrefab"); break;
            case PLAYERCOLOR.WHITE: prefab = (GameObject) Resources.Load("PlayerObjects3D/whiteCityPrefab"); break;
            case PLAYERCOLOR.YELLOW: prefab = (GameObject) Resources.Load("PlayerObjects3D/yellowCityPrefab"); break;
            default: Debug.Log("buildCity: wrong player color"); return;
        }
        
        Instantiate(prefab, position, Quaternion.identity);
    }

    public void BuildRoad(Vector3 position, Quaternion rotation, PLAYERCOLOR buildColor)
    {
        GameObject prefab;
        switch (buildColor)
        {
            case PLAYERCOLOR.RED: prefab = (GameObject) Resources.Load("PlayerObjects3D/redStreetPrefab"); break;
            case PLAYERCOLOR.BLUE: prefab = (GameObject) Resources.Load("PlayerObjects3D/blueStreetPrefab"); break;
            case PLAYERCOLOR.WHITE: prefab = (GameObject) Resources.Load("PlayerObjects3D/whiteStreetPrefab"); break;
            case PLAYERCOLOR.YELLOW: prefab = (GameObject) Resources.Load("PlayerObjects3D/yellowStreetPrefab"); break;
            default: Debug.Log("buildCity: wrong player color"); return;
        }
        
        Instantiate(prefab, position, Quaternion.identity);
    }
}