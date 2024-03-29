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
        #region Private Fields
        VisualElement _root;
        ProjectileData _target;

        Label _behavioursContext;
        GroupBox _behavioursList;
        #endregion

        #region Public Methods
        public override VisualElement CreateInspectorGUI()
        {
            if (_target != null) {
                _root = new VisualElement();
                Redraw();
            }

            return _root;
        }
        #endregion

        #region Private Methods
        void Redraw()
        {
            if(_root != null)
                _root.Clear();

            CreateRoot();
            CreateDefualtInspector();
        }


        void CreateRoot()
        {
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
            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Damage")));
            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("StatusEffects")));
            dataRoot.Add(generalFoldout);
            #endregion

            #region Sprite Data Foldout
            Foldout spriteFoldout = new Foldout();
            spriteFoldout.value = _target.FoldOutSprite;
            spriteFoldout.name = "SpriteData_Foldout";
            spriteFoldout.text = "Sprite";
            spriteFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutSprite = spriteFoldout.value);

            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Sprite")));
            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("BirthColor")));
            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("MidLifeColor")));
            spriteFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("DeathColor")));
            dataRoot.Add(spriteFoldout);
            #endregion

            #region Animation/VFX Data Foldout
            Foldout animationFoldout = new Foldout();
            animationFoldout.value = _target.FoldOutAnimation;
            animationFoldout.name = "AnimationData_Foldout";
            animationFoldout.text = "Animation/VFX";
            animationFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutAnimation = animationFoldout.value);

            animationFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("BirthVFX")));
            animationFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("DeathVFX")));
            dataRoot.Add(animationFoldout);
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
            behaviourFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("InheritVelocity")));

            _behavioursContext = new Label();
            _behavioursContext.name = "BehaviourData_ContextMenu";
            RedrawBehaviourContext();

            _behavioursList = new GroupBox();
            RedrawBehavioursList();

            behaviourFoldout.Add(_behavioursContext);
            behaviourFoldout.Add(_behavioursList);
            dataRoot.Add(behaviourFoldout);
            #endregion

            #region Collision Data Foldout
            Foldout collisionFoldout = new Foldout();
            collisionFoldout.value = _target.FoldOutCollision;
            collisionFoldout.name = "CollisionData_Foldout";
            collisionFoldout.text = "Collision";
            collisionFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutCollision = collisionFoldout.value);

            collisionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("HasCollision")));
            collisionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("DestroyOnCollision")));
            collisionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("CollisionTags")));
            collisionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Collider")));
            dataRoot.Add(collisionFoldout);
            #endregion
        }

        void RedrawBehaviours()
        {
            RedrawBehaviourContext();
            RedrawBehavioursList();
        }

        void RedrawBehaviourContext()
        {
            _behavioursContext.Clear();
            _behavioursContext.text = "Add New Behaviour (Right Click)";
            _behavioursContext.AddManipulator(new ContextualMenuManipulator(BuildBehaviourContextMenu));
        }

        void RedrawBehavioursList()
        {
            _behavioursList.Clear();
            _behavioursList.name = "Behaviour_Box";

            foreach (BaseProjectileBehaviour behaviour in _target.Behaviours) {
                Editor editor = CreateEditor(behaviour);
                VisualElement behaviourRoot = editor.CreateInspectorGUI();

                Button deleteBtn = behaviourRoot.Query<Button>("delete-button").First();

                deleteBtn.clicked += () => {
                    _target.RemoveBehaviour(behaviour);
                    RedrawBehaviours();
                };

                _behavioursList.Add(behaviourRoot);
            }
        }


        void BuildBehaviourContextMenu(ContextualMenuPopulateEvent evt)
        {
            BaseProjectileBehaviour[] behaviours = Resources.LoadAll<BaseProjectileBehaviour>("ProjectileBehaviours/");

            for (int i = 0; i < behaviours.Length; i++) {
                BaseProjectileBehaviour behaviour = behaviours[i];

                if (_target.AlreadyHasBehaviour(behaviour)) continue;

                evt.menu.AppendAction(behaviour.name, (action) => {
                    BaseProjectileBehaviour newBehaviour = Instantiate(behaviour);
                    newBehaviour.name = behaviour.name;
                    _target.AddBehaviour(newBehaviour);
                    RedrawBehaviours();
                });
            }
        
        }
        private void OnEnable()
        {
            _target = target as ProjectileData;
        }
        #endregion
    }
}
