using BestHTTP;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class OutdoorSession : MonoBehaviour
    {
        public class Tile : TileBase
        {
            public class Signature
            {
                public class Region : IEquatable<Region>
                {
                    public readonly Data.Layout.Environment.EVariety Variety;
                    public readonly int Elevation;

                    public Region(Data.Layout.Environment.EVariety variety_, int elevation_)
                    {
                        Variety = variety_;
                        Elevation = elevation_;
                    }
                    public override int GetHashCode()
                    {
                        return (((int) Variety) << 10) + Elevation;
                    }
                    public override bool Equals(object obj)
                    {
                        return Equals(obj as Region);
                    }
                    public bool Equals(Region obj)
                    {
                        return obj != null && obj.Variety == Variety && obj.Elevation == Elevation;
                    }
                }

                public Region[] Surroundings = new Region[4]; //up, left, down, right
                public Region Center;

                public Signature(Region[] surroundings_, Region center_)
                {
                    Surroundings = surroundings_;
                    Center = center_;
                }
            }

            public GameObject prefab;

            public override void RefreshTile(Vector3Int position, ITilemap tilemap)
            {
                // Read docs for an explanation on this.
                tilemap.RefreshTile(position);
            }

            public override void GetTileData(Vector3Int position, ITilemap tileMap, ref TileData tileData)
            {
                tileData.gameObject = prefab;
            }
        }

        public class Environment
        {
            Data.Layout.Environment data;
            readonly List<Environment> nestedEnvironments = new List<Environment>();

            public Environment(Data.Layout.Environment data_)
            {
                data = data_;
                foreach (var environment in data.NestedEnvironments)
                {
                    nestedEnvironments.Add(new Environment(environment));
                }
            }

            public bool GetRegion(Vector2 coordinate, Tile.Signature.Region parentRegion, out Tile.Signature.Region region)
            {
                if (data.Contains(coordinate))
                {
                    region = new Tile.Signature.Region(data.Variety, parentRegion.Elevation + data.HeightDeltaAsInt());
                    foreach (var nestedEnvironment in nestedEnvironments)
                    {
                        nestedEnvironment.GetRegion(coordinate, region, out region);
                    }
                    return true;
                }
                region = parentRegion;
                return false;
            }
        }

        public Context.PlayerNew Player;
        public string LayoutId;
        public int Size;
        public int BoundarySize;
        public GameObject GrassPrefab;
        public GameObject DirtPrefab;
        public GameObject RockPrefab;
        public GameObject SnowPrefab;
        public GameObject OceanPrefab;
        public Tilemap Tilemap;

        List<Environment> environments = new List<Environment>();
        readonly Dictionary<Tile.Signature.Region, Tile> tilePrefabs = new Dictionary<Tile.Signature.Region, Tile>();
        Dictionary<Data.Layout.Environment.EVariety, GameObject> prefabs = new Dictionary<Data.Layout.Environment.EVariety, GameObject>();

        // Use this for initialization
        IEnumerator Start()
        {
            yield return StartCoroutine(App.Content.Layouts.Load());

            var layout = App.Content.Layouts.OutdoorLayouts[LayoutId];
            foreach (var environment in layout.Environments)
            {
                environments.Add(new Environment(environment));
            }

            prefabs.Add(Data.Layout.Environment.EVariety.Grass, GrassPrefab);
            prefabs.Add(Data.Layout.Environment.EVariety.Dirt, DirtPrefab);
            prefabs.Add(Data.Layout.Environment.EVariety.Rock, RockPrefab);
            prefabs.Add(Data.Layout.Environment.EVariety.Snow, SnowPrefab);
            prefabs.Add(Data.Layout.Environment.EVariety.Ocean, OceanPrefab);

            int totalSize = Size + BoundarySize + BoundarySize;
            Tile.Signature.Region[,] regions = new Tile.Signature.Region[totalSize, totalSize];

            for (int col = 0; col < totalSize; col++)
            {
                for (int row = 0; row < totalSize; row++)
                {
                    regions[col, row] = GetRegionInLayout(layout, col, row);
                }
            }

            for (int col = 0; col < totalSize; col++)
            {
                for (int row = 0; row < totalSize; row++)
                {
                    Tilemap.SetTile(new Vector3Int(col, row, 0), GetTile(regions[col, row]));
                }
            }
        }

        public Tile.Signature.Region GetRegionInLayout(Data.Layout.Outdoor layout, int x, int y)
        {
            Tile.Signature.Region region = new Tile.Signature.Region(layout.BaselineEnvironment, 0);
            if (x < BoundarySize || y < BoundarySize || x > Size + BoundarySize || y > Size + BoundarySize)
            {
                return region;
            }

            Vector2 coordinate = new Vector2(x - BoundarySize, y - BoundarySize);
            coordinate /= Size;
            coordinate *= layout.Size;
            coordinate += new Vector2(0.5f, 0.5f) + layout.Center - layout.Size * 0.5f;

            foreach (var environment in environments)
            {
                if (environment.GetRegion(coordinate, region, out region))
                {
                    return region;
                }
            }

            return region;
        }

        public Tile GetTile(Tile.Signature.Region region)
        {
            Tile tile;
            if (tilePrefabs.TryGetValue(region, out tile))
            {
                return tile;
            }
            tile = ScriptableObject.CreateInstance<Tile>();
            tile.prefab = prefabs[region.Variety];
            tilePrefabs.Add(region, tile);
            return tile;
        }
    }
}
