using BestHTTP;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class OutdoorSession : Session
    {
        public class TileSignature
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

            public TileSignature(Region[] surroundings_, Region center_)
            {
                Surroundings = surroundings_;
                Center = center_;
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

            public bool GetRegion(Vector2 coordinate, TileSignature.Region parentRegion, out TileSignature.Region region)
            {
                if (data.Contains(coordinate))
                {
                    region = new TileSignature.Region(data.Variety, parentRegion.Elevation + data.HeightDeltaAsInt());
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

        public string LayoutId;
        public int Size;
        public int BoundarySize;
        public GameObject GrassPrefab;
        public GameObject DirtPrefab;
        public GameObject RockPrefab;
        public GameObject SnowPrefab;
        public GameObject OceanPrefab;

        List<Environment> environments = new List<Environment>();
        readonly Dictionary<TileSignature.Region, GameObject> tilePrefabs = new Dictionary<TileSignature.Region, GameObject>();
        Dictionary<Data.Layout.Environment.EVariety, GameObject> prefabs = new Dictionary<Data.Layout.Environment.EVariety, GameObject>();

        // Use this for initialization
        IEnumerator Start()
        {
            yield return StartCoroutine(App.Content.Layouts.Load());

            var layout = App.Content.Layouts.OutdoorLayouts[LayoutId].Fractured();
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
            TileSignature.Region[,] regions = new TileSignature.Region[totalSize, totalSize];
            Color32[] worldTextureData = new Color32[totalSize * totalSize];
            Color32[] oceanTextureData = new Color32[totalSize * totalSize];

            ComputeRegions(regions, layout, totalSize);
            ComputeTextures(worldTextureData, oceanTextureData, regions, totalSize);

            Texture2D worldTexture = new Texture2D(totalSize, totalSize, TextureFormat.RGBA32, false);
            worldTexture.filterMode = FilterMode.Point;
            worldTexture.SetPixels32(worldTextureData, 0);
            worldTexture.Apply();
            WriteTexture(worldTexture, "World");
            Texture2D oceanTexture = new Texture2D(totalSize, totalSize, TextureFormat.RGBA32, false);
            oceanTexture.filterMode = FilterMode.Trilinear;
            oceanTexture.SetPixels32(oceanTextureData, 0);
            oceanTexture.Apply();
            WriteTexture(oceanTexture, "Ocean");

            for (int col = 0; col < totalSize; col++)
            {
                for (int row = 0; row < totalSize; row++)
                {
                    var tile = Instantiate(GetTile(regions[col, row]));
                    PlaceTile(tile, regions[col, row].Variety, worldTextureData[col + row * totalSize], col, row);
                }
            }

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            Dictionary<string, Pair<Material, List<CombineInstance>>> combines = new Dictionary<string, Pair<Material, List<CombineInstance>>>();

            for (int i = 0; i < meshFilters.Length; ++i)
            {
                var combineInstance = new CombineInstance();
                combineInstance.mesh = meshFilters[i].sharedMesh;
                combineInstance.transform = meshFilters[i].transform.localToWorldMatrix;

                var combineMaterial = meshFilters[i].gameObject.GetComponent<MeshRenderer>().material;

                if (!combines.ContainsKey(combineMaterial.name)) 
                {
                    combines.Add(combineMaterial.name, new Pair<Material, List<CombineInstance>>(combineMaterial, new List<CombineInstance>()));
                }

                combines[combineMaterial.name].Second.Add(combineInstance);
                Destroy(meshFilters[i].gameObject.GetComponent<MeshRenderer>());
                Destroy(meshFilters[i]);
            }

            foreach (var combine in combines)
            {
                var terrainObject = new GameObject(combine.Key);
                terrainObject.transform.parent = gameObject.transform;
                var meshFilter = terrainObject.AddComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                meshFilter.mesh.CombineMeshes(combine.Value.Second.ToArray());
                var meshRenderer = terrainObject.AddComponent<MeshRenderer>();
                meshRenderer.material = combine.Value.First;
                meshRenderer.material.SetVector("_WorldSize", new Vector4(totalSize, totalSize, 1, 1));
                meshRenderer.material.SetTexture("_WorldTex", worldTexture);
                meshRenderer.material.SetTexture("_OceanTex", oceanTexture);
                terrainObject.transform.gameObject.SetActive(true);
            }

            Vector2 spawnPosition = new Vector2(0, 0);
            Vector2 validSpawnPosition = GetValidSpawnPosition(spawnPosition, regions);
            Player.transform.position = new Vector3(validSpawnPosition.x, validSpawnPosition.y, -1);

            loaded = true;
        }

        private void ComputeRegions(TileSignature.Region[,] regions, Data.Layout.Outdoor layout, int totalSize)
        {
            for (int col = 0; col < totalSize; col++)
            {
                for (int row = 0; row < totalSize; row++)
                {
                    regions[col, row] = GetRegionInLayout(layout, col, row);
                }
            }
        }

        private TileSignature.Region GetRegionInLayout(Data.Layout.Outdoor layout, int x, int y)
        {
            TileSignature.Region region = new TileSignature.Region(layout.BaselineEnvironment, 0);
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

        public GameObject GetTile(TileSignature.Region region)
        {
            GameObject tile;
            if (tilePrefabs.TryGetValue(region, out tile))
            {
                return tile;
            }

            tilePrefabs.Add(region, prefabs[region.Variety]);
            return prefabs[region.Variety];
        }

        private void ComputeTextures(Color32[] worldTexture, Color32[] oceanTexture, TileSignature.Region[,] regions, int totalSize)
        {
            for (int col = 0; col < totalSize; col++)
            {
                for (int row = 0; row < totalSize; row++)
                {
                    ComputePixel(worldTexture, oceanTexture, regions, col, row, totalSize);
                }
            }
        }

        private void ComputePixel(Color32[] worldTexture, Color32[] oceanTexture, TileSignature.Region[,] regions, int x, int y, int totalSize)
        {
            Color pixel = new Vector4();
            var region = regions[x, y];
            float boundaryDistance = 0;
            Material mat = GetTile(region).GetComponentInChildren<MeshRenderer>().material;
            int maxBoundaryCheck = mat.mainTexture.width / Enum.GetNames(typeof(Data.Layout.Environment.EVariety)).Length;

            if (x > 0 && regions[x - 1, y] == region)
            {
                boundaryDistance = Mathf.Max(0, worldTexture[x - 1 + y * regions.GetLength(0)].r * maxBoundaryCheck - 1);
            }
            else if (y > 0 && regions[x, y - 1] == region)
            {
                boundaryDistance = Mathf.Max(0, worldTexture[x + (y - 1) * regions.GetLength(0)].r * maxBoundaryCheck - 1);
            }

            TileSignature.Region foundNewRegion = region;
            while (boundaryDistance < maxBoundaryCheck)
            {
                float circle = Mathf.PI * 2.0f;
                float step = circle / Mathf.CeilToInt(circle * (boundaryDistance + 1.0f));
                for (float i = 0; i < circle; i += step)
                {
                    int steppedX = Mathf.FloorToInt(x + 0.5f + (boundaryDistance + 1.0f) * Mathf.Cos(i));
                    int steppedY = Mathf.FloorToInt(y + 0.5f + (boundaryDistance + 1.0f) * Mathf.Sin(i));
                    if (steppedX < 0 || steppedY < 0 || steppedX >= regions.GetLength(0) || steppedY >= regions.GetLength(1))
                    {
                        continue;
                    }

                    if (regions[steppedX, steppedY].Variety != region.Variety)
                    {
                        foundNewRegion = regions[steppedX, steppedY];
                        break;
                    }
                }
                if (foundNewRegion.Variety != region.Variety)
                {
                    break;
                }
                ++boundaryDistance;
            }

            if (foundNewRegion.Variety == region.Variety)
            {
                boundaryDistance = maxBoundaryCheck - 1;
            }
            if (boundaryDistance < 0)
            {
                boundaryDistance = 0;
            }

            pixel.r = ((float)(int)region.Variety + (boundaryDistance / maxBoundaryCheck)) / Enum.GetNames(typeof(Data.Layout.Environment.EVariety)).Length;
            pixel.g = (((float) (int) foundNewRegion.Variety)) / mat.mainTexture.height;
            pixel.b = UnityEngine.Random.Range(0.0f, 0.1f);
            pixel.a = boundaryDistance / maxBoundaryCheck;
            worldTexture[x + y * totalSize] = pixel;

            pixel.r = region.Variety == Data.Layout.Environment.EVariety.Ocean ? ((boundaryDistance + 1.0f) / maxBoundaryCheck) : 0.0f;
            pixel.g = pixel.r;
            pixel.b = pixel.r;
            pixel.a = 1.0f;
            oceanTexture[x + y * totalSize] = pixel;
        }

        Vector2 GetValidSpawnPosition(Vector2 candidateSpawnPosition, TileSignature.Region[,] regions)
        {
            for (float boundaryDistance = 1; boundaryDistance < BoundarySize * 2.0f; ++boundaryDistance)
            {
                float circle = Mathf.PI * 2.0f;
                float step = circle / Mathf.CeilToInt(circle * boundaryDistance);
                for (float i = 0; i < circle; i += step)
                {
                    int steppedX = Mathf.FloorToInt(candidateSpawnPosition.x + boundaryDistance * Mathf.Cos(i));
                    int steppedY = Mathf.FloorToInt(candidateSpawnPosition.y + boundaryDistance * Mathf.Sin(i));
                    if (steppedX < 0 || steppedY < 0 || steppedX >= regions.GetLength(0) || steppedY >= regions.GetLength(1))
                    {
                        continue;
                    }

                    if (regions[steppedX, steppedY].Variety != Data.Layout.Environment.EVariety.Rock
                        && regions[steppedX, steppedY].Variety != Data.Layout.Environment.EVariety.Ocean)
                    {
                        return new Vector2(steppedX, steppedY);
                    }
                }
            }
               
            return candidateSpawnPosition;
        }

        void WriteTexture(Texture2D texture, string filename)
        {
            var bytes = texture.EncodeToPNG();
            var dirPath = Application.dataPath + "/../SaveImages/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + filename + ".png", bytes);
        }

        void PlaceTile(GameObject tile, Data.Layout.Environment.EVariety variety, Color32 worldPixel, int col, int row)
        {
            if (variety == Data.Layout.Environment.EVariety.Rock)
            {
                float height = 2.0f + (worldPixel.a / 255.0f + 0.5f) * UnityEngine.Random.value * 4.0f;
                tile.transform.position = new Vector3(col + 0.5f, row + 0.5f, -height);
            }
            else if (variety == Data.Layout.Environment.EVariety.Ocean)
            {
                float height = -worldPixel.a / 32.0f + UnityEngine.Random.value * 0.5f;
                tile.transform.Find("Floor").localPosition = new Vector3(0, 0, 5.8f - height);
                tile.transform.position = new Vector3(col + 0.5f, row + 0.5f, 0);
            }
            else
            {
                tile.transform.position = new Vector3(col + 0.5f, row + 0.5f, 0);
            }
            tile.transform.parent = transform;
        }
    }
}
