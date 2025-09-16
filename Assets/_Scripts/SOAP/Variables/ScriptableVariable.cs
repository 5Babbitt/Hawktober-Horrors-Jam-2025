using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.SOAP.Variables
{
    public abstract class ScriptableVariable<T> : RuntimeScriptableObject
    {
        [SerializeField] protected T initialValue;
        [SerializeField] protected T value;

        public event UnityAction<T> OnValueChanged = delegate { };

        public T Value
        {
            get => value;
            set
            {
                if (EqualityComparer(this.value, value)) return;
                this.value = value;
                OnValueChanged.Invoke(value);
            }
        }
        
        protected override void OnReset()
        {
            OnValueChanged.Invoke(value = initialValue);
        }
        
        protected abstract bool EqualityComparer(T a, T b);
    }
}