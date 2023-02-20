using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletHell.Emitters.Projectiles.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(BaseProjectileBehaviour), true)]
    public class BehaviourDataEditor : Editor
    {
        VisualElement _root;
        BaseProjectileBehaviour _target;

        public override VisualElement CreateInspectorGUI()
        {
            if (_target != null)
                Redraw();

            return _root;
        }

        void Redraw()
        {
            CreateRoot();
            CreateDefualtInspector();
        }

        void CreateRoot()
        {
            _root = new VisualElement();
            VisualTreeAsset original = Resources.Load<VisualTreeAsset>("ProjectileBehaviourInspector");
            StyleSheet sheet = Resources.Load<StyleSheet>("EmitterDataStyleSheet");
            _root.styleSheets.Add(sheet);
            TemplateContainer treeAsset = original.CloneTree();
            _root.Add(treeAsset);
        }

        void CreateDefualtInspector()
        {
            Label behaviourName = _root.Query<Label>("behaviour-name").First();
            behaviourName.text = _target.name;

            GroupBox dataRoot = _root.Query<GroupBox>("behaviour-data").First();
            SerializedProperty prop = serializedObject.GetIterator();

            if (prop.NextVisible(true)) {
                do {
                    if(prop.name == "m_Script") { continue; }
                    dataRoot.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty(prop.name)));
                }
                while (prop.NextVisible(false));
            }


        }

        private void OnEnable()
        {
            _target = target as BaseProjectileBehaviour;
        }
    }
}