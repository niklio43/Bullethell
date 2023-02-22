using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace BulletHell
{
    public static class EditorExtensions
    {
        public static PropertyField CreatePropertyField(SerializedProperty property, string name = "")
        {
            PropertyField field = new PropertyField(property);
            if(name != "") { field.name = name; }

            field.BindProperty(property);

            return field;
        }

    }
}
