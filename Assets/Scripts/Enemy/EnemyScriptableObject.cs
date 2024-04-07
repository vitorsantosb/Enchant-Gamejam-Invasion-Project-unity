﻿using UnityEngine;

namespace EnemyObject
{
    [CreateAssetMenu(fileName = "enemyObject", menuName = "Enemy/New enemy",order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        public int enemyHealth;
        public float enemySpeed;
        public float enemyDamage;
        public EnemyType enemyType;
    }

    public enum EnemyType
    {
        NORMAL,
        WEAK,
        STRONG,
        RANGED,
        SPLITTER,
        HEALER
    }
}