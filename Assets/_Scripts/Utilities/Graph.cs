using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Utilities
{
    public static class Graph
    {
        private static Dictionary<Vector3, Vertex> vertexDictionary = new Dictionary<Vector3, Vertex>
            {{new Vector3(), new Vertex()}};

        private static void GenerateRandomGraph(int vertexAmountTarget)
        {
            while (vertexDictionary.Count < vertexAmountTarget)
            {
                var vertex = vertexDictionary.ElementAt(Random.Range(0, vertexDictionary.Count)).Value;
                var neighborCoords = vertex.RandomNullNeighbor();
                if (neighborCoords == default) continue;

                var neighborPosition = vertex.Position + neighborCoords;
                var newVertex = new Vertex(neighborPosition);

                UpdateNeighborhoods(newVertex);

                vertexDictionary.Add(neighborPosition, newVertex);
            }
        }

        private static void UpdateNeighborhoods(Vertex vertex)
        {
            var tempNewNeighbors = new Dictionary<Vector3, Vertex>(6);

            foreach (var neighbor in vertex.Neighbors)
            {
                var otherNeighborPosition = vertex.Position + neighbor.Key;
                if (!vertexDictionary.TryGetValue(otherNeighborPosition, out var neighborVertex))
                {
                    tempNewNeighbors.Add(neighbor.Key, null);
                    continue;
                }

                tempNewNeighbors.Add(neighbor.Key, neighborVertex);

                neighborVertex.Neighbors[-neighbor.Key] = new Vertex(vertex);
                vertexDictionary[otherNeighborPosition] = new Vertex(neighborVertex);
            }

            vertex.Neighbors = new Dictionary<Vector3, Vertex>(tempNewNeighbors);
        }

        public static Dictionary<Vector3, Vertex> GetVertices(int vertexAmountTarget)
        {
            GenerateRandomGraph(vertexAmountTarget);
            var result = vertexDictionary;
            vertexDictionary = new Dictionary<Vector3, Vertex>
                {{new Vector3(), new Vertex()}};
            return result;
        }
    }

    public class Vertex
    {
        public Vertex(Vector3 position = default)
        {
            Position = position;
        }

        public Vertex(Vertex vertex)
        {
            Position = vertex.Position;
            Neighbors = new Dictionary<Vector3, Vertex>(vertex.Neighbors);
        }


        public Vector3 Position;

        public Dictionary<Vector3, Vertex> Neighbors =
            new Dictionary<Vector3, Vertex>(6)
            {
                {new Vector3(0, 0, 1), null}, {new Vector3(0, 0, -1), null}, {new Vector3(0, 1, 0), null},
                {new Vector3(0, -1, 0), null}, {new Vector3(1, 0, 0), null}, {new Vector3(-1, 0, 0), null}
            };
    }
}