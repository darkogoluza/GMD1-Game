using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{

    [SerializeField]
    Vector3 startSize;
    [SerializeField]
    Vector3 EndsSize;
    [SerializeField]
    float ExpandeSpeed;

    bool finished = false;
    public void Start()
    {
        transform.localScale = startSize;
    }

    void Update()
    {
        if (!finished)
            transform.localScale += Vector3.one * Time.deltaTime * ExpandeSpeed;

        if (transform.localScale.magnitude >= EndsSize.magnitude)
        {
            finished = true;
            transform.localScale = startSize;
        }

    }

    private void OnEnable()
    {
        finished = false;
    }
}
