using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleOfLife
{
    public struct SkillContext 
    {
        public Vector2 TriggerPos;
        public Vector2 TargetPos;
        public Vector2 Direction;
        public GameObject BodyPrefab;
        public float MoveSpeed;

        public List<GameObject> OtherPrefabs;
       
    }
}
