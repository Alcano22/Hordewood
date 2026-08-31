using UnityEngine;
using UnityEditor;
using Hordewood.Weapons;

namespace Hordewood.EditorTools
{
    [CustomEditor(typeof(Gun))]
    public class GunEditor : Editor
    {
        private SerializedProperty _displayName;
        private SerializedProperty _icon;
        private SerializedProperty _muzzleOffset;
        private SerializedProperty _visualScale;

        private SerializedProperty _damage;
        private SerializedProperty _fireRate;
        private SerializedProperty _capacity;
        private SerializedProperty _reloadTime;
        private SerializedProperty _bulletSpeed;
        private SerializedProperty _range;

        private SerializedProperty _fireMode;
        private SerializedProperty _burstCount;
        private SerializedProperty _burstShotDelay;
        private SerializedProperty _pelletCount;
        private SerializedProperty _spreadAngle;

        private SerializedProperty _fireSound;

        private void OnEnable()
        {
            _displayName  = serializedObject.FindProperty("displayName");
            _icon         = serializedObject.FindProperty("icon");
            _muzzleOffset = serializedObject.FindProperty("muzzleOffset");
            _visualScale  = serializedObject.FindProperty("visualScale");

            _damage = serializedObject.FindProperty("damage");
            _fireRate    = serializedObject.FindProperty("fireRate");
            _capacity    = serializedObject.FindProperty("capacity");
            _reloadTime  = serializedObject.FindProperty("reloadTime");
            _bulletSpeed = serializedObject.FindProperty("bulletSpeed");
            _range       = serializedObject.FindProperty("range");

            _fireMode       = serializedObject.FindProperty("fireMode");
            _burstCount     = serializedObject.FindProperty("burstCount");
            _burstShotDelay = serializedObject.FindProperty("burstShotDelay");
            _pelletCount    = serializedObject.FindProperty("pelletCount");
            _spreadAngle    = serializedObject.FindProperty("spreadAngle");

            _fireSound = serializedObject.FindProperty("fireSound");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_displayName);
            EditorGUILayout.PropertyField(_icon);
            EditorGUILayout.PropertyField(_muzzleOffset);
            EditorGUILayout.PropertyField(_visualScale);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_damage);
            EditorGUILayout.PropertyField(_fireRate);
            EditorGUILayout.PropertyField(_capacity);
            EditorGUILayout.PropertyField(_reloadTime);
            EditorGUILayout.PropertyField(_bulletSpeed);
            EditorGUILayout.PropertyField(_range);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Behaviour", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_fireMode);

            var mode = (FireMode)_fireMode.enumValueIndex;
            if (mode == FireMode.Burst)
            {
                EditorGUILayout.PropertyField(_burstCount);
                EditorGUILayout.PropertyField(_burstShotDelay);
            } else if (mode == FireMode.Spread)
            {
                EditorGUILayout.PropertyField(_pelletCount);
                EditorGUILayout.PropertyField(_spreadAngle);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_fireSound);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
