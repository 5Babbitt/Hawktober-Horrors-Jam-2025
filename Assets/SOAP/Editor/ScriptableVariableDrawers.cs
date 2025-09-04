using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Scripts.SOAP.Editor
{
    // Primitive Types
    [CustomPropertyDrawer(typeof(Variables.FloatVariable))]
    public class FloatVariableDrawer : GenericVariableDrawer<Variables.FloatVariable, float, FloatField> { }
    
    [CustomPropertyDrawer(typeof(Variables.IntVariable))]
    public class IntVariableDrawer : GenericVariableDrawer<Variables.IntVariable, int, IntegerField> { }
    
    [CustomPropertyDrawer(typeof(Variables.BoolVariable))]
    public class BoolVariableDrawer : GenericVariableDrawer<Variables.BoolVariable, bool, Toggle> { }
    
    [CustomPropertyDrawer(typeof(Variables.StringVariable))]
    public class StringVariableDrawer : GenericVariableDrawer<Variables.StringVariable, string, TextField> { }
    
    // Unity Types
    [CustomPropertyDrawer(typeof(Variables.Vector2Variable))]
    public class Vector2VariableDrawer : GenericVariableDrawer<Variables.Vector2Variable, Vector2, Vector2Field> { }
    
    [CustomPropertyDrawer(typeof(Variables.Vector3Variable))]
    public class Vector3VariableDrawer : GenericVariableDrawer<Variables.Vector3Variable, Vector3, Vector3Field> { }
    
    [CustomPropertyDrawer(typeof(Variables.ColourVariable))]
    public class ColourVariableDrawer : GenericVariableDrawer<Variables.ColourVariable, Color, ColorField> { }
    
}