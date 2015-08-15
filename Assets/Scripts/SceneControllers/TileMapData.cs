using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityRPG
{

    public class ZoneObjectBounds
    {
        public Bounds bounds;
        public bool isActive;
        public long index;
            
    }

    public class TileMapData
    {
        public List<Bounds> collisionBoundsList = new List<Bounds>();
        public Bounds spawnBounds;
        public List<Bounds> objectBounds = new List<Bounds>();

        //Battle Bounds
        public List<Bounds> playerSpawnBounds = new List<Bounds>();
        public List<Bounds> enemySpawnBounds = new List<Bounds>();
        public List<Bounds> npcSpawnBounds = new List<Bounds>();

        public Tile[,] tileArray;

        public TileMapData(GameObject tileMapGameObject)
        {
            loadCollisionRectListFromPrefab(tileMapGameObject);
            loadObjectBounds(tileMapGameObject);
            loadSpawn(tileMapGameObject);

            loadTileArray(tileMapGameObject);
            
        }

        private void loadObjectBounds(GameObject tileMapGameObject)
        {

            objectBounds = getObjectBoundsFromType(tileMapGameObject, "objects");
            playerSpawnBounds = getObjectBoundsFromType(tileMapGameObject, "PlayerStart");
            enemySpawnBounds = getObjectBoundsFromType(tileMapGameObject, "EnemyStart");
            npcSpawnBounds = getObjectBoundsFromType(tileMapGameObject, "NPCStart");
        }

        //Calculate the 2D array of tiles, given the tile prefab
        private void loadTileArray(GameObject tileMapGameObject)
        {

            string strTileArray = "";

            Bounds mapBounds = tileMapGameObject.GetComponentInChildren<Renderer>().bounds;

            int tileWidth = (int)Math.Ceiling(mapBounds.size.x / Tile.TILE_SIZE);
            int tileHeight = (int)Math.Ceiling(mapBounds.size.y / Tile.TILE_SIZE);

            tileArray = new Tile[tileWidth, tileHeight];
            for (int y = 0; y < tileHeight; y++)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    Vector3 center = new Vector3(x * Tile.TILE_SIZE + (Tile.TILE_SIZE / 2), -y * Tile.TILE_SIZE + (Tile.TILE_SIZE / 2), 0);
                    Vector3 size = new Vector3(Tile.TILE_SIZE, Tile.TILE_SIZE);
                    Bounds tileBounds = new Bounds(center, size);
                    bool empty = !checkCollision(tileBounds);

                    tileArray[x, y] = new Tile(x, y, empty);

                    //Extra metadata on tile
                    tileArray[x, y].tileSpriteLookup = getTileSpriteLookup(tileBounds, x, y, empty);

                    strTileArray += empty ? "." : "#";
                }
                strTileArray += System.Environment.NewLine;
            }
            
        }

        private  TileSpriteLookup getTileSpriteLookup(Bounds testBounds, int x, int y, bool isEmpty)
        {
            TileSpriteType tileSpriteType = TileSpriteType.Floor;
            if (!isEmpty)
            {
                tileSpriteType = TileSpriteType.Wall;
            }
            else if (checkObjectBounds(enemySpawnBounds, testBounds))
            {
                tileSpriteType = TileSpriteType.EnemyStart;
            }
            else if (checkObjectBounds(playerSpawnBounds, testBounds))
            {
                tileSpriteType = TileSpriteType.PlayerStart;
            }
            else if (checkObjectBounds(npcSpawnBounds, testBounds))
            {
                tileSpriteType = TileSpriteType.NPCStart;
            }

            TileSpriteLookup tileSpriteLookup = new TileSpriteLookup('_',"",0,isEmpty,tileSpriteType);
            return tileSpriteLookup;
        }


        private List<Bounds> getObjectBoundsFromType(GameObject tileMapGameObject, string objectName)
        {
            List<Bounds> objectBounds = new List<Bounds>();
            Transform objectChild = tileMapGameObject.transform.FindChild(objectName);
            if (objectChild != null)
            {
                foreach (var box in objectChild.GetComponentsInChildren<BoxCollider2D>())
                {
                    objectBounds.Add(box.bounds);
                }
            }

            return objectBounds;
        }

        private void loadSpawn(GameObject tileMapGameObject)
        {
            //for now, default to the first object as the spawn point
            if (objectBounds.Count > 0)
            {
                spawnBounds = objectBounds[0];
            }
          
        }

        //spawn point is the current node location we are on, or defaults to object 1
        public Bounds getSpawnPoint(int objectIndex)
        {
            if(objectBounds.Count > objectIndex){
                return objectBounds[objectIndex];
            }
            else
            {
                return objectBounds[0];
            }
           
        }

        private void loadCollisionRectListFromPrefab(GameObject tileMapGameObject)
        {
            Transform collisionChild = tileMapGameObject.transform.FindChild("collision");

            if (collisionChild != null)
            {
                foreach (var box in collisionChild.GetComponentsInChildren<BoxCollider2D>())
                {
                    collisionBoundsList.Add(box.bounds);
                }
            }

        }

        public bool checkCollision(Bounds testBounds)
        {
            foreach (var b in collisionBoundsList)
            {
                if (b.Intersects(testBounds))
                {
                    return true;
                }
            }
            return false;
        }

        

        public int checkObjectCollision(Bounds testBounds)
        {
            for(int i=0;i<objectBounds.Count;i++)
            {
                if (objectBounds[i].Intersects(testBounds))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool checkObjectBounds(List<Bounds> boundsList, Bounds testBounds)
        {
            foreach (var b in boundsList)
            {
                if (b.Intersects(testBounds))
                {
                    return true;
                }
            }
            return false;
        }

        //Given a point, check if it is in the player spawn bounds (used to find spawn tiles in battle maps)
        public bool checkPlayerSpawnCollision(Point centerPoint)
        {
            Bounds checkBounds = new Bounds(new Vector3(centerPoint.x, centerPoint.y, 0), new Vector3(Tile.TILE_SIZE, Tile.TILE_SIZE));
            foreach (var playerSpawn in playerSpawnBounds)
            {
                if (checkBounds.Intersects(playerSpawn))
                {
                    return true;
                }
            }
            return false;
        }

        //Given a point, check if it is in the enemy spawn bounds (used to find spawn tiles in battle maps)
        public bool checkEnemySpawnCollision(Point centerPoint)
        {
            Bounds checkBounds = new Bounds(new Vector3(centerPoint.x, centerPoint.y, 0), new Vector3(Tile.TILE_SIZE, Tile.TILE_SIZE));
            foreach (var enemySpawn in enemySpawnBounds)
            {
                if (checkBounds.Intersects(enemySpawn))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Point> getPath(int x1, int y1, int x2, int y2)
        {
            return PathFind.Pathfind(this.tileArray, x1, y1, x2, y2);
        }

    }
}
