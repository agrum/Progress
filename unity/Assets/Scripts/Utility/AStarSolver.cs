using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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
            public bool Visited = false;
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

            static public List<Node> GenerateGridMap(Vector2 start_, Vector2 end_, float minRes_, int buffer_, List<Data.Layout.Environment> obstacles_, Data.Layout.Environment container_, float randomness_)
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
                        if (i < map.GetLength(0) - 1 && j > 0)
                        {
                            Connection.Connect(map[i, j], map[i + 1, j - 1], rnd * 1.414f);
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
                return rtn;
            }
        }

        Node Start;
        Node End;
        List<Node> Nodes;

        static public List<Node> Compute(Vector2 start_, Vector2 end_, List<Data.Layout.Environment> obstacles_, Data.Layout.Environment container_, float randomness_ = 3.0f)
        {
            AStarSolver solver = new AStarSolver();
            solver.Nodes = Node.GenerateGridMap(start_, end_, 0.1f, 2, obstacles_, container_, randomness_);
            foreach (var node in solver.Nodes)
            {
                if (node.Position == start_)
                {
                    solver.Start = node;
                }
                else if (node.Position == end_)
                {
                    solver.End = node;
                }
            }

            if (solver.Start == null || solver.End == null)
            {
                return null;
            }

            return solver.GetShortestPath();
        }

        public List<Node> GetShortestPath()
        {
            foreach (var node in Nodes)
                node.StraightLineDistanceToEnd = node.StraightLineDistanceTo(End);

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
                return null;
            }

            // if all good, return path
            Node temp = closedList[closedList.IndexOf(current)];
            if (temp == null)
            {
                return null;
            }
            path.Add(Start);
            do
            {
                path.Add(temp);
                temp = temp.NearestToStart;
            } while (temp != Start && temp != null);
            return path;
        }
    }
}