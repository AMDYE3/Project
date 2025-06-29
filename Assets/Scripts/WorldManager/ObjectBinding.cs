using UnityEngine;

[CreateAssetMenu(fileName = "ObjectBindingConfig", menuName = "Create Object Binding Config")]
public class ObjectBinding : ScriptableObject
{
    [SerializeField]
    public GameObject[] Value;

    [SerializeField]
    public GameObject Debug;
}
