using UnityEngine;

namespace _Scripts.Utilities
{
    public readonly struct SetPair<T> where T : Object
    {
        public readonly T Obj1;
        public readonly T Obj2;

        public SetPair(T obj1, T obj2)
        {
            Obj1 = obj1;
            Obj2 = obj2;
        }
        
        public bool Contains(T obj) =>
            Obj1 == obj || Obj2 == obj;

        public override bool Equals(object obj)
        {
            if (!(obj is SetPair<T> other)) return false;
            return (other.Obj1 == Obj1 && other.Obj2 == Obj2) ||
                   (other.Obj1 == Obj2 && other.Obj2 == Obj1);
        }

        public override int GetHashCode()
        {
            return Obj1.GetHashCode() ^ Obj2.GetHashCode();
        }
    }
}