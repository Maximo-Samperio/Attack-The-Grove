using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class PatrolState : MonoBehaviour
{
    public Transform[] waypoints;                   // Array to hold the patrol waypoints
    private int currentWaypointIndex = 0;           // Index of the current waypoint
    private NavMeshAgent agent;                     // Reference to the NavMeshAgent component

    [HideInInspector]
    public bool isWaiting = false;                  // Flag to indicate if the enemy is currently waiting
    private float waitDuration = 3f;                // Duration of wait time in seconds

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();       // Get the NavMeshAgent component
        MoveToWaypoint(currentWaypointIndex);       // Start patrolling from the first waypoint
    }

    private void Update()
    {
        // Check if the agent has reached the current waypoint
        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Start waiting
            isWaiting = true;
            Invoke(nameof(ContinuePatrol), waitDuration);   // Wait for the specified duration before continuing patrol
        }
    }

    private void MoveToWaypoint(int index)
    {
        agent.SetDestination(waypoints[index].position);    // Set the destination to the position of the current waypoint
    }

    private void ContinuePatrol()
    {
        isWaiting = false; // End the waiting period

        // Roulette to go to a random waypoint inside the array
        CalculateProbabilitiesAndMoveToNextWaypoint();
    }

    private void CalculateProbabilitiesAndMoveToNextWaypoint()
    {
        List<float> probabilities = CalculateProbabilities();
        int selectedIndex = RouletteWheelSelection(probabilities);

        if (selectedIndex != -1)
        {
            currentWaypointIndex = selectedIndex;
            MoveToWaypoint(currentWaypointIndex);
        }
        else
        {
            Debug.LogError("No waypoints available for selection");
        }
    }

    private List<float> CalculateProbabilities()
    {
        List<float> distances = new List<float>();

        // Calculates distances from the agent to each waypoint
        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.position);
            distances.Add(distance);
        }

        List<float> probabilities = new List<float>();
        float totalDistance = distances.Sum();

        foreach (float distance in distances)
        {
            float probability = 1f - (distance / totalDistance);
            probabilities.Add(probability);
        }

        return probabilities;
    }

    private int RouletteWheelSelection(List<float> probabilities)
    {
        float totalProbability = probabilities.Sum();
        float randomValue = Random.value * totalProbability;  // Generate a random number between 0 and the sum of probabilities

        // Make selection based on probabilities
        float cumulativeProbability = 0f;
        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return i;
            }
        }

        // If no waypoint can be selected (should not happen if probabilities are correctly calculated)
        Debug.LogError("No waypoints available for selection");
        return -1;
    }
}
