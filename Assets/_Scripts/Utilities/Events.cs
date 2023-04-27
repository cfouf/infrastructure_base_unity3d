using System;
using System.Collections.Generic;
using _Scripts.Entities;
using _Scripts.Utilities.EventBus;
using UnityEngine;

namespace _Scripts.Utilities
{
    public struct ResetButtonClick : IEvent {}

    public struct CubesCollision : IEvent
    {
        public readonly List<Tuple<Cube, Cube>> CollisionPairs;

        public CubesCollision(List<Tuple<Cube, Cube>> collisionPairs)
        {
            CollisionPairs = collisionPairs;
        }
    }
    
    public struct ConstructionsCollision : IEvent
    {
        public readonly Construction Construction;
        public readonly Collision ConstructionOther;

        public ConstructionsCollision(Construction construction, Collision constructionOther)
        {
            Construction = construction;
            ConstructionOther = constructionOther;
        }
    }

}