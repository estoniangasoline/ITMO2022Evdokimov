using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ubavar.core.Utils
{
    public class GameObjectUtils
    {
        public static Transform FindDescendentTransformByPathOnRootTransform(string path)
        {
            GameObject[] rootGameObjects =
                SceneManager.GetActiveScene().GetRootGameObjects();

            Transform result = null;

            foreach (GameObject rootGameObject in rootGameObjects)
            {
                if (rootGameObject.name == path)
                {
                    result = rootGameObject.transform;
                }
                else
                {
                    result = FindDescendentTransformByPath(rootGameObject.transform, path);
                }

                if (result)
                {
                    break;
                }
            }

            return result;
        }

        public static Transform FindDescendentTransformByPath(Transform searchTransform, string path)
        {
            string[] parts = path.Split(new[] { '.' }, (int)2);

            if (parts.Length == 2)
            {
                string parentName = parts[0];
                string searchName = parts[1];

                Transform parentTransform = FindDescendentTransform(searchTransform, parentName);

                if (parentTransform)
                {
                    return FindDescendentTransformByPath(parentTransform, searchName);
                }

                return null;
            }

            return FindDescendentTransform(searchTransform, path);
        }

        public static Transform FindDescendentTransform(Transform searchTransform, string descendantName)
        {
            Transform result = null;

            if (searchTransform.name == descendantName)
            {
                result = searchTransform;
            }
            else
            {
                int childCount = searchTransform.childCount;

                for (int i = 0; i < childCount; i++)
                {
                    Transform childTransform = searchTransform.GetChild(i);

                    if (childTransform.name != descendantName)
                    {
                        // Not it, but has children? Search the children.
                        if (childTransform.childCount > 0)
                        {
                            Transform grandchildTransform = FindDescendentTransform(childTransform, descendantName);
                            if (grandchildTransform == null)
                                continue;

                            result = grandchildTransform;
                            break;
                        }

                        // Not it, but has no children?  Go on to the next sibling.
                        {
                            continue;
                        }
                    }

                    // Found it.
                    result = childTransform;
                    break;
                }
            }

            return result;
        }

        public static List<GameObject> FindGameObjectsWithTag(GameObject parent, string tag)
        {
            List<GameObject> taggedGameObjects = new List<GameObject>();
            Transform parentTransform = parent.transform;

            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                if (child.CompareTag(tag))
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindGameObjectsWithTag(child.gameObject, tag));
                }
            }
            return taggedGameObjects;
        }

        public static void MoveToLayer(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
                MoveToLayer(child.gameObject, layer);
        }

        public static void ClearImmediate(GameObject gameObject)
        {
            Transform transform = gameObject.transform;
            int childs = transform.childCount;

            for (int i = childs - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}