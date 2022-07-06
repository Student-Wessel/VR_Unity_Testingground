using System;
using UnityEngine;
using UnityEngine.Events;

public class WhackMoleHammer : MonoBehaviour
{
    [SerializeField] private string moleString = "Mole";
    [SerializeField] private float swingTreshold = 0.004f,swingCooldown = 0.25f;
    [SerializeField] private GameObject swingPosition;

    private float cooldownCountDown = 0f;
    
    public UnityEvent MoleHit;
    public event Action<Mole> HammerMoleHit;

    private Vector3 previousPosition;
    private ExponentialMovingAverage Yema;

    private Vector3 startpos;
    private Quaternion startrot;
    
    private bool isSwining = false;
    
    private void Awake()
    {
        Yema = new ExponentialMovingAverage(10);
        startpos = transform.position;
        startrot = transform.rotation;
    }

    private void Update()
    {
        cooldownCountDown = Mathf.Max(cooldownCountDown - Time.deltaTime, -0.1f);

        if (transform.position.y < 0.3f)
        {
            transform.position = startpos;
            transform.rotation = startrot;
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = swingPosition.transform.position - previousPosition;
        Yema.Add(velocity.y);

        var lenght = Vector3.Magnitude(new Vector3(0, Yema.Value, 0));
        isSwining = lenght > swingTreshold;
        
        previousPosition = swingPosition.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSwining)
            return;

        if (other.transform.CompareTag(moleString) && cooldownCountDown <= 0)
        {
            var mole = other.transform.GetComponent<Mole>();

            if (mole == null)
                return;
            
            mole.OnHit();
            HammerMoleHit?.Invoke(mole);
            MoleHit?.Invoke();
            cooldownCountDown = swingCooldown;
        }
    }
}
