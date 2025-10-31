using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanonRotation : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public Vector3 _maxRotation;
    public Vector3 _minRotation;
    private float _offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 0;
    public float MaxSpeed;
    public float MinSpeed;
    public GameObject PotencyBar;
    private float _initialScaleX;
    private Vector2 _distanceBetweenMouseAndPlayer;
    private bool isRaising = false;
    [SerializeField] private float _multiplier = 10f;
    private float ang;
    private InputSystem_Actions inputAction;

    private void Awake()
    {
        _initialScaleX = PotencyBar.transform.localScale.x;
        inputAction = new InputSystem_Actions();
        inputAction.Player.SetCallbacks(this);
    }
    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();

    }
    void Update()
    {
         //obtenir el valor del click del cursor (Fer amb new input system)
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        _distanceBetweenMouseAndPlayer = new Vector2(mousePos.x - ShootPoint.transform.position.x, mousePos.y - ShootPoint.transform.position.y);
        ang = (Mathf.Atan2(_distanceBetweenMouseAndPlayer.y, _distanceBetweenMouseAndPlayer.x) * 180f / Mathf.PI + _offset);
        if(ang > 35)
        {
            ang = 35;
        }
        if(ang < -35)
        {
            ang = -35;
        }
        //transform.rotation = Quaternion.Euler(0,0,0); //en quin dels tres eixos va l'angle?
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, ang);
        transform.rotation = targetRotation;
        //Debug.Log(ang);

        if (isRaising)
        {
            ProjectileSpeed = Time.deltaTime * _multiplier + ProjectileSpeed; //acotar entre dos valors (mirar variables)
            CalculateBarScale();
        }
        
        CalculateBarScale();

    }
    public void CalculateBarScale()
    {
        PotencyBar.transform.localScale = new Vector3(Mathf.Lerp(0, _initialScaleX, ProjectileSpeed / MaxSpeed),
            transform.localScale.y,
            transform.localScale.z);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //Instantiate(Bullet, ShootPoint.transform.position, Quaternion.Euler(0f, 0f, ang));
        OnLeftClick(context, ShootPoint);
    }

    public void OnLeftClick(InputAction.CallbackContext context, GameObject Shootpoint)
{
   if (context.started)
   {
       isRaising = true;
   }
   if (context.canceled)
   {
       var projectile = Instantiate(Bullet, Shootpoint.transform.position, Quaternion.identity); //canviar la posició on s'instancia
            projectile.GetComponent<Rigidbody2D>().linearVelocity = _distanceBetweenMouseAndPlayer.normalized * ProjectileSpeed;
       ProjectileSpeed =0;
       isRaising = false;
   }
}

   
}
