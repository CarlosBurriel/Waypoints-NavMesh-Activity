using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creamos una estructura para facilitar la entrada de datos en nuestro grafo
[System.Serializable]
public struct Link //Edge entre nodos
{
    public enum direction { UNI, BI }; //Edge de una direccion o bidireccional
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

public class GraphManager : MonoBehaviour
{
    public GameObject[] waypoints;    //Array de waypoints
    public Link[] links;              //Array de links
    public Graph graph = new Graph(); //Codigo para el grafo esta ya creado en la carpeta "Graphs"

    // Start is called before the first frame update
    void Start()
    {
        //Si wayPoints tiene nodos definidos (prevenir estructura vacia)
        if(waypoints.Length > 0) 
        {
            foreach (GameObject waypoint in waypoints)
            {
                //Iteramos para cada WP y lo guardamos en la estructura graph
                graph.AddNode(waypoint);
            }
            //Iteramos para cada link y lo guardamos en la estructura graph
            foreach (Link link in links)
            {
                if (link.node1 == null || link.node2 == null)
                {
                    Debug.LogWarning(link + ": Alguno de los nodos es nulo");
                    continue;
                }
                if (link.dir == Link.direction.UNI)
                {
                    graph.AddEdge(link.node1, link.node2);
                }
                else
                {
                    graph.AddEdge(link.node2, link.node1);
                    graph.AddEdge(link.node1, link.node2);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        graph.debugDraw();
    }
}
