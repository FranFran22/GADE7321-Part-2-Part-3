using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectorCreator : MonoBehaviour
{
    [SerializeField] private Material freeSquareMat;
    [SerializeField] private Material oppSquareMat;
    [SerializeField] private GameObject prefab;

    private List<GameObject> instantiateSelectors = new List<GameObject>();


    public void ShowSelection(Dictionary<Vector3, bool> squareData)
    {
        ClearSelection();

        foreach (var data in squareData)
        {
            GameObject selector = Instantiate(prefab, new Vector3(data.Key.x + 13.4f, 5, data.Key.z), Quaternion.identity);
            instantiateSelectors.Add(selector);

            foreach (var setter in selector.GetComponentsInChildren<MaterialSetter>())
            {
                setter.SetAMaterial(data.Value ? freeSquareMat : oppSquareMat);
            }
        }
    }

    public void ClearSelection()
    {
        foreach (var selector in instantiateSelectors)
        {
            Destroy(selector.gameObject);
        }
        instantiateSelectors.Clear();
    }
}
