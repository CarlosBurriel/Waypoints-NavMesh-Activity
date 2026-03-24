using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal; //Goal from A* algorithm
    float speed = 5.0f;          //Velocidad del tanque
    float accuracy = 1.0f;		 //Distancia por debajo de la cual considero que he alcanzado el nodo
    float rotSpeed = 2.0f;       //Velocidad de rotacion del tanque
    public GameObject wpManager; //All the graph points and links
    public GameObject[] wps;            //Nodos del grafo
    public GameObject currentNode;		 //El nodo mas cercano en cada momento
    int currentWP = 0;           //Indice a lo largo del camino que seguimos, no tiene que ver con el numero/posicion de cada WP
    public Graph g;                     //Grafo
    public GameObject helicopterplatform;
    public GameObject oil;

    // Start is called before the first frame update
    void Start()
    {
        //Inicializar wps, g y currentNode
        GraphManager graphManager =  wpManager.GetComponent<GraphManager>();
        wps = graphManager.waypoints;
        g = graphManager.graph;
        currentNode = graphManager.waypoints[0];
    }

    public void GoToHeli()
    {
        GameObject newgoal = helicopterplatform;
        float smallDist = 1000;
        foreach (GameObject waypoints in wps)
        {
            if (waypoints == null) continue;
            float distance = Vector3.Distance(transform.position, waypoints.transform.position);
            if (distance < smallDist)
            {
                smallDist = distance;
                currentNode = waypoints;
            }
        }
        g.AStar(currentNode, newgoal);
        currentWP = 0;
    }

    public void GoToRuin()
    {
        GameObject newgoal = oil;
        float smallDist = 1000;
        foreach (GameObject waypoints in wps)
        {
            if (waypoints == null) continue;
            float distance = Vector3.Distance(transform.position, waypoints.transform.position);
            if (distance < smallDist)
            {
                smallDist = distance;
                currentNode = waypoints;
            }
        }
        g.AStar(currentNode, newgoal);
        currentWP = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Testear si no hay un camino definido (empty path) o currentWP esta ya al final del camino
        if(g == null) { return; }

        if(g.getPathLength() <= 0) { print("plonga"); return; }

        //Guardamos en currentNode el nodo que tenemos mas cercano en este momento
        float smallDist = 1000;
        foreach (GameObject waypoints in wps)
        {
            if (waypoints == null) { continue; }
            float distance = Vector3.Distance(transform.position, waypoints.transform.position);
            if (distance < smallDist)
            {
                smallDist = distance;
                currentNode = waypoints;
            }
        }
        
        //Si aun no hemos llegado al final del camino, nos dirigimos al siguiente nodo
        GameObject pathPoint = g.getPathPoint(currentWP);
        if (pathPoint == null) { return; }
            

        goal = pathPoint.transform;

        //Si estamos suficientemente cerca al nodo currentWP, pasamos a buscar el siguiente
        if (Vector3.Distance(transform.position, goal.position) < accuracy)
        {
            currentWP++;
            // si hemos avanzado fuera del array terminamos aquí para evitar acceso inválido
            if (currentWP >= g.getPathLength())
            {
                g.debugDraw();
                return;
            }
            //Guardamos en goal el nuevo nodo a visitar
            goal = g.getPathPoint(currentWP).transform;
        }

        //Miramos hacia nuestro destino (goal)
        Quaternion lookRotation = Quaternion.LookRotation((goal.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
        //Movemos el tanque
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        g.debugDraw();

    }
}
