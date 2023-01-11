using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace BulletHell
{
    public static class EditorExtensions
    {
        public static PropertyField CreatePropertyField(SerializedProperty property)
        {
            PropertyField field = new PropertyField(property);
            field.BindProperty(property);

            return field;
        }

    }
}
