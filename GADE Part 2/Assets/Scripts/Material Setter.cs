using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private MeshRenderer meshRenderer
    {
        get
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();
            return _meshRenderer;
        }
    }

    public void SetAMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}