using BestHTTP;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class OutdoorSession : MonoBehaviour
    {
        public Context.PlayerNew player;
        public string layoutId;
        public int size;
        public Context.Terrain.Tile[,] tiles;

        // Use this for initialization
        IEnumerator Start()
        {
            yield return StartCoroutine(App.Content.Layouts.Load());

            var layout = App.Content.Layouts.OutdoorLayouts[layoutId];
            tiles = new Context.Terrain.Tile[size, size];

            for (int col = 0; col < tiles.GetLength(0); col++)
            {
                for (int row = 0; row < tiles.GetLength(1); row++)
                {
                    tiles[col, row] = new Context.Terrain.Tile();
                }
            }
        }
    }
}
