using UnityEngine;
using System;

public class OverworldGenerator : MonoBehaviour {

    private int _numberOfSmothings = 5;
    /*If more than wallExtensionConstant neighbouring squares are walls, make this square a wall.*/
    private int _wallExtensionConstant = 4;
    /*If less than spaceExtensionConstant neighbouring squares are space, make this square a space.*/
    private int _spaceExstensionConstant = 4;

    public GameObject[] _allTheMatter;
    public GameObject[] _allTheBackground;

    public int _width;
    public int _height;

    [Range(0, 100)]
    public int _randomFillPercent;
    


    public string _seed;
    public bool _useRandomSeed;
    private System.Random _pseudoRandom;

    void Start() {
        generateMap();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            for(int x = 0; x < 10; x++) {
                generateMap();
            }
            
        }
    }

    /* The map generation process is managed here. */
    private void generateMap() {

        if (_useRandomSeed) {
            _seed = System.DateTime.Now.ToString();
        }

        _pseudoRandom = new System.Random(_seed.GetHashCode());

        TerrainMap terrainMap = new TerrainMap(_width, _height);

        terrainMap.MapFront        = randomFillMap(terrainMap.MapFront);

        for (int i = 0; i < _numberOfSmothings; i++) {
            terrainMap.MapFront    = smoothMap(terrainMap.MapFront);
        }
        
        terrainMap.MapFront        = cutOffFloating(terrainMap.MapFront);
        
        terrainMap.MapBackground   = fillBackground(terrainMap);
        
        terrainMap                 = enrichMap(terrainMap);

        whatsInThisMap(terrainMap);

        drawTiles(terrainMap);
    }

    /* Fills map[] with random entries. */
    private int[,] randomFillMap(int[,] map) {

        /* Fill the inner Map*/
        for (int x = 1; x < _width-1; x++) {
            for (int y = 1; y < _height - 1; y++) {
                map[x, y] = (_pseudoRandom.Next(0, 100) < _randomFillPercent) ? 1 : 0;
            }
        }

        /* Lower border */
        for (int x = 0; x < _width; x++) map[x, 0] = 1;
        /* Upper border */
        for(int x = 0; x < _width; x++) map[x, _height - 1] = 0;

        return map;
    }

    /* Smoothes the map. To change the variables look at the top of this document. */
    private int[,] smoothMap(int[,] map) {
        int neighbourWallTiles;

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                neighbourWallTiles = getSurroundingWallCount(x, y, map);

                if (neighbourWallTiles > _wallExtensionConstant) {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < _spaceExstensionConstant) {
                    map[x, y] = 0;
                }
            }
        }

        return map;
    }

    /* Removes floating tiles, by making the map the connected component of the lower left tile. */
    private int[,] cutOffFloating(int[,] map) {
        return getConnectedComponent(0, 0, 1, map);
    }

    /* For a given tile, this counts the number of walltiles around it. */
    private int getSurroundingWallCount(int gridX, int gridY, int[,] map) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < _width && neighbourY >= 0 && neighbourY < _height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }
        
        return wallCount + overworldFunction(gridX, gridY);
    }

    /* Generates the blocks, as defined in map[]. */
    private void drawTiles(TerrainMap terrainMap) {
        if (terrainMap.MapFront != null && _allTheMatter != null) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    if (terrainMap.MapFront[x, y] > 0) {
                        Vector3 pos = new Vector3(-_width / 2 + x + .5f, -_height / 2 + y + .5f, 0);
                        GameObject block = Instantiate(_allTheMatter[terrainMap.MapFront[x, y] -1], pos, Quaternion.identity) as GameObject;
                        block.transform.parent = transform;
                    }

                    if (terrainMap.MapBackground[x,y] > 0) {
                        Vector3 pos = new Vector3(-_width / 2 + x + .5f, -_height / 2 + y + .5f, 1);
                        GameObject block = Instantiate(_allTheBackground[terrainMap.MapBackground[x, y] -1], pos, Quaternion.identity) as GameObject;
                        block.transform.parent = transform;
                    }
                }
            }
        }
    }

    /* This is a mathematical function, which represents the overall shape of the overworld. */
    private int overworldFunction(int gridX, int gridY) {
        int functionValue = (int) ((_height / 4) * Math.Cos( 1.0 * gridX * 2 * Math.PI / _width) + _height/2);

        if (gridY < functionValue && (gridX < _width / 4 || 3 * _width / 4 < gridX)) return 1;

        if (gridY < _height / 4 && (_width / 4 < gridX || gridX < 3 * _width / 4)) return 1;

        return 0;
    }

    /* Recursivly maps out the connected component (typevise) of the tile it first started. */
    private int[,] getConnectedComponent(int gridX, int gridY, int tileType, int[,] map) {
        int[,] mapResult = new int[_width,_height];

        for (int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                mapResult[x, y] = 1 - tileType;
            }
        }

        getConnectedComponentRecurssion(gridX, gridY, tileType, map, mapResult);

        return mapResult;
    }

    private void getConnectedComponentRecurssion(int gridX, int gridY, int tileType, int[,] map, int[,] mapResult) {
        if (map[gridX, gridY] == tileType && mapResult[gridX, gridY] != tileType) {
            mapResult[gridX, gridY] = tileType;

            if (gridX - 1 >= 0) getConnectedComponentRecurssion(gridX - 1, gridY, tileType, map, mapResult);
            if (gridX + 1 < _width) getConnectedComponentRecurssion(gridX + 1, gridY, tileType, map, mapResult);

            if (gridY - 1 >= 0) getConnectedComponentRecurssion(gridX, gridY - 1, tileType, map, mapResult);
            if (gridY + 1 < _height) getConnectedComponentRecurssion(gridX, gridY + 1, tileType, map, mapResult);

        }
    }

    /* Generates the background tile information and puts it into mapBackground. */
    private int[,] fillBackground(TerrainMap terrainMap) {
        int[,] mapResult = new int[_width, _height];

        if (terrainMap.MapFront != null) {
            mapResult = getConnectedComponent((_width - 1) / 2, _height - 1, 0, terrainMap.MapFront);
        }

        return mapResult;
    }

    /* Generates none earth tiles. */
    private TerrainMap enrichMap(TerrainMap terrainMap) {
        TerrainMap terrainMapResult = new TerrainMap(_width, _height);

        if (terrainMap.MapFront != null && _allTheMatter != null && terrainMap.MapBackground != null) {
            for (int typeOfMatter = 0; typeOfMatter < MapSettings.NumberOfRessources; typeOfMatter++) {
                
                for (int x = 0; x < _width; x++) {
                    for (int y = 0; y < _height; y++) {
                        if (terrainMap.MapFront[x, y] > 0 && _allTheMatter[typeOfMatter].GetComponent<Matter>() != null) {
                            
                            if (1.0 * _pseudoRandom.Next(0, 10000) / 10000 < _allTheMatter[typeOfMatter].GetComponent<Matter>().Percent) {
                                veinGenerator(terrainMapResult, x, y, _allTheMatter[typeOfMatter].GetComponent<Matter>().AverageQuantity, typeOfMatter+1);
                            }
                        }
                    }
                }
            }
        }

        return terrainMapResult;
    }


    private void veinGenerator(TerrainMap terrainMap, int gridX, int gridY, int averageQuantity, int typeOfMatter) {
        int randomDirection; //0 = right, 1 = down, 2 = left, 3 = up

        terrainMap.MapFront[gridX, gridY] = typeOfMatter;
        terrainMap.MapBackground[gridX, gridY] = typeOfMatter;
        

        for (int i = 1; i < averageQuantity;i++) {
            randomDirection = _pseudoRandom.Next(0, 3);

            if (randomDirection == 0 && gridX + 1 < _width) {
                if (terrainMap.MapFront[gridX + 1, gridY] != 0) gridX++;
                else randomDirection++;
            }

            if (randomDirection == 1 && gridY > 0) {
                if (terrainMap.MapFront[gridX, gridY - 1] != 0) gridY--;
                else randomDirection++;
            }

            if (randomDirection == 2 && gridX > 0) {
                if (terrainMap.MapFront[gridX - 1, gridY] != 0) gridX--;
                else randomDirection++;
            }

            if (randomDirection == 3 && gridY + 1 < _height) {
                if (terrainMap.MapFront[gridX, gridY + 1] != 0) gridY++;
                else randomDirection++;
            }

            terrainMap.MapFront[gridX, gridY] = typeOfMatter;
            terrainMap.MapBackground[gridX, gridY] = typeOfMatter;
        }
    }



    private void whatsInThisMap(TerrainMap terrainMap) {
        int[] counter = { 0, 0, 0, 0 };
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                counter[terrainMap.MapFront[x, y]]++;
            }
        }
        Debug.Log("EmptySpace = " + counter[0] + "  Earth = " + counter[1] + "  Stone = " + counter[2] + "  Copper = " + counter[3]);
    }


}




