using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using System.ComponentModel;
using UnityEngine.Rendering;

public class movePlayer : MonoBehaviour
{
    [Header("TOUCHE")]
    [SerializeField]
    [ReadOnly(true)]
    Vector3 dir = Vector3.zero;
    Vector3 dirRotation;
    float buttonBoost ;

    [SerializeField]
    [ReadOnly(true)]
    float molletteVitesse ;


    // COUCOU C'EST MOI


    [SerializeField]
    float rotationSpeed = 1f;


    [SerializeField]
    RectTransform cursorTransform;


    //








    [Header("WARPVFX")]
    public VisualEffect warpSpeedVFX;
    private bool warpActive;
    public float rate = 0.02f;


    [Header("MOUVEMENT")]
    public float speed = 200f;
    public float speedMax = 1500f;

    float speedCurseurs = 1500f ;
    float speedOrigine = 200f;
    float turbo = 500f;
    float turboMax = 500f;
    public GameObject vaisseau ; 
    public GameObject cam ;   
    public Image barreBoost ; 
    ///////// CURSEURS INVISIBLE, LE VAISSEAU SUIT CES MOUVEMENTS ///////////// 
    public RectTransform curseurs ; 
    Vector3 posInitViseurs = Vector3.zero;
    Vector3 targetViseurs = Vector3.zero;

    ///////// ROTATION /////////////
    Vector3 currentAngle  ;

    Vector3 targetAngle  ;

    Quaternion targetRotation  ;

    private Coroutine resetPositionCoroutine = null;



    // Start is called before the first frame update
    void Start()
    {
        speedOrigine = speed ;
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
    [Range(0.1f,5f)]
    float targetRotationAccelerationSpeed = 0.2f;


    private void UpdateTargetRotation()
    {
        //pourrait être dans une autre fonction, mais ppour l'instant s'pratique ici

        curTargetDir = Vector2.Lerp(curTargetDir, dir, targetRotationAccelerationSpeed * Time.deltaTime);
        curDirTorque = Mathf.Lerp(dirTorque, curDirTorque, targetRotationAccelerationSpeed*5 * Time.deltaTime);
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
        cursorTransform.localPosition = new Vector3 (curTargetDir.x * _cursorMaxDist, -curTargetDir.y * _cursorMaxDist,0);
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
        if (buttonBoost > 0)
        {
            targetSpeed = molletteVitesse * (speed * 10);
            StartCoroutine(ActivateParticles());
            warpActive = true;
        }
        else
        targetSpeed = molletteVitesse * speed;
        warpActive = false;

    }

    private void UpdateSpeed()
    { 
        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------


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
                if(amount <= 0 + rate)
                {
                    amount = 0;
                    warpSpeedVFX.SetFloat("WarpAmount", amount);
                    warpSpeedVFX.Stop();
                }
            }
        }
    }
}





///////////////////////------------------------------------ ARCHIVES ------------------------------------////////////////

/*/////////////////////// COROUTINES ////////////////
    private IEnumerator ResetPositionWhenNotPressingLeftOrRight()
    {
        float replacementRatio = 0.98f;
        float acceptableFloor = 0.05f;

        while (vaisseau.transform.eulerAngles.z > 150 && Mathf.Abs(vaisseau.transform.eulerAngles.z - 360) >= acceptableFloor  || vaisseau.transform.eulerAngles.z >= acceptableFloor)
        {

            Quaternion vaisseauRotation = Quaternion.identity;

            if (vaisseau.transform.eulerAngles.z > 150)
            {
                vaisseauRotation = Quaternion.Euler(new Vector3(*//* X *//*
vaisseau.transform.eulerAngles.x, *//* Y *//* vaisseau.transform.eulerAngles.y, *//* Z *//* Mathf.Lerp(360f, vaisseau.transform.eulerAngles.z, replacementRatio)));
            }
            else
{
    vaisseauRotation = Quaternion.Euler(new Vector3(*//* X *//* vaisseau.transform.eulerAngles.x, *//* Y *//* vaisseau.transform.eulerAngles.y, *//* Z *//* Mathf.Lerp(0f, vaisseau.transform.eulerAngles.z, replacementRatio)));
}

vaisseau.transform.rotation = vaisseauRotation;
cam.transform.rotation = vaisseauRotation;

yield return new WaitForSeconds(0.01f);
        }

        resetPositionCoroutine = null;
    }
    /////////////////////// FIN ////////////////*/
///

