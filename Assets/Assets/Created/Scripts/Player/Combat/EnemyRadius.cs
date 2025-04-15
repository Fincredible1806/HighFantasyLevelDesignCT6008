using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRadius : MonoBehaviour
{
    public float playerRadius;
    [SerializeField] Vector3 RadiusOffset;
    [SerializeField] LayerMask enemyLayer;
    public List<EnemyNavigation> navigation = new();

    private void Update()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position + RadiusOffset, playerRadius, enemyLayer);
        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            Collider collider = enemiesInRange[i];
            bool contains;
            EnemyNavigation en;
            en = collider.GetComponent<EnemyNavigation>();
            contains = navigation.Contains(en);
            if(!contains)
            {
                navigation.Add(collider.GetComponent<EnemyNavigation>());
            }
            foreach(EnemyNavigation nav in navigation)
            {
                bool stillInRange;
                Collider enemyNav = nav.GetComponent<Collider>();
                stillInRange = enemiesInRange.Contains(enemyNav);
                if(!stillInRange)
                {
                    navigation.Remove(nav);
                }
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + RadiusOffset, playerRadius);
    }
}
