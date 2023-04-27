using System.Collections.Generic;
using _Scripts.Entities;
using _Scripts.Utilities;
using _Scripts.Utilities.Interfaces;

namespace _Scripts.Managers
{
    public class MovementManager : MonoBehaviourService<MovementManager>, IManager, ICallable
    {
        public List<Construction> constructions;

        protected override void OnCreateService(){}

        protected override void OnDestroyService(){}

        public void Call()
        {
            foreach (var construction in constructions)
            {
                construction.Move();
            }
        }
        
    }
}