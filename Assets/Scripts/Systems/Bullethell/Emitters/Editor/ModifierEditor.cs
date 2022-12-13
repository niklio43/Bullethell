using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletHell.Emitters.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(EmitterModifier))]
    public class ModifierEditor : Editor
    {
        VisualElement _root;
        EmitterModifier _target;

        public override VisualElement CreateInspectorGUI()
        {
            if(_target != null)
                Redraw();

            return _root;
        }

        private void Redraw()
        {
            CreateRoot();
            CreateHeader();
            CreateFields();
        }

        private void CreateRoot()
        {
            _root = new VisualElement();
            VisualTreeAsset original = Resources.Load<VisualTreeAsset>("ModifierInspector");
            TemplateContainer treeAsset = original.CloneTree();
            StyleSheet sheet = Resources.Load<StyleSheet>("EmitterDataStyleSheet");
            _root.styleSheets.Add(sheet);
            _root.Add(treeAsset);
        }

        private void CreateHeader()
        {
            GroupBox header = _root.Query<GroupBox>("modifier-header");
            Label name = header.Query<Label>("name");
            name.text = _target.name;

            Toggle toggle = header.Query<Toggle>("toggle-modifier").First();

            toggle.value = _target.Enabled;
            toggle.RegisterCallback<ClickEvent>((changeEvent) => _target.Enabled = toggle.value);


            Button editBtn = header.Query<Button>("edit").First();
            VisualElement nameField = header.Query<VisualElement>("namefield").First();
            TextField textField = nameField.Query<TextField>("newname_input").First();
            Button confirmButton = nameField.Query<Button>("confirm_name").First();

            textField.value = _target.name;

            editBtn.clicked += () => { 
                nameField.visible = !nameField.visible; 
                textField.visible = nameField.visible;
                name.visible = !nameField.visible;
            };

            confirmButton.clicked += () => {
                _target.Rename(textField.text);
                name.text = _target.name;
                nameField.visible = false;
                textField.visible = false;
                name.visible = true;
            };
        }

        private void CreateFields()
        {
            Label name = _root.Query<Label>("name").First();
            GroupBox dataRoot = _root.Query<GroupBox>("modifier-data").First();

            name.RegisterCallback<ClickEvent>((changeEvent) => {
                _target.FoldOut = !_target.FoldOut;

                dataRoot.style.display = (_target.FoldOut) ? DisplayStyle.Flex : DisplayStyle.None;
            });



            dataRoot.style.display = DisplayStyle.None;

            #region General Data Foldout
            Foldout generalFoldout = new Foldout();
            generalFoldout.value = _target.FoldOutGeneral;
            generalFoldout.name = "modifier-data-foldout";
            generalFoldout.text = "General";
            generalFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutGeneral = generalFoldout.value);

            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Factor")));
            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Count")));
            dataRoot.Add(generalFoldout);
            #endregion

            #region Projectile Modifiers Foldout
            Foldout projectileFoldout = new Foldout();
            projectileFoldout.value = _target.FoldOutProjectile;
            projectileFoldout.name = "modifier-data-foldout";
            projectileFoldout.text = "Projectile Modifiers";
            projectileFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutProjectile = projectileFoldout.value);

            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("ProjectileData")));
            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("TimeToLive")));
            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("SpeedMultiplier")));
            dataRoot.Add(projectileFoldout);
            #endregion

            #region Emission Modifiers Foldout
            Foldout emissionFoldout = new Foldout();
            emissionFoldout.value = _target.FoldOutEmitter;
            emissionFoldout.name = "modifier-data-foldout";
            emissionFoldout.text = "Emission Modifiers";
            emissionFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutEmitter = emissionFoldout.value);

            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Pitch")));
            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Offset")));
            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("NarrowSpread")));
            dataRoot.Add(emissionFoldout);
            #endregion
        }

        private void OnEnable()
        {
            _target = target as EmitterModifier;
        }
    }
}
