using UnityEngine;
using UnityEngine.UI;

namespace EnemyObject
{
    [CreateAssetMenu(fileName = "enemyObject", menuName = "Enemy/New enemy",order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        public float enemySpeed;
        public float enemyDamage;
        public EnemyType enemyType;
        public int enemyMaxHealth;
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