
using UnityEngine;

public class CineController : Singleton<CineController>
{
    // [SerializeField] private float m_shakeDuration = 0.3f;
    protected override void Awake()
    {
        MakeSingleton(false);
    }

    private void Start()
    {

    }

    void Update()
    {
        
    }

    public void ShakeTrigger()
    {
        
    }
}
