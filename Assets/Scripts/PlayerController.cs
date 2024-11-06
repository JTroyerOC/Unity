using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 1;
    public float LookSpeed = 0.25f;

    public bool EnableInput;

    public GameObject Projectile;
    public float ProjectileSpeed = 50f;
    public Transform FireTransform;
    public float FireTimeout = 0.5f;
    
    private bool _shouldFire;
    private float _timeSinceLastFire;

    private Animator _animator;
    private static readonly int XVel = Animator.StringToHash("XVel");
    private static readonly int YVel = Animator.StringToHash("YVel");

    private CharacterController _characterController;

    private Vector2 _walkMovement = Vector2.zero;
    private Vector2 _lookVector = Vector2.zero;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(EnableInputAfterTimeout());
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!EnableInput) return;
        _walkMovement = ctx.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        if (!EnableInput) return;
        _lookVector = ctx.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if(!EnableInput) return;
        _shouldFire = ctx.ReadValueAsButton();
    }

    private IEnumerator EnableInputAfterTimeout()
    {
        yield return new WaitForSeconds(0.1f);
        EnableInput = true;
    }

    private void Update()
    {
        //TODO: perf, fix allocations
        #region Looking

        //Handle horizontal turning
        var currentHorizontalAngle = transform.localEulerAngles.y;
        var horizontalAddAmount = _lookVector.x * LookSpeed;
        var smoothedHorizontalAngle = Mathf.Lerp(currentHorizontalAngle, currentHorizontalAngle + horizontalAddAmount, Time.deltaTime);
        
        transform.localEulerAngles = new Vector3(0, smoothedHorizontalAngle, 0);

        //Handle vertical look
        var currentVertAngle = _camera.transform.localEulerAngles.x;
        var vertAddAmount = _lookVector.y * LookSpeed * -1f;
        var smoothedVertAngle = Mathf.Lerp(currentVertAngle, currentVertAngle + vertAddAmount, Time.deltaTime);

        _camera.transform.localEulerAngles = new Vector3(smoothedVertAngle, 0, 0);

        #endregion

        #region Moving

        _animator.SetFloat(XVel, _walkMovement.x);
        _animator.SetFloat(YVel, _walkMovement.y);
        
        var totalMovement = _walkMovement * Speed;
        _characterController.SimpleMove(transform.forward * totalMovement.y + transform.right * totalMovement.x);
        // _characterController.Move(Physics.gravity * Time.deltaTime);

        #endregion

        #region Firing

        switch (_shouldFire)
        {
            case true when _timeSinceLastFire <= 0:
            {
                var projectile = Instantiate(Projectile, FireTransform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().AddForce(FireTransform.forward * ProjectileSpeed, ForceMode.Impulse);
                
                _timeSinceLastFire = FireTimeout;
                break;
            }
            case true:
                _timeSinceLastFire -= Time.deltaTime;
                break;
            case false:
                _timeSinceLastFire = 0;
                break;
        }

        #endregion
    }
}
