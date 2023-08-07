using _Scripts.Utilities.Interfaces;
using UnityEngine;

namespace _Scripts.Utilities
{
    public class FiledGenerator : MonoBehaviourService<FiledGenerator>, ICodeGenerator
    {
        public bool GenerateCode()
        {
           return global::FiledGenerator.FiledGenerator.GenerateFields();
        }

        protected override void OnCreateService()
        {
        }

        protected override void OnDestroyService()
        {
        }
    }
}