using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleController : MonoBehaviour
{
    [SerializeField] private List<People> m_Peoples;
    [SerializeField] private List<NodeOfInterest> m_Nodes;

    public void RegisterPeople(People people)
    {
        if (!m_Peoples.Contains(people))
            m_Peoples.Add(people);
        people.OnLookingForClosestPlace += TellClosestNode;
        people.OnWantsToChangePlace += TellWhereToGo;
    }

    public void UnregisterPeople(People people)
    {
        people.OnLookingForClosestPlace -= TellClosestNode;
        people.OnWantsToChangePlace -= TellWhereToGo;
        m_Peoples.Remove(people);

    }

    private void TellClosestNode(People people)
    {
        float minDistance = 9999f;
        NodeOfInterest closestNode = null;
        foreach (var node in m_Nodes)
        {
            float distance = Vector3.Distance(node.transform.position, transform.position) - node.Radius;
            
            if (distance < minDistance)
            {
                minDistance = distance;
                closestNode = node;
            }
        }
        if (closestNode != null)
        {
            people.ForceTarget(closestNode);
        }
    }

    private void TellWhereToGo(People people)
    {
        int randomNodeIndex = UnityEngine.Random.Range(0, m_Nodes.Count);
        people.ForceTarget(m_Nodes[randomNodeIndex]);
    }

    private void TellWhereToGo(People people, NodeOfInterest node)
    {
        people.ForceTarget(node);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        m_Nodes = new List<NodeOfInterest>(FindObjectsOfType<NodeOfInterest>());
        foreach (var people in FindObjectsOfType<People>())
        {
            RegisterPeople(people);
        }
    }
}
