using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using System.ComponentModel;
using TMPro;
using UnityEngine.Rendering;

//using static System.Net.Mime.MediaTypeNames;
//using System.Diagnostics;

public class _movePlayer : MonoBehaviour
{
    [Header("TOUCHE")]
    [SerializeField]
    [ReadOnly(true)]
    Vector3 dir = Vector3.zero;
    Vector3 dirRotation;
    float buttonBoost;

    [SerializeField]
    [ReadOnly(true)]
    float molletteVitesse;


    // COUCOU C'EST MOI


    [SerializeField]
    float rotationSpeed = 1f;


    [SerializeField]
    RectTransform cursorTransform;


    //



    /// collision asteroide////////////// 


    private Coroutine impactCoroutine;
    public bool siObjStop = false;
    public float tempsDeCollision = 1.5f; 


    [Header("WARPVFX")]
    public VisualEffect warpSpeedVFX;
    private bool warpActive;
    public float rate = 0.02f;
    public Camera cameraFOVModifier;
    public float cameraFOV;
    public TextMeshProUGUI speedText;


    [Header("MOUVEMENT")]
    public bool canTurbo;
    public float speed = 200f;
    public float speedMax = 1500f;

    float speedCurseurs = 1500f;
    float speedOrigine = 200f;
    float turbo = 1000f;
    float turboMax = 1000f;
    public GameObject vaisseau;
    public GameObject cam;
    public Image barreBoost;
    ///////// CURSEURS INVISIBLE, LE VAISSEAU SUIT CES MOUVEMENTS ///////////// 
    public RectTransform curseurs;
    Vector3 posInitViseurs = Vector3.zero;
    Vector3 targetViseurs = Vector3.zero;

    ///////// ROTATION /////////////
    Vector3 currentAngle;

    Vector3 targetAngle;

    Quaternion targetRotation;

    private Coroutine resetPositionCoroutine = null;



    // Start is called before the first frame update
    void Start()
    {
        speedOrigine = speed;
        warpSpeedVFX.Stop();
        warpSpeedVFX.SetFloat("WarpAmount", 0);
    }
    void Update()
    {
        UpdateTargetRotation();
        UpdateCurRotation();
        UpdateCursorPosition();


        //Speed
        UpdateTargetSpeed();
        UpdateSpeed();

        if(barreBoost != null)
        {
            barreBoost.fillAmount = turbo / turboMax;
        }

        targetSpeed = Mathf.RoundToInt(targetSpeed);
        speedText.SetText(targetSpeed.ToString());

        float _distanceParcouredInOneFrame = curSpeed * Time.deltaTime;

        transform.position += transform.forward * _distanceParcouredInOneFrame;
    }



    //////////// DECLARATION DES INPUTS /////////////

    /// DECLARATION DU STICK ///

    // ROTATION // ---------------------------------------------------------------------------------------------------------------------

    Vector2 curTargetDir = Vector2.zero;

    public void stick(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
        Debug.Log(dir);
    }

    float dirTorque = 0f;
    float curDirTorque = 0f;

    public void TorqueInput(InputAction.CallbackContext context)
    {
        //// MOLETTE D'ORIGINE DE -1 A 1 , CONVERSION EN 0 A 1 ////////////////////
        ///A inverser aussi
        dirTorque = -context.ReadValue<float>();
    }


    Quaternion targetRot;

    [SerializeField]
    [Tooltip("Vitesse de transition des rotations visées")]
    [Range(0.1f, 5f)]
    float targetRotationAccelerationSpeed = 0.2f;


    private void UpdateTargetRotation()
    {
        //pourrait être dans une autre fonction, mais ppour l'instant s'pratique ici

        curTargetDir = Vector2.Lerp(curTargetDir, dir, targetRotationAccelerationSpeed * Time.deltaTime);
        curDirTorque = Mathf.Lerp(dirTorque, curDirTorque, targetRotationAccelerationSpeed * 5 * Time.deltaTime);
        //

        float _maxTargetTorqueAngle = 50f;

        float _maxTargetAngle = 45f;


        Quaternion _curRotation = transform.rotation;

        targetRot = _curRotation * Quaternion.Euler(curTargetDir.y * _maxTargetAngle, curTargetDir.x * _maxTargetAngle, curDirTorque * _maxTargetTorqueAngle);


    }

    [SerializeField]
    [Tooltip("Vitesse de transition de rotation, de la rotation actuelle vers la rotation visée")]
    [Range(0.1f, 5f)]
    float rotationAccelerationSpeed = 0.2f;

    private void UpdateCurRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }


    private void UpdateCursorPosition()
    {
        float _cursorMaxDist = 50f;
        cursorTransform.localPosition = new Vector3(curTargetDir.x * _cursorMaxDist, -curTargetDir.y * _cursorMaxDist, 0);
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------

    /// DECLARATION FLECHE ///

    public void fleche(InputAction.CallbackContext context)
    {
        dirRotation = context.ReadValue<Vector2>();
        Debug.Log(dirRotation);
    }
    /// DECLARATION BOUTON BOOST ///

    public void boostButton(InputAction.CallbackContext context)
    {
        buttonBoost = context.ReadValue<float>();
    }
    /// DECLARATION MOLETTE ///




    // SPEED // ---------------------------------------------------------------------------------------------------------------------

    [SerializeField]
    [Tooltip("Acceleration/deceleration")]
    [Range(0.1f, 5f)]
    float acceleration = 0.2f;

    public void vitesseButton(InputAction.CallbackContext context)
    {
        //// MOLETTE D'ORIGINE DE -1 A 1 , CONVERSION EN 0 A 1 ////////////////////
        ///A inverser aussi
        molletteVitesse = (context.ReadValue<float>() * -1 + 1) * 0.5f;
    }

    float targetSpeed = 0f;

    float curSpeed = 0f;

    private void UpdateTargetSpeed()
    {
        if (canTurbo)
        {
            if ((buttonBoost > 0 && siObjStop == false ) && turbo > 0)
            {
                AudioManager.instance.PlaySFX("Hyperdrive");
                cameraFOV = Mathf.Lerp(cameraFOV, 80, 0.01f);
                cameraFOVModifier.fieldOfView = cameraFOV;
                turbo = turbo - 100f * Time.deltaTime;

                if (targetSpeed < 2000f)
                {
                    targetSpeed += molletteVitesse * (speed * 10f) * Time.deltaTime;
                }

                StartCoroutine(ActivateParticles());
                warpActive = true;
            }
        }

        else if (targetSpeed > 200f && buttonBoost < 1 || turbo <= 0)
        {
            AudioManager.instance.StopSFX("Hyperdrive");
            cameraFOV = Mathf.Lerp(cameraFOV, 60, 0.01f);
            cameraFOVModifier.fieldOfView = cameraFOV;
            targetSpeed -= molletteVitesse * (speed * 10f) * Time.deltaTime;
            StartCoroutine(ActivateParticles());
            warpActive = false;
        }
        else if (targetSpeed < 200f && buttonBoost < 1 || turbo <= 0)
        {
            cameraFOV = Mathf.Lerp(cameraFOV, 60, 0.01f);
            cameraFOVModifier.fieldOfView = cameraFOV;
            targetSpeed = molletteVitesse * (speed * 2);
            StartCoroutine(ActivateParticles());
            warpActive = false;
        }

        if(turbo <= turboMax) //tentative de limitation du turbo, ça marche pas
        {
            canTurbo = false;
            turbo = turbo + 30f * Time.deltaTime;
        }

        if(turbo > 100)
        {
            canTurbo = true;
        }




    }

    private void UpdateSpeed()
    {
        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------
                                                                                            /// LEO //

    //COROUTINE ACTIVATION DES PARTICULES DE VITESSE//
    IEnumerator ActivateParticles()
    {
        if (warpActive)
        {
            warpSpeedVFX.Play();

            float amount = warpSpeedVFX.GetFloat("WarpAmount");

            //AUGMENTE LE NOMBRE DE PARTICULES SELON LA VARIABLE WarpAmount DU VFX GRAPH SI IL Y A UN BOOST//
            while (amount < 1 && warpActive)
            {
                amount += rate;
                warpSpeedVFX.SetFloat("WarpAmount", amount);
                yield return new WaitForSeconds(0.01f);
            }
        }

        else
        {

            float amount = warpSpeedVFX.GetFloat("WarpAmount");

            //BAISSE LE NOMBRE DE PARTICULES SELON LA VARIABLE WarpAmount DU VFX GRAPH SI IL N'Y A PAS DE BOOST//
            while (amount > 0 && !warpActive)
            {
                amount -= rate;
                warpSpeedVFX.SetFloat("WarpAmount", amount);
                yield return new WaitForSeconds(0.01f);

                //SI LES PARTICULES SONT INFERIEURES OU EGALES A ZERO ALORS STOP//
                if (amount <= 0 + rate)
                {
                    amount = 0;
                    warpSpeedVFX.SetFloat("WarpAmount", amount);
                    warpSpeedVFX.Stop();
                }
            }
        }
    }

    //// RAYAN //////////////////////
    ///
    void OnCollisionEnter(Collision collision)
    {

        if (collision.rigidbody)
        {
            if (collision.rigidbody.tag == "asteroideComplet")
            {
                siObjStop = true;

                if (impactCoroutine != null)
                {
                    warpSpeedVFX.Stop();
                    StopCoroutine(impactCoroutine);
                    impactCoroutine = null;
                }
                impactCoroutine = StartCoroutine(CollisionCoroutine());
            }
        }
    }

    private IEnumerator CollisionCoroutine()
    {
        targetSpeed = 0;
        turbo = 0;

        if(targetSpeed <= 1) 
        {
            targetSpeed += Time.deltaTime / tempsDeCollision;
            yield return new WaitForSeconds(tempsDeCollision);
        }

       // targetSpeed = 1;
        siObjStop = false;
        impactCoroutine = null;

        yield return null;
    }
}

