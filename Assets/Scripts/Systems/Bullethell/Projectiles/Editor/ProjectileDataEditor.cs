using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletHell.Emitters.Projectiles.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(ProjectileData))]
    public class ProjectileDataEditor : Editor
    {
        VisualElement _root;
        ProjectileData _target;


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
            VisualTreeAsset original = Resources.Load<VisualTreeAsset>("ProjectileDataInspector");
            StyleSheet sheet = Resources.Load<StyleSheet>("ProjectileDataStyleSheet");
            _root.styleSheets.Add(sheet);
            TemplateContainer treeAsset = original.CloneTree();
            _root.Add(treeAsset);
        }

        void CreateDefualtInspector()
        {
            GroupBox dataRoot = _root.Query<GroupBox>("projectile-data").First();

            #region General Data Foldout
            Foldout generalFoldout = new Foldout();
            generalFoldout.value = _target.FoldOutGeneral;
            generalFoldout.name = "DefaultData_Foldout";
            generalFoldout.text = "General";
            generalFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutGeneral = generalFoldout.value);

            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("name")));
            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Scale")));
            dataRoot.Add(generalFoldout);
            #endregion

            #region Sprite Data Foldout
            Foldout spriteFoldout = new Foldout();
            spriteFoldout.value = _target.FoldOutSprite;
            spriteFoldout.name = "SpriteData_Foldout";
            spriteFoldout.text = "Sprite";
            spriteFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutSprite = spriteFoldout.value);

            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Sprite")));
            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Birth")));
            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("MidLife")));
            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Death")));
            dataRoot.Add(spriteFoldout);
            #endregion

            #region Behaviour Data Foldout
            Foldout behaviourFoldout = new Foldout();
            behaviourFoldout.value = _target.FoldOutBehaviour;
            behaviourFoldout.name = "BehaviourData_Foldout";
            behaviourFoldout.text = "Behaviour";
            behaviourFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutBehaviour = behaviourFoldout.value);

            behaviourFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Speed")));
            behaviourFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("MaxSpeed")));
            behaviourFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Acceleration")));

            #region Behaviour Drop Down menu
            Label contextMenu = new Label("Add New Behaviour");
            contextMenu.AddManipulator(new ContextualMenuManipulator(BuildBehaviourContextMenu));

            behaviourFoldout.Add(contextMenu);
            #endregion

            dataRoot.Add(behaviourFoldout);
            #endregion

            #region Behaviours List
            GroupBox behaviourBox = new GroupBox();
            behaviourBox.name = "Behaviour_Box";
            for (int i = 0; i < _target.Behaviours.Count; i++) {
                //VisualElement _behaviourRoot = Editor_target.Behaviours
            }

            #endregion


            #region Collision Data Foldout
            Foldout collisionFoldout = new Foldout();
            collisionFoldout.value = _target.FoldOutCollision;
            collisionFoldout.name = "CollisionData_Foldout";
            collisionFoldout.text = "Collision";
            collisionFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutCollision = collisionFoldout.value);

            collisionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("CollisionTags")));
            collisionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Collider")));
            dataRoot.Add(collisionFoldout);
            #endregion
        }

        void BuildBehaviourContextMenu(ContextualMenuPopulateEvent evt)
        {
            BaseProjectileBehaviour[] behaviours = Resources.LoadAll<BaseProjectileBehaviour>("ProjectileBehaviours/");

            for (int i = 0; i < behaviours.Length; i++) {
                BaseProjectileBehaviour behaviour = behaviours[i];

                evt.menu.AppendAction(behaviour.name, (action) => {
                    BaseProjectileBehaviour newBehaviour = Instantiate(behaviour);
                    newBehaviour.name = behaviour.name;
                    newBehaviour.SetOwner(_target);
                    AddAssetToDatabase(newBehaviour);
                    _target.Behaviours.Add(newBehaviour);
                });
            }
        
        }
        
        void AddAssetToDatabase(Object objectToAdd)
        {
#if UNITY_EDITOR
            Debug.Log(objectToAdd);
            AssetDatabase.AddObjectToAsset(objectToAdd, _target);
            UpdateAssetDatabase();
#endif
        }

        void UpdateAssetDatabase()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }


        private void OnEnable()
        {
            _target = target as ProjectileData;
        }
    }
}
