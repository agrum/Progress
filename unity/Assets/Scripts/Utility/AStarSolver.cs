using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Extensions;

namespace Assets.Scripts.Utility
{
    public class AStarSolver
    {
        public class Node
        {
            public class Connection
            {
                readonly public Node Node;
                readonly public float Cost;

                public Connection(Node node_, float cost_)
                {
                    Node = node_;
                    Cost = cost_;
                }

                static public void Connect(Node n1_, Node n2_, float cost_)
                {
                    if (n1_ == null || n2_ == null)
                    {
                        return;
                    }
                    n1_.Connections.Add(new Connection(n2_, cost_));
                    n2_.Connections.Add(new Connection(n1_, cost_));
                }
            }

            public Vector2 Position;
            public float MinCostToStart = 0;
            public List<Connection> Connections = new List<Connection>();
            public float StraightLineDistanceToEnd = 0;
            public Node NearestToStart;
            public float Weight = 0;
            public float Cost = 0;

            public Node(Vector2 position_)
            {
                Position = position_;
            }

            public float StraightLineDistanceTo(Node other_)
            {
                return (Position - other_.Position).magnitude;
            }

            public float F
            {
                get
                {
                    if (StraightLineDistanceToEnd != -1 && Cost != -1)
                        return StraightLineDistanceToEnd + Cost;
                    else
                        return -1;
                }
            }

            static public Grid GenerateGridMap(Vector2 start_, Vector2 end_, float minRes_, int buffer_, List<Data.Layout.Environment> obstacles_, Data.Layout.Environment container_, float randomness_)
            {
                Debug.Assert(minRes_ > 0.001);
                Debug.Assert(buffer_ > 0);
                Vector2 min = new Vector2(Mathf.Min(start_.x, end_.x), Mathf.Min(start_.y, end_.y));
                float spreadX = Mathf.Abs(end_.x - start_.x);
                float spreadY = Mathf.Abs(end_.y - start_.y);
                float spread = Mathf.Max(spreadX, spreadY);
                float size = Mathf.Min(spread / minRes_, 20.0f);
                float cellSize = spread / size;
                float cellRadiusSqr = 0.25f * cellSize * cellSize;
                Node[,] map = new Node[2 * buffer_ + Mathf.RoundToInt(size * spreadX / spread), 2 * buffer_ + Mathf.RoundToInt(size * spreadY / spread)];
                for (int i = 0; i < map.GetLength(0); ++i)
                {
                    for (int j = 0; j < map.GetLength(1); ++j)
                    {
                        var node = new Vector2(min.x + (i - buffer_) * cellSize, min.y + (j - buffer_) * cellSize);

                        //snap node to start and end
                        if ((node - start_).sqrMagnitude < cellRadiusSqr)
                        {
                            node = start_;
                        }
                        else if ((node - end_).sqrMagnitude < cellRadiusSqr)
                        {
                            node = end_;
                        }

                        //skip if masked
                        if (container_ != null && !container_.Contains(node))
                        {
                            continue;
                        }
                        if (obstacles_ != null)
                        {
                            foreach (var obstacle in obstacles_)
                            {
                                if (obstacle.Contains(node))
                                {
                                    continue;
                                }
                            }
                        }

                        map[i, j] = new Node(node);
                        float rnd = cellSize * Random.value * randomness_;
                        if (i > 0)
                        {
                            Connection.Connect(map[i, j], map[i - 1, j], rnd);
                        }
                        if (j > 0)
                        {
                            Connection.Connect(map[i, j], map[i, j - 1], rnd);
                        }
                        if (i > 0 && j > 0)
                        {
                            Connection.Connect(map[i, j], map[i - 1, j - 1], rnd * 1.414f);
                        }
                        if (i > 0 && j < map.GetLength(1) - 1)
                        {
                            Connection.Connect(map[i, j], map[i - 1, j + 1], rnd * 1.414f);
                        }
                    }
                }

                List<Node> rtn = new List<Node>(map.GetLength(0) * map.GetLength(1));
                for (int i = 0; i < map.GetLength(0); ++i)
                {
                    for (int j = 0; j < map.GetLength(1); ++j)
                    {
                        if (map[i, j] != null)
                        {
                            rtn.Add(map[i, j]);
                        }
                    }
                }
                if (rtn.Count == 0)
                {
                    return null;
                }
                return new Grid(rtn, cellSize);
            }

