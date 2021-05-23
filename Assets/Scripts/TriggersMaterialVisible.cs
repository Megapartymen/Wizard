using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersMaterialVisible : MonoBehaviour
{
    [SerializeField] Material _triggerMaterial;

    private void Start()
    {
        _triggerMaterial.color = new Color(1, 1, 1, 0);
    }

    public void SetMaterialVisible()
    {
        _triggerMaterial.color = new Color(1, 1, 1, 0.1f);
    }

    public void SetMaterialInvisible()
    {
        _triggerMaterial.color = new Color(1, 1, 1, 0);
    }
}
