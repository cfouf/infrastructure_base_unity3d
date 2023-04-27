using _Scripts.Utilities;
using _Scripts.Utilities.EventBus;
using _Scripts.Utilities.Interfaces;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers
{
    public class GUIManager : MonoBehaviourService<GUIManager>, IManager
    {
        [SerializeField] private ClickableObject resetButton;
        [SerializeField] private TMP_Text timeView;
        [SerializeField] private TMP_Text collisionsCountView;

        protected override void OnCreateService()
        {
            resetButton.OnClick += _ => { EventBus<ResetButtonClick>.Raise(); };
        }

        protected override void OnDestroyService()
        {
        }

        public void RedrawTime(string newValue) =>
            timeView.text = $"Времени прошло {newValue}";
        
        public void RedrawCollisionsCount(int newValue) =>
            collisionsCountView.text = $"Столкновений {newValue}";
    }
}