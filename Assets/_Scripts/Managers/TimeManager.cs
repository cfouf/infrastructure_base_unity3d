using System.Globalization;
using _Scripts.Utilities;
using _Scripts.Utilities.EventBus;
using _Scripts.Utilities.Interfaces;
using UnityEngine;

namespace _Scripts.Managers
{
    public class TimeManager : MonoBehaviourService<TimeManager>, IEventReceiver<ResetButtonClick>, ICallable, IManager 
    {
        private float totalTime;
        private readonly GUIManager guiManager = GUIManager.Get();
        
        protected override void OnCreateService() =>
            EventBus.Register(this);

        protected override void OnDestroyService() =>
            EventBus.UnRegister(this);

        public void Call()
        {
            totalTime += Time.deltaTime;
            RedrawTime();
        }

        private void RedrawTime() =>
            guiManager.RedrawTime(Mathf.Round(totalTime).ToString(CultureInfo.InvariantCulture));

        private void ResetTime()
        {
            totalTime = 0.0f;
            RedrawTime();
        }

        public void OnEvent(ResetButtonClick e) =>
            ResetTime();
    }
}