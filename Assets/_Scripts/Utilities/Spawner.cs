using System.Collections.Generic;
using System.Linq;
using _Scripts.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Utilities
{
    public static class Spawner
    {
        public static List<Construction> CreateConstructions(int amountOfConstructions, int sizeOfConstruction)
        {
            var constructions = new List<Construction>();

            var cube = Resources.Load<GameObject>("Cube");
            for (var i = 0; i < amountOfConstructions; i++)
            {
                var vertices = Graph.GetVertices(sizeOfConstruction);

                var centerOfMass = vertices.Values
                    .Select(x => x.Position)
                    .ToList()
                    .GetCenterOfMass();

                var construction = new GameObject($"Construction {i + 1}").AddComponent<Construction>();
                var constructionRb = construction.GetComponent<Rigidbody>();
                constructionRb.mass = vertices.Count;
                constructionRb.centerOfMass = centerOfMass;
                constructionRb.useGravity = false;

                var coordinatesWhereToSpawn = vertices
                    .RemoveUnseenVertices()
                    .Values
                    .Select(x => x.Position)
                    .ToList();

                SpawnCubes(cube, construction, coordinatesWhereToSpawn);
                CreateConstructionMesh(construction);

                constructions.Add(construction);

                construction.transform.position =
                    new Vector3(sizeOfConstruction * (i / 2 + 1) * Mathf.Pow(-1, i), 10, 0) - centerOfMass;
            }

            return constructions;
        }

        private static void SpawnCubes(GameObject cube, Construction construction,
            IEnumerable<Vector3> coordinatesWhereToSpawn)
        {
            var spawnedCubes = coordinatesWhereToSpawn.Select(vertex =>
                Object.Instantiate(cube, vertex, Quaternion.identity));


            foreach (var newCube in spawnedCubes)
            {
                newCube.transform.parent = construction.transform;
                newCube.AddComponent<Cube>();
                construction.cubes.Add(newCube.GetComponent<Cube>());
            }
        }

        private static void CreateConstructionMesh(Construction construction)
        {
            var constructionMesh = new Mesh();

            constructionMesh.CombineMeshes(construction.cubesColliders
                .Select(cubeCollider => cubeCollider.gameObject
                    .GetComponent<MeshFilter>())
                .Select(meshFilter =>
                    new CombineInstance
                        {mesh = meshFilter.sharedMesh, transform = meshFilter.transform.localToWorldMatrix})
                .ToArray());
            var meshCollider = construction.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;
            meshCollider.sharedMesh = constructionMesh;
        }


        public static Center CreateCenter(float gravityAcceleration, Vector3 position = default)
        {
            var center = Object
                .Instantiate(Resources.Load<GameObject>("Center"), position, Quaternion.identity)
                .AddComponent<Center>();
            center.gravityAcceleration = gravityAcceleration;
            return center;
        }
    }
}