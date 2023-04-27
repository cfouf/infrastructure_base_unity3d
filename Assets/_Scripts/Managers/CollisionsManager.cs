using _Scripts.Utilities;
using _Scripts.Utilities.EventBus;
using _Scripts.Utilities.Interfaces;

namespace _Scripts.Managers
{
    public class CollisionsManager : MonoBehaviourService<CollisionsManager>, IEventReceiver<ConstructionsCollision>, IManager
    {
        protected override void OnCreateService() =>
            EventBus.Register(this);

        protected override void OnDestroyService() =>
            EventBus.UnRegister(this);
        public void OnEvent(ConstructionsCollision e)
        {
            for (var index = 0; index < e.Construction.cubesColliders.Count; index++)
            {
                var cubeCollider = e.Construction.cubesColliders[index];
                if (e.ConstructionOther.collider.bounds.Intersects(cubeCollider.bounds))
                    e.Construction.cubes[index].ChangeColor();
            }
        }
    }
}
