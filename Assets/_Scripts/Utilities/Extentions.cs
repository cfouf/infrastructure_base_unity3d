using System.Collections.Generic;
using System.Linq;
using _Scripts.Entities;
using UnityEngine;

namespace _Scripts.Utilities
{
    public static class Extensions
    {
        public static Vector3 RandomNullNeighbor(this Vertex vertex)
        {
            var nullNeighbors = vertex.Neighbors
                .Where(x => x.Value == null)
                .Select(x => x.Key)
                .ToList();
            return nullNeighbors.Count == 0 ? default : nullNeighbors[Random.Range(0, nullNeighbors.Count)];
        }

        public static Vector3 GetCenterOfMass(this List<Vector3> cubes)
        {
            var centerOfMass = new Vector3();
            return
                cubes.Aggregate(centerOfMass, (current, cube) => current + cube) / cubes.Count;
        }

        public static Dictionary<Vector3, Vertex> RemoveUnseenVertices(this Dictionary<Vector3, Vertex> vertices)
        {
            return vertices
                .Where(x => x.Value.Neighbors
                    .Any(y => y.Value == null))
                .ToDictionary(x => x.Key, x => x.Value);
        }
        
        
        public static float Integral(this AnimationCurve curve, float tStart, float tEnd) {
            var integral = 0f;
            const int steps = 100;
            var dt = (tEnd - tStart) / steps;
        
            for (var i = 0; i < steps; i++) {
                var t1 = tStart + i * dt;
                var t2 = tStart + (i + 1) * dt;
                var y1 = curve.Evaluate(t1);
                var y2 = curve.Evaluate(t2);
                var area = dt * (y1 + y2) / 2f;
                integral += area;
            }
        
            return integral;
        }
    }
}