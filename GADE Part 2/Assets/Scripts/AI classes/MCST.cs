using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCST : MonoBehaviour
{
    // Monte Carlo Search Tree class

    // Selection:
    //   - selection formula
    //   - keep track of if the node has been chosen

    // Expansion:
    //   - choose the moves to move to the next node

    // Simulation:
    //   - expand each node (& chosen moves) to find the terminal state (either 0 or 1)

    // Backpropagation:
    //   - traverse back along the chosen nodes


    public List<Node> nodes = new List<Node>(); // stores all the generated nodes


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


}
