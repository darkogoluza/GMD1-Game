using UnityEngine;

public class Entity : MonoBehaviour, IEntity
{
    private string _id;

    public void AssignId(string id)
    {
        _id = id;
    }

    private void OnDestroy()
    {
        GameManager.Instance?.OnEntityDestroy(_id);
    }
}
