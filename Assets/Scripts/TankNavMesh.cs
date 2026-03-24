using UnityEngine;
using UnityEngine.AI;

public class TankNavMesh : MonoBehaviour
{
    public Transform helicopterPlatform;
    public Transform oilplatform;
    public NavMeshAgent tank;
    public void GoToHelicopter()
    {
        tank.SetDestination(helicopterPlatform.position);
    }

    public void GoToOil()
    {
        tank.SetDestination(oilplatform.position);
    }

}
