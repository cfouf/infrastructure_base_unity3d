using System.Collections.Generic;
using _Scripts.Utilities;
using _Scripts.Utilities.EventBus;
using _Scripts.Utilities.Interfaces;
using UnityEngine;

namespace _Scripts.Managers
{
    public class ScoreManager : MonoBehaviourService<ScoreManager>, IEventReceiver<ResetButtonClick>,
        IEventReceiver<ConstructionsCollision>, IManager

    {
        private readonly GUIManager guiManager = GUIManager.Get();
        private int collisionsCount;
        private readonly HashSet<SetPair<GameObject>> collidedPairs = new HashSet<SetPair<GameObject>>();

        public int amountOfConstructions;

        protected override void OnCreateService() =>
            EventBus.Register(this);

        protected override void OnDestroyService() =>
            EventBus.UnRegister(this);

        public void OnEvent(ResetButtonClick e)
        {
            collisionsCount = 0;
            guiManager.RedrawCollisionsCount(collisionsCount);
        }

        public void OnEvent(ConstructionsCollision e)
        {
            var pair = new SetPair<GameObject>(e.Construction.gameObject, e.ConstructionOther.gameObject);
            if (collidedPairs.Contains(pair))
            {
                collisionsCount++;
                guiManager.RedrawCollisionsCount(collisionsCount);
                collidedPairs.Remove(pair);
            }
            else
            {
                collidedPairs.Add(pair);
            }
        }
    }
}