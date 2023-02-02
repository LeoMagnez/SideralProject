using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using TMPro;
using UnityEditor.Rendering;

public class movePlayer : MonoBehaviour
{
    [Header("TOUCHE")]
    Vector3 dir = Vector3.zero;
    Vector3 dirRotation;
    float buttonBoost ; 
    float molletteVitesse ;

    [Header("WARPVFX")]
    public VisualEffect warpSpeedVFX;
    private bool warpActive;
    public float rate = 0.02f;
    public Camera cameraModifier; // RECUPERE LA MAIN CAMERA DANS LE BUT DE CHANGER LE FOV PENDANT LE BOOST
    public float camFOV;


    [Header("MOUVEMENT")]
    public bool canControl;
    public float speed = 200f;
    public float speedMax = 1500f;
    public TextMeshProUGUI speedNumber;

    float speedCurseurs = 1500f ;
    float speedOrigine = 200f;
    float turbo = 1000f;
    float turboMax = 1000f;
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
        canControl = true; 
        speedOrigine = speed ;
        warpSpeedVFX.Stop();
        warpSpeedVFX.SetFloat("WarpAmount", 0);
    }
    void Update()
    {
        speed = Mathf.RoundToInt(speed);
        speedNumber.SetText(speed.ToString());
        if (canControl)
        {
            ///////////////////// LA COROUTINES SERT A REDRESSER LE VAISSEAU LORSQUE LE JOUEUR LACHE LA MANETTE : FONCTIONNE UNIQUEMENT DU COTE GAUCHE ET DROITE/////////////////////////// 
            float detectionFloor = 0.05f;

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
                camFOV = Mathf.Lerp(camFOV, 70, 0.01f); //LERP LE FOV DE LA CAM LORSQUE LE BOOST EST ACTIF
                cameraModifier.fieldOfView = camFOV;
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

                    camFOV = Mathf.Lerp(camFOV, 60, 0.01f); //LERP LE FOV DE LA CAM LORSQUE LE BOOST EST INACTIF
                    cameraModifier.fieldOfView = camFOV;
                    warpActive = false;
                    StartCoroutine(ActivateParticles());
                    speed = speed - 800f * Time.deltaTime;
                }
                /* else if(speed <= speedOrigine){

                     speed = speedOrigine * molletteVitesse/2 ; 

                 }  */
            }
            if (turbo < turboMax)
            {

                turbo = turbo + 30f * Time.deltaTime;

            }
            ///////////////////// FIN /////////////////////////// 

            ///////////////////// DEPLACEMENT POUR LE GAMEOBJECT  /////////////////////////// 

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }
    }


        /////////////////////// COROUTINES ////////////////
    private IEnumerator ResetPositionWhenNotPressingLeftOrRight()
    {
        float replacementRatio = 0.98f;
        float acceptableFloor = 0.05f;

        while (vaisseau.transform.eulerAngles.z > 150 && Mathf.Abs(vaisseau.transform.eulerAngles.z - 360) >= acceptableFloor  || vaisseau.transform.eulerAngles.z >= acceptableFloor)
        {

            Quaternion vaisseauRotation = Quaternion.identity;

            if (vaisseau.transform.eulerAngles.z > 150)
            {
                vaisseauRotation = Quaternion.Euler(new Vector3(/* X */ vaisseau.transform.eulerAngles.x, /* Y */ vaisseau.transform.eulerAngles.y, /* Z */ Mathf.Lerp(360f, vaisseau.transform.eulerAngles.z, replacementRatio)));
            }
            else
            {
                 vaisseauRotation = Quaternion.Euler(new Vector3(/* X */ vaisseau.transform.eulerAngles.x, /* Y */ vaisseau.transform.eulerAngles.y, /* Z */ Mathf.Lerp(0f, vaisseau.transform.eulerAngles.z, replacementRatio)));
            }

            vaisseau.transform.rotation = vaisseauRotation;
            cam.transform.rotation = vaisseauRotation;

            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }
        /////////////////////// FIN ////////////////

        //////////// DECLARATION DES INPUTS /////////////

        /// DECLARATION DU STICK ///

    public void stick(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }
        /// DECLARATION FLECHE ///

    public void fleche(InputAction.CallbackContext context)
    {
        dirRotation = context.ReadValue<Vector2>();
    }
        /// DECLARATION BOUTON BOOST ///

    public void boostButton(InputAction.CallbackContext context)
    {
        buttonBoost = context.ReadValue<float>();
    } 
        /// DECLARATION MOLETTE ///

    public void vitesseButton(InputAction.CallbackContext context)
    {
        //// MOLETTE D'ORIGINE DE -1 A 1 , CONVERSION EN 0 A 1 ////////////////////
        molletteVitesse = context.ReadValue<float>() * - 1 + 1;
   

    }
    
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

        //////////// FIN /////////////        

}
