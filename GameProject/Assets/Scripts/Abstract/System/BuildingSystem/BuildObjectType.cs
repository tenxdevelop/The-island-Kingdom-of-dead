using System;
using UnityEngine;

namespace TheIslandKOD
{

    [Serializable]
    public class BuildObjectType
    {
        public GameObject buildObject;
        public GameObject prefab;
        public string tagObject;
        public Vector3 objectOffset;
        public Vector3 objectRotation;
    }
}
