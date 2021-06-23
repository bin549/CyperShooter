using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configure/MaterialCtrlData")]
public class MaterialCtrl : ScriptableObject
{
    public List<ImpactMaterial> ImpactMaterialData;
}

[System.Serializable]
public class ImpactMaterial
{
    public string Tag;
    public GameObject ImpactHitEffectPrefab;
}
