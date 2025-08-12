using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private Flag _flagPrefab;

    public Flag Flag { get; private set; } = null;
    public bool IsFlagSet { get; private set; } = false;

    private void Awake()
    {
        Flag = Instantiate(_flagPrefab, transform);
        HideFlag();
    }

    public void SetFlag(Vector3 position)
    {
        if (IsFlagSet == false)
        {
            Flag.gameObject.SetActive(true);
            IsFlagSet = true;
        }
        
        Flag.transform.position = position;
    }

    public void HideFlag()
    {
        IsFlagSet = false;
        Flag.gameObject.SetActive(false);
    }
}