///////////////////// LA COROUTINES SERT A REDRESSER LE VAISSEAU LORSQUE LE JOUEUR LACHE LA MANETTE : FONCTIONNE UNIQUEMENT DU COTE GAUCHE ET DROITE/////////////////////////// 
/*float detectionFloor = 0.05f;

if (Mathf.Abs(dir.x) < detectionFloor && Mathf.Abs(dir.y) < detectionFloor)
{
    if (resetPositionCoroutine == null)
    {
        resetPositionCoroutine = StartCoroutine(ResetPositionWhenNotPressingLeftOrRight());
    }
}

else
{
    if (resetPositionCoroutine != null)
    {
        StopCoroutine(resetPositionCoroutine);
        resetPositionCoroutine = null;
    }
    ///////////////////// FIN /////////////////////////// 

    ///////////////////// DEPLACEMENT DU CURSEURS/////////////////////////// 

    targetViseurs = new Vector2(dir.x * speedCurseurs, dir.y * -speedCurseurs);
    posInitViseurs = Vector2.Lerp(posInitViseurs, targetViseurs, Time.deltaTime);
    curseurs.anchoredPosition = (Vector2)posInitViseurs * Time.deltaTime;
    ///////////////////// FIN /////////////////////////// 
    ///////////////////// LE GAMEOBJECT QUI CONTIENT, LE VAISSEAU, LA CAM ET LE CANVAS REGARDE TOUJOURS VERS LE CURSEURS/////////////////////////// 

    transform.LookAt(curseurs, transform.up);
    ///////////////////// FIN /////////////////////////// 

    ///////////////////// ROTATIONS DU VAISSEAU /////////////////////////// 

    currentAngle = vaisseau.transform.eulerAngles;
    targetAngle = new Vector3(currentAngle.x, currentAngle.y, currentAngle.z - 180f * dir.x * Time.deltaTime);

    ///////////////////// BLOQUE LES ROTATIONS GAUCHE ET DROITE A 45° /////////////////////////// 

    if (vaisseau.transform.eulerAngles.z >= 45 && vaisseau.transform.eulerAngles.z <= 150f && targetAngle.z > currentAngle.z || vaisseau.transform.eulerAngles.z <= 315f && vaisseau.transform.eulerAngles.z >= 150 && targetAngle.z < currentAngle.z)
    {
        targetRotation = Quaternion.Euler(currentAngle);
    }
    else
    {
        targetRotation = Quaternion.Euler(targetAngle);
    }
    ///////////////////// FIN /////////////////////////// 
    ///////////////////// ROTATION UNIQUEMENT POUR LE VAISSEAU ET LA CAMERA  /////////////////////////// 

    vaisseau.transform.rotation = targetRotation;
    cam.transform.rotation = targetRotation;

}
/////////// SECURITE SI LA BARRE N'EST PAS DISPO ///////////// 
        if (barreBoost != null)
        {

            barreBoost.fillAmount = turbo / turboMax;

        }
        /////////// FIN ///////////// 

        if (buttonBoost == 1 && turbo > 0)
        {

            warpActive = true;
            turbo = turbo - 200f * Time.deltaTime;
            StartCoroutine(ActivateParticles());

            if (speed < speedMax)
            {

                speed = speed + 1000f * Time.deltaTime;
            }
        }
        else if (buttonBoost == 0 || turbo <= 0)
        {

            if (speed > speedOrigine)
            {

                warpActive = false;
                StartCoroutine(ActivateParticles());
                speed = speed - 800f * Time.deltaTime;
            }
            // else if(speed <= speedOrigine){

            //  speed = speedOrigine * molletteVitesse/2 ; 

            //}    
        }
        if (turbo < turboMax)
        {

            turbo = turbo + 30f * Time.deltaTime;

        }
///////////////////// FIN /////////////////////////// 

///////////////////// TURBO ET JAUGE DE LA MOLETTE SPEED /////////////////////////// 

/////////// SECURITE SI LA BARRE N'EST PAS DISPO ///////////// 
if (barreBoost != null)
{

    barreBoost.fillAmount = turbo / turboMax;

}
/////////// FIN ///////////// 

if (buttonBoost == 1 && turbo > 0)
{

    warpActive = true;
    turbo = turbo - 200f * Time.deltaTime;
    StartCoroutine(ActivateParticles());

    if (speed < speedMax)
    {

        speed = speed + 1000f * Time.deltaTime;
    }
}
else if (buttonBoost == 0 || turbo <= 0)
{

    if (speed > speedOrigine)
    {

        warpActive = false;
        StartCoroutine(ActivateParticles());
        speed = speed - 800f * Time.deltaTime;
    }
    // else if(speed <= speedOrigine){

    //  speed = speedOrigine * molletteVitesse/2 ; 

    //}    
}
if (turbo < turboMax)
{

    turbo = turbo + 30f * Time.deltaTime;

}
///////////////////// FIN /////////////////////////// 

///////////////////// DEPLACEMENT POUR LE GAMEOBJECT  /////////////////////////// 

transform.Translate(Vector3.forward * speed * Time.deltaTime);
*/


//Rotation

/**/
