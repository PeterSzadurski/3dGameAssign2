using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scr_player : MonoBehaviour
{

    #region OffSets and Joints
    Animator _Anim;
    Transform _Hips;
    Transform _Spine;
    Transform _RArm;
    Transform _LArm;
    Transform _RHand;
    Transform _LHand;
    [SerializeField]
    private Vector3 _LeftArmOffset;
    [SerializeField]
    private Vector3 _HipsOffSet;
    [SerializeField]
    private Vector3 _RightArmOffset;
    [SerializeField]
    private Vector3 _LeftHandOffset;
    [SerializeField]
    private Vector3 _RightHandOffset;
    [SerializeField]
    private Vector3 _AimOffset;
    [SerializeField]
    private Vector3 _AdjustSpineOffset;
    #endregion

    #region UI Stuff
    [SerializeField]
    private scr_crosshair _Crosshair;

    [SerializeField]
    private scr_reloadIndicator _Reload;

    private scr_ammo _Ammo;

    private TextMeshProUGUI _AmmoText;

    private scr_dashbar _Dashbar;
    #endregion

    #region Stats
    [SerializeField]
    private float _DashMulti;
    [SerializeField]
    private float _DashcoolDown = 2f;
    [SerializeField]
    float baseSpeed;
    #endregion


    CharacterController _CC;
    [SerializeField]
    private Transform _BulletOut;



    [SerializeField]
    private GameObject _Bullet;

    private bool _ShotFiring;



    [SerializeField]
    private float _ReloadTime = 3.0f;


    private AudioSource _AS;

    [SerializeField]
    private AudioClip[] _Sounds;


    [SerializeField]
    Transform _AimHeight;


    public bool noOffset = true;


    private float _CurrentDashMulti = 1;

    private bool IsAiming;

    Plane _AimPlane;

    bool _CanDash = true;

    // Start is called before the first frame update
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Hips = _Anim.GetBoneTransform(HumanBodyBones.Hips);
        _Spine = _Anim.GetBoneTransform(HumanBodyBones.Spine);
        _CC = GetComponent<CharacterController>();
        _RArm = _Anim.GetBoneTransform(HumanBodyBones.RightShoulder);
        _LArm = _Anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
        _RHand = _Anim.GetBoneTransform(HumanBodyBones.RightHand);
        _LHand = _Anim.GetBoneTransform(HumanBodyBones.LeftHand);
        _ShotFiring = false;
        _Ammo = GetComponent<scr_ammo>();
        _AmmoText = GameObject.FindGameObjectWithTag("AmmoText").GetComponent<TextMeshProUGUI>();
        _AS = GetComponent<AudioSource>();
        _AmmoText.text = _Ammo.GetAmmoInClip() + "/" + _Ammo.GetCurrentAmmoToString();
        _AimPlane = new Plane(Vector3.up, Vector3.zero);
        _Dashbar = GameObject.FindGameObjectWithTag("Dashbar").GetComponent<scr_dashbar>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Make movementt animation match body orientation instead of movement keys

        if (_Hips.localEulerAngles.y > 44 && _Hips.localEulerAngles.y < 152)
        {
            // Right

            Debug.Log("Right");

            _Anim.SetFloat("AxisV", horizontal);
            _Anim.SetFloat("AxisH", vertical);
        }
        else if (_Hips.localEulerAngles.y < 323 && _Hips.localEulerAngles.y > 274)
        {

            // Left
            Debug.Log("Left");
            _Anim.SetFloat("AxisV", -horizontal);
            _Anim.SetFloat("AxisH", vertical);
        }
        else if (_Hips.localEulerAngles.y > 170 && _Hips.localEulerAngles.y < 205)
        {

            _Anim.SetFloat("AxisV", -vertical);
            _Anim.SetFloat("AxisH", horizontal);
        }
        else
        {
            _Anim.SetFloat("AxisV", vertical);
            _Anim.SetFloat("AxisH", horizontal);
        }


        if (_CC.isGrounded || _CurrentDashMulti > 1)
        {



            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {
                _CC.Move(direction * GetSpd() * Time.deltaTime);
                // _Anim.blendt
            }

        }
        else
        {
            _CC.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);

        }
        #endregion



        if (Input.GetButtonDown("Fire1") && !_ShotFiring && !_Ammo.GetIsReloading())
        {
            if (_Ammo.GetAmmoInClip() > 0)
            {
                if (_Ammo.GetAmmoInClip() <= Mathf.CeilToInt((float)_Ammo.GetClipAmmo() / 4f))
                {
                    _AS.pitch = Mathf.Lerp(3, 1, (float)_Ammo.GetAmmoInClip() / (float)_Ammo.GetClipAmmo());

                }
                else
                {
                    _AS.pitch = 1;
                }
                _Ammo.SubtractAmmo(1);
                _AmmoText.text = _Ammo.GetAmmoInClip() + "/" + _Ammo.GetCurrentAmmoToString();
                StartCoroutine(FireShot());
            }
            else
            {
                _AS.pitch = 1;
                _AS.PlayOneShot(_Sounds[0]);
            }
        }

        if (Input.GetButton("Reload"))
        {
            _AS.pitch = 1;
            if (_AS.isPlaying == _Sounds[1])
            {
                _AS.Stop();
            }
            _AS.PlayOneShot(_Sounds[1]);

            _Reload.StartReload(_ReloadTime, _Ammo);
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (_CanDash)
            {

                Debug.Log("Dashing");
                StartCoroutine(Dash());
            }
        }

    }

    private float GetSpd()
    {
        return baseSpeed * _CurrentDashMulti;
    }

    private IEnumerator RecoverDash()
    {

        yield return null;
    }
    private IEnumerator FireShot()
    {
        _Crosshair.IncreaseSize();
        _ShotFiring = true;

        //_MuzzleLight.SetActive(true);
        //_MuzzleFlash.Play(true);
        _AS.PlayOneShot(_Sounds[2]);
        GameObject bullet = Instantiate(_Bullet, _BulletOut);
        bullet.transform.rotation = _BulletOut.transform.rotation;
        bullet.SetActive(true);
        bullet.transform.parent = null;
        //bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000);
        yield return new WaitForSeconds(0.1f);
        _ShotFiring = false;
        _Crosshair.OriginalSize();
        // _MuzzleLight.SetActive(false);
        //_MuzzleFlash.Stop(true);

        yield return null;
    }

    private IEnumerator Dash()
    {
        _Dashbar.SetFill(0);

        _CanDash = false;
        _CurrentDashMulti = _DashMulti;
        yield return new WaitForSeconds(0.1f);
        _CurrentDashMulti = 1;
        float time = 0;
        while (time < _DashcoolDown)
        {
            time += Time.deltaTime;
            _Dashbar.SetFill(Mathf.Lerp(0, 1, time / _DashcoolDown));
            _Dashbar.SetColor(Color.Lerp(Color.black, Color.white, time / _DashcoolDown));
            yield return null;
        }
        _CanDash = true;
        yield return null;
    }

    private void LateUpdate()
    {


        Vector3 mousePos = Input.mousePosition;

        // mousePos.y = _AimHeight.position.y;
        // Debug.Log(Vector3.Distance(_followObj2.position, _followObj.position));



        //



        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(camRay.origin, camRay.direction, Color.green);
        float rayLength;


        RaycastHit mouseHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit) && mouseHit.transform.gameObject.layer == 8)
        {
          //  Debug.Log("Hit: " + mouseHit.transform.gameObject.name);
            _Hips.LookAt(new Vector3(mouseHit.transform.position.x, _AimHeight.position.y, mouseHit.transform.position.y));
            _Spine.LookAt(new Vector3(mouseHit.transform.position.x, _AimHeight.position.y, mouseHit.transform.position.y));
            Offsets();

            _BulletOut.LookAt(mouseHit.transform.position);
        }
        else
        {
            _BulletOut.localEulerAngles = Vector3.zero;

            if (_AimPlane.Raycast(camRay, out rayLength))
            {
                Vector3 pointToLook = camRay.GetPoint(rayLength);
                pointToLook = new Vector3(pointToLook.x + 0.925401f, _AimHeight.position.y, pointToLook.z - 0.961411f);
                _Hips.LookAt(new Vector3(pointToLook.x, pointToLook.y, pointToLook.z));
                _Spine.LookAt(new Vector3(pointToLook.x, pointToLook.y, pointToLook.z));
                Offsets();
            }
        }
        

    }

    private void Offsets()
    {
        if (!noOffset)
        {
            Vector3 adjustedRight = _RArm.localEulerAngles;
            Vector3 adjustedLeft = _LArm.localEulerAngles;


            // rotate spine
            Vector3 newEulers;
            Vector3 adjustedSpine;

            newEulers = _Hips.localEulerAngles;
            newEulers.x = 0;
            newEulers += _HipsOffSet;
            _Hips.localEulerAngles = newEulers;

            adjustedSpine = _Spine.localEulerAngles;

            adjustedSpine.y += _AdjustSpineOffset.y;
            adjustedSpine.x += _AdjustSpineOffset.x;

            adjustedRight += _RightArmOffset;
            adjustedLeft += _LeftArmOffset;


            _RArm.localEulerAngles = adjustedRight;
            _LArm.localEulerAngles = adjustedLeft;

            _Spine.localEulerAngles = adjustedSpine;

        }
    }

    void OnDrawGizmos()
    {

        //Debug.DrawRay(_BulletOut.position, _BulletOut.forward * 100, Color.red);

    }
}
