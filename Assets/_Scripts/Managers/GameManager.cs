using System;
using System.Collections.Generic;
using System.Reflection;
using _Scripts.Entities;
using _Scripts.Utilities;
using _Scripts.Utilities.Interfaces;
using Assets._Scripts.Attributes;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GameManager : MonoBehaviourService<GameManager>, IManager
    {
        [SerializeField] private int amountOfConstructions = 2;
        [SerializeField] private int sizeOfConstruction = 50;
        [SerializeField] private int maxSpeed = 40;

        [Header("After start can be changed in realtime in Center game object")] [SerializeField]
        private float gravityAcceleration = 10f;

        private List<Construction> constructions;

        private List<ICallable> callables = new();
        private List<IManager> managers = new();

        private Center center;

        protected override void OnCreateService()
        {
#if UNITY_EDITOR
            var codeGenerators = MonoBehaviourServiceHelper.GetAllInstances<ICodeGenerator>();
            var wasCodeGenerated = false;
            foreach (var codeGenerator in codeGenerators)
                wasCodeGenerated = codeGenerator.GenerateCode();

            if (wasCodeGenerated)
                MediaTypeNames.Application.Quit();

#endif

            managers = MonoBehaviourServiceHelper.GetAllInstances<IManager>();

            FillManagerVariables();

            constructions = Spawner.CreateConstructions(amountOfConstructions, sizeOfConstruction);
            center = Spawner.CreateCenter(gravityAcceleration);

            foreach (var construction in constructions)
            {
                foreach (var cube in construction.cubes)
                {
                    construction.cubesColliders.Add(cube.GetComponent<Collider>());
                }
                
                construction.rb = construction.GetComponent<Rigidbody>();
                construction.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                construction.spinRadius = construction.cubes.Count * 0.5f;
                construction.maxSpeed = maxSpeed;
            }
            
            ScoreManager.Get().amountOfConstructions = amountOfConstructions;

            constructions.ForEach(construction => construction.center = center);

            MovementManager.Get().constructions = constructions;
            
            callables = MonoBehaviourServiceHelper.GetAllInstances<ICallable>();
        }

        protected override void OnDestroyService()
        {
        }

        private void Update()
        {
            callables.ForEach(callable => callable.Call());
        }

        private void FillManagerVariables()
        {
            foreach (var manager in managers)
            {
                var fields = manager.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    var attribute =
                        (AssignFromGameManagerAttribute) Attribute.GetCustomAttribute(field,
                            typeof(AssignFromGameManagerAttribute));
                    if (attribute != null)
                    {
                        var gameManagerField = typeof(GameManager).GetField(field.Name,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (gameManagerField != null)
                        {
                            field.SetValue(manager, gameManagerField.GetValue(this));
                        }
                    }
                }
            }
        }
        
    }
}