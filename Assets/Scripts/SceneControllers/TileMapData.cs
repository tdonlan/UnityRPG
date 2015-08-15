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

        //USED?  - REMOVE IF NOT
        //public List<ZoneObjectBounds> objectBoundsList = new List<ZoneObjectBounds>();

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

                    strTileArray += empty ? "." : "#";
                }
                strTileArray += System.Environment.NewLine;
            }
            
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

        public List<Point> getPath(int x1, int y1, int x2, int y2)
        {
            return PathFind.Pathfind(this.tileArray, x1, y1, x2, y2);
        }

    }
}
