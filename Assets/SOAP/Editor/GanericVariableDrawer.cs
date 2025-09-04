using System;
using _Scripts.SOAP.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace _Scripts.SOAP.Editor
{
    public abstract class GenericVariableDrawer<TVariable, TValue, TField> : PropertyDrawer 
        where TVariable : ScriptableVariable<TValue> 
        where TField : BaseField<TValue>, new()
    {
        protected virtual string DefaultFieldLabel(string text = " ") => text;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var objectField = new ObjectField(property.displayName)
            {
                objectType = typeof(TVariable)
            };
            objectField.BindProperty(property);
            container.Add(objectField);

            var valueField = CreateValueField(DefaultFieldLabel());
            valueField.SetEnabled(false);
            valueField.style.paddingRight = 20;

            objectField.RegisterValueChangedCallback(
                evt =>
                {
                    var variable = evt.newValue as TVariable;
                    if (variable != null)
                    {
                        container.Add(valueField);
                        UpdateValueField(valueField, GetValue(variable));
                        RegisterCallback(variable, newValue => UpdateValueField(valueField, newValue));
                    }
                    else
                    {
                        if (container.Contains(valueField))
                            container.Remove(valueField);
                        UpdateValueField(valueField);
                    }
                }
            );

            var currentVariable = property.objectReferenceValue as TVariable;
            if (currentVariable != null)
            {
                UpdateValueField(valueField, GetValue(currentVariable));
                RegisterCallback(currentVariable, newValue => UpdateValueField(valueField, newValue));
                container.Add(valueField);
            }

            return container;
        }

        protected virtual TValue GetValue(TVariable variable)
        {
            return variable.Value;
        }

        protected virtual void RegisterCallback(TVariable variable, Action<TValue> callback)
        {
            variable.OnValueChanged += value => callback(value);
        }

        

        protected virtual VisualElement CreateValueField(string label)
        {
            return new TField { 
                label = label,
                value = default
            };
        }
        
        protected virtual void UpdateValueField(VisualElement field, TValue value = default)
        {
            if (field is TField typedField)
            {
                typedField.value = value;
            }
        }
    }
}
