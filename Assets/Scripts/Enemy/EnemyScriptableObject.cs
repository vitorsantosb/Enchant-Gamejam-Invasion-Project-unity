using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "enemyObject", menuName = "Enemy/New enemy",order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        public string enemyName;
        public int enemyHealth;
        public float enemySpeed;
        public float enemyDamage;
        public GameObject enemyObject;
    }
}