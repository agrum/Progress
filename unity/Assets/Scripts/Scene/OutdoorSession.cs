using BestHTTP;
using SimpleJSON;
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
            public GameObject prefab;

            public Tile(GameObject prefab_)
            {
                prefab = prefab_;
            }

            public override void RefreshTile(Vector3Int position, ITilemap tilemap)
            {
                // Read docs for an explanation on this.
                tilemap.RefreshTile(position);
            }

            public override void GetTileData(Vector3Int position, ITilemap tileMap, ref UnityEngine.Tilemaps.TileData tileData)
            {
                tileData.gameObject = prefab;
            }
        }

        public Context.PlayerNew player;
        public string layoutId;
        public int size;
        public GameObject DummyTile;
        public Tilemap Tilemap;

        // Use this for initialization
        IEnumerator Start()
        {
            yield return StartCoroutine(App.Content.Layouts.Load());

            var layout = App.Content.Layouts.OutdoorLayouts[layoutId];

            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.prefab = DummyTile;

            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    Tilemap.SetTile(new Vector3Int(col, row, 0), tile);
                }
            }
        }
    }
}