            public void Disconnect()
            {
                foreach (var connection in Connections)
                {
                    connection.Node.Connections.RemoveAll(x => x.Node == this);
                }
                Connections.Clear();
            }
        }

        public class Grid
        {
            public List<Node> Nodes = new List<Node>();
            public float CellSize = 0;

            public Grid(List<Node> nodes_, float cellSize_)
            {
                Nodes = nodes_;
                CellSize = cellSize_;
            }
        }

        Node Start;
        Node End;
        List<Node> Nodes;

        static public List<Node> GetFractured(Data.Layout.Environment toFracture_, Data.Layout.Environment container_, List<Data.Layout.Environment> obstacles_, float randomness_ = 3.0f)
        {
            var rtn = new List<Node>();
            AStarSolver solver = new AStarSolver();

            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);
            foreach (var edge in toFracture_.EdgeList)
            {
                min.x = Mathf.Min(min.x, edge.Position.x);
                min.y = Mathf.Min(min.y, edge.Position.y);
                max.x = Mathf.Max(max.x, edge.Position.x);
                max.y = Mathf.Max(max.y, edge.Position.y);
            }
            var grid = Node.GenerateGridMap(min, max, 0.1f, 2, obstacles_, container_, randomness_);
            if (grid.Nodes.Count == 0)
            {
                grid = null;
            }
            solver.Nodes = grid.Nodes;

            for (var i = 0; i < toFracture_.NumEdges; ++i)
            {
                solver.Start = null;
                solver.End = null;
                var desriedStart = toFracture_[i].Position;
                var desriedEnd = toFracture_[i + 1].Position;
                var closestStartDistSq = float.MaxValue;
                var closestEndDistSq = float.MaxValue;
                foreach (var node in solver.Nodes)
                {
                    if (node.Connections.Count > 0 && (node.Position - desriedStart).sqrMagnitude < closestStartDistSq)
                    {
                        solver.Start = node;
                        closestStartDistSq = (node.Position - desriedStart).sqrMagnitude;
                    }
                    else if (node.Connections.Count > 0 && (node.Position - desriedEnd).sqrMagnitude < closestEndDistSq)
                    {
                        solver.End = node;
                        closestEndDistSq = (node.Position - desriedEnd).sqrMagnitude;
                    }
                }

                var path = solver.GetShortestPath();
                if (path.Count == 0)
                {
                    rtn.Add(solver.End);
                    continue;
                }
                for (int j = 1; j < path.Count - 2; ++j)
                {
                    path[j].Disconnect();
                }
                path.RemoveAt(0);
                rtn.AddRange(path);
            }
            return rtn;
        }

        private List<Node> GetShortestPath()
        {
            foreach (var node in Nodes)
            {
                node.StraightLineDistanceToEnd = node.StraightLineDistanceTo(End);
                node.NearestToStart = null;
                node.Cost = 0;
            }

            List<Node> path = new List<Node>();
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            List<Node.Connection> neighbors;
            Node current = Start;

            // add start node to Open List
            openList.Add(Start);

            while (openList.Count != 0 && !closedList.Exists(x => x.Position == End.Position))
            {
                current = openList[0];
                openList.Remove(current);
                closedList.Add(current);
                if (current == null)
                {
                    neighbors = current.Connections;
                }
                neighbors = current.Connections;


                foreach (Node.Connection cnn in neighbors)
                {
                    var n = cnn.Node;
                    if (!closedList.Contains(n))
                    {
                        if (!openList.Contains(n))
                        {
                            n.NearestToStart = current;
                            n.Cost = n.Weight + cnn.Cost + n.NearestToStart.Cost;
                            openList.Add(n);
                            openList = openList.OrderBy(node => node.F).ToList<Node>();
                        }
                    }
                }
            }

            // construct path, if end was not closed return null
            if (!closedList.Exists(x => x.Position == End.Position))
            {
                return path;
            }

            // if all good, return path
            Node temp = closedList[closedList.IndexOf(current)];
            while (temp != null)
            {
                path.Add(temp);
                temp = temp.NearestToStart;
            } 
            path.Reverse();
            return path;
        }
    }
}