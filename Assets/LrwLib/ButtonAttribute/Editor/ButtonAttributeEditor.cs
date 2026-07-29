using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LrwLib.ButtonAttribute.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public sealed class ButtonAttributeMonoBehaviourEditor : ButtonAttributeEditorBase
    {
    }

    [CustomEditor(typeof(ScriptableObject), true)]
    [CanEditMultipleObjects]
    public sealed class ButtonAttributeScriptableObjectEditor : ButtonAttributeEditorBase
    {
    }

    public abstract class ButtonAttributeEditorBase : UnityEditor.Editor
    {
        private const BindingFlags MethodFlags =
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private List<ButtonMethod> buttonMethods;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            buttonMethods ??= FindButtonMethods(target.GetType());
            if (buttonMethods.Count == 0)
            {
                return;
            }

            EditorGUILayout.Space();

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);

                foreach (var buttonMethod in buttonMethods)
                {
                    DrawButton(buttonMethod);
                }
            }
        }

        private static List<ButtonMethod> FindButtonMethods(Type targetType)
        {
            var methods = new List<ButtonMethod>();

            for (var type = targetType; IsDrawableType(type); type = type.BaseType)
            {
                foreach (var method in type.GetMethods(MethodFlags))
                {
                    var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>(true);
                    if (buttonAttribute == null)
                    {
                        continue;
                    }

                    var parameters = method.GetParameters();
                    var isInvokable = parameters.Length == 0 && !method.ContainsGenericParameters;
                    var label = string.IsNullOrWhiteSpace(buttonAttribute.Label)
                        ? ObjectNames.NicifyVariableName(method.Name)
                        : buttonAttribute.Label;

                    methods.Add(new ButtonMethod(method, label, isInvokable));
                }
            }

            return methods;
        }

        private static bool IsDrawableType(Type type)
        {
            return type != null
                   && type != typeof(MonoBehaviour)
                   && type != typeof(ScriptableObject)
                   && type != typeof(UnityEngine.Object);
        }

        private void DrawButton(ButtonMethod buttonMethod)
        {
            using (new EditorGUI.DisabledScope(!buttonMethod.IsInvokable))
            {
                if (GUILayout.Button(buttonMethod.Label))
                {
                    InvokeButtonMethod(buttonMethod.Method);
                }
            }

            if (!buttonMethod.IsInvokable)
            {
                EditorGUILayout.HelpBox(
                    $"{buttonMethod.Method.Name} cannot be invoked because Button methods must have no parameters and cannot be generic.",
                    MessageType.Warning);
            }
        }

        private void InvokeButtonMethod(MethodInfo method)
        {
            if (method.IsStatic)
            {
                InvokeSingle(method, null);
                return;
            }

            foreach (var selectedTarget in targets)
            {
                if (selectedTarget == null || !method.DeclaringType.IsInstanceOfType(selectedTarget))
                {
                    continue;
                }

                Undo.RecordObject(selectedTarget, $"Invoke {method.Name}");
                InvokeSingle(method, selectedTarget);
                EditorUtility.SetDirty(selectedTarget);
            }
        }

        private static void InvokeSingle(MethodInfo method, UnityEngine.Object instance)
        {
            try
            {
                var result = method.Invoke(instance, null);
                if (method.ReturnType != typeof(void))
                {
                    Debug.Log($"{method.Name} returned {result}", instance);
                }
            }
            catch (TargetInvocationException exception)
            {
                Debug.LogException(exception.InnerException ?? exception, instance);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, instance);
            }
        }

        private readonly struct ButtonMethod
        {
            public readonly MethodInfo Method;
            public readonly string Label;
            public readonly bool IsInvokable;

            public ButtonMethod(MethodInfo method, string label, bool isInvokable)
            {
                Method = method;
                Label = label;
                IsInvokable = isInvokable;
            }
        }
    }
}