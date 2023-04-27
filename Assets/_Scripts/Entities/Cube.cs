using UnityEngine;

namespace _Scripts.Entities
{
    [RequireComponent(typeof(BoxCollider))]
    public class Cube : MonoBehaviour
    {
        private new Renderer renderer;
    
        private void Start()
        {
            renderer = GetComponent<Renderer>();
            renderer.material.color = Color.white;
        }

        public void ChangeColor() =>
            renderer.material.color = Color.red;
    }
}