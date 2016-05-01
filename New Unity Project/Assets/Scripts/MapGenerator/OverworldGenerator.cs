using UnityEngine;
using System;

public class OverworldGenerator : MonoBehaviour {

    private int numberOfSmothings = 5;
    /*If more than wallExtensionConstant neighbouring squares are walls, make this square a wall.*/
    private int wallExtensionConstant = 4;
    /*If less than spaceExtensionConstant neighbouring squares are space, make this square a space.*/
    private int spaceExstensionConstant = 4;

    public GameObject[] allTheMatter;
    public GameObject[] allTheBackground;

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;
    private System.Random pseudoRandom;

    [Range(0, 100)]
    public int randomFillPercent;

    int[,] mapFront;
    int[,] mapBackground;
    private int[,] mapDummy;

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
    void generateMap() {

        if (useRandomSeed) {
            seed = System.DateTime.Now.ToString();
        }

        pseudoRandom = new System.Random(seed.GetHashCode());

        mapFront = new int[width, height];
        mapBackground = new int[width, height];
        mapDummy = new int[width, height];

        randomFillMap();

        for (int i = 0; i < numberOfSmothings; i++) {
            smoothMap();
        }

        cutOffFloating();

        fillBackground();

        enrichMap();

        drawTiles();

        
    }

    /* Fills map[] with random entries. */
    void randomFillMap() {

        /* Fill the inner Map*/
        for (int x = 1; x < width-1; x++) {
            for (int y = 1; y < height-1; y++) {
                    mapFront[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }

        /* Left  border */
        for (int y = 1; y < height - 1; y++) {
            if (y < height / 2) mapFront[0, y] = 0;
            else mapFront[0, y] = 0;
        }
        /* Right border */
        for (int y = 1; y < height - 1; y++) {
            if (y < height / 2) mapFront[width -1, y] = 0;
            else mapFront[width -1, y] = 0;
        }
        /* Lower border */
        for (int x = 0; x < width; x++) mapFront[x, 0] = 1;
        /* Upper border */
        for(int x = 0; x < width; x++) mapFront[x,height-1] =0;
    }

    /* Smoothes the map. To change the variables look at the top of this document. */
    void smoothMap() {
        int neighbourWallTiles;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                neighbourWallTiles = getSurroundingWallCount(x, y);

                if (neighbourWallTiles > wallExtensionConstant) {
                    mapFront[x, y] = 1;
                }
                else if (neighbourWallTiles < spaceExstensionConstant) {
                    mapFront[x, y] = 0;
                }
            }
        }
    }

    /* For a given tile, this counts the number of walltiles around it. */
    int getSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += mapFront[neighbourX, neighbourY];
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
    void drawTiles() {
        if (mapFront != null && allTheMatter != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (mapFront[x, y] > 0) {
                        Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 0);
                        GameObject block = Instantiate(allTheMatter[mapFront[x, y]-1], pos, Quaternion.identity) as GameObject;
                        block.transform.parent = transform;
                    }
                    if (mapBackground[x,y] > 0) {
                        Vector3 pos = new Vector3(-width / 2 + x + .5f, -height / 2 + y + .5f, 1);
                        GameObject block = Instantiate(allTheBackground[mapBackground[x, y] - 1], pos, Quaternion.identity) as GameObject;
                        block.transform.parent = transform;
                    }
                }
            }
        }
    }

    /* This is a mathematical function, which represents the overall shape of the overworld. */
    int overworldFunction(int gridX, int gridY) {
        int functionValue = (int) ((height / 4) * Math.Cos( 1.0 * gridX * 2 * Math.PI / width) + (height/4));
        if (gridY < functionValue && (gridX < width / 4 || 3 * width / 4 < gridX)) return 1; //
        if (gridY < height / 4 && (width / 4 < gridX || gridX < 3 * width / 4)) return 1;
        return 0;
    }

    /* Removes floating tiles, by making the map the connected component of the lower left tile. */
    void cutOffFloating() {        
        getConnectedComponent(0, 0, 1);

        swapMaps(ref mapFront, ref mapDummy);
    }

    /* Recursivly maps out the connected component (typevise) of the tile it first started. The result is noted in mapDummy. */
    private void getConnectedComponent(int gridX, int gridY, int tileType) {
        resetMapDummy();

        getConnectedComponentRecurssion(gridX, gridY, tileType);
    }

    private void getConnectedComponentRecurssion(int gridX, int gridY, int tileType) {
        if (mapFront[gridX, gridY] == tileType && mapDummy[gridX, gridY] == 0) {
            mapDummy[gridX, gridY] = 1;

            if (gridX - 1 >= 0) getConnectedComponentRecurssion(gridX - 1, gridY, tileType);
            if (gridX + 1 < width) getConnectedComponentRecurssion(gridX + 1, gridY, tileType);

            if (gridY - 1 >= 0) getConnectedComponentRecurssion(gridX, gridY - 1, tileType);
            if (gridY + 1 < height) getConnectedComponentRecurssion(gridX, gridY + 1, tileType);

        }
    }
    
    /* Generates none earth tiles. */
    void enrichMap() {
        if (mapFront != null && allTheMatter != null && mapBackground != null) {
            for (int type = 1; type < RessourceDensity.NumberOfRessources; type++) {
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        if (mapFront[x, y] != 0 && allTheMatter[type].GetComponent<Copper>() != null) {
                            mapFront[x, y] = (pseudoRandom.Next(0, 100) < allTheMatter[type].GetComponent<Copper>().Percent) ? type + 1 : mapFront[x, y];
                            mapBackground[x, y] = mapFront[x, y];
                        }
                    }
                }
            }
        }
    }

    /* Generates the background tile information and puts it into mapBackground. */
    void fillBackground() {
        if (mapFront != null) {
            getConnectedComponent((width - 1)/2, height - 1, 0);

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    mapBackground[x, y] = 1 - mapDummy[x,y];
                }
            }
        }
    }
    
    /* Fills the mapDummy with 0's. */
    void resetMapDummy() {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                mapDummy[x, y] = 0;
            }
        }
    }

    void swapMaps(ref int[,] map1, ref int[,] map2) {
        int[,] justAMapPointerForSwaping;

        justAMapPointerForSwaping = map1;
        map1 = map2;
        map2 = justAMapPointerForSwaping;
    }
}




