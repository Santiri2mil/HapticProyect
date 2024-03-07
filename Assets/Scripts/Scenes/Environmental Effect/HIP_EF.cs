using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIP_EF : MonoBehaviour
{
    // establish Haptic Manager and IHIP objects
    public GameObject hapticManager;
    public GameObject IHIP;

    // get haptic device information from the haptic manager
    private HM_EF myHapticManager;

    // haptic device number
    public int hapticDevice;
    // haptic device variables
    private Vector3 position;
    private bool button0;
    private bool button1;
    private bool button2;
    private bool button3;
    public float mass;
    private Material material;
    private Rigidbody rigidBody;
    private Rigidbody rbMasa;

    public float Ydistance, YmassDistance, originalY,originalmassY, massDistancceDiference;
    public float senF, senT, cosF, cosT;
    public float teta;
    public float distance1, distance2, hypotenuse;
    public GameObject masaDePolea;

    public Vector3 desiredPosition;

    [Header("Attraction Force")]
    // attraction force
    public bool enableForce;
    // stiffness coefficient
    public float Kp = 1000;

    [Header("Damping Factors")]
    // damping term
    public float Kv = 20; // [N/m]
    public float Kvr = 10;
    public double Kvg = 10;

    private float originalmass;

    private bool cambioPocision;
    // Called when the script instance is being loaded
    void Awake()
    {
        position = new Vector3(0, 0, 0);
        button0 = false;
        button1 = false;
        button2 = false;
        button3 = false;
        material = IHIP.GetComponent<Renderer>().material;
        rigidBody = GetComponent<Rigidbody>();
        enableForce = false;
        originalmassY = masaDePolea.transform.position.y;
        originalY= IHIP.transform.localPosition.y;
        rbMasa = masaDePolea.GetComponent<Rigidbody>();
        originalmass = rbMasa.mass;
        Application.targetFrameRate = 30;

    }

    // Use this for initialization
    void Start()
    {
        
        YmassDistance = masaDePolea.transform.position.y;
        myHapticManager = (HM_EF)hapticManager.GetComponent(typeof(HM_EF));
        //rbMasa.useGravity = false;
        distance1 = 7.5f;
        distance2 = 9.25f;
        hypotenuse = Mathf.Sqrt((distance1 *distance1) + (distance2 *distance2));
        Debug.Log("hipotenusa: " + hypotenuse);
        senF = distance1 / hypotenuse;
        teta = Mathf.Asin(senF);//Angulo polea masa
        Debug.Log("teta: " + teta * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Application.targetFrameRate!=30)
        {
            Application.targetFrameRate = 30;
        }
        // get haptic device to be used
        int hapticsFound = myHapticManager.GetHapticDevicesFound();
        hapticDevice = (hapticDevice > -1 && hapticDevice < hapticsFound) ? hapticDevice : hapticsFound - 1;
        


        // get haptic device variables
        position = myHapticManager.GetPosition(hapticDevice);
        button0 = myHapticManager.GetButtonState(hapticDevice, 0);
        button1 = myHapticManager.GetButtonState(hapticDevice, 1);
        button2 = myHapticManager.GetButtonState(hapticDevice, 2);
        button3 = myHapticManager.GetButtonState(hapticDevice, 3);

        // update haptic device mass
        mass = (mass > 0) ? mass : 0.0f;
        rigidBody.mass = mass;


        float previousPopsition = Ydistance;
        Ydistance = IHIP.transform.localPosition.y;//Ubicación del haptico

        YmassDistance = masaDePolea.transform.position.y;//Ubicación de la masa
        ////Calculos
        //distance1 = YmassDistance;
        //distance2 = 9.25f;
        //hypotenuse = Mathf.Sqrt((distance1 * distance1) + (distance2 * distance2));
        //Debug.Log("hipotenusa: " + hypotenuse);
        //senF = distance1 / hypotenuse;
        //teta = Mathf.Asin(senF);//Angulo polea masa
        //Debug.Log("teta: " + teta * Mathf.Rad2Deg);

        if (previousPopsition != Ydistance)
        {
            cambioPocision = true;
        }
        if(previousPopsition<Ydistance)
        {
            cambioPocision = false;
            masaDePolea.transform.position = new Vector3(masaDePolea.transform.position.x, masaDePolea.transform.position.y - Mathf.Abs(-Ydistance) * Time.deltaTime, masaDePolea.transform.position.z);

        }
        
        previousPopsition = Ydistance;
       

       
        // update position
        if (position.y>-8&&position.y<4.1)//Limite de haptico
        {


            //IHIP.transform.position = new Vector3(0, 0, 0);//Mover indicador en los limites
            //transform.position = new Vector3(0, 0, 0);//Mantener solo en Y

            //rbMasa.useGravity = false;
            IHIP.transform.position = new Vector3(0, position.y, 0);//Mover indicador en los limites
            transform.position = new Vector3(0, position.y, 0);//Mantener solo en Y
            moveMass(cambioPocision);

            distance1 = Mathf.Abs(massDistancceDiference);
            Debug.Log("Distancia del triangulo: " + distance1);
            senF = distance1 / hypotenuse;
            teta = Mathf.Asin(senF);//Angulo polea masa
            Debug.Log("teta: " + teta * Mathf.Rad2Deg);
            cambioPocision = false;
            //if (button0)
            //{
                

            //}
            //if(!button0)
            //{
            //    rbMasa.useGravity = true;
            //    Debug.Log("Masa con gravedad");
            //    rbMasa.mass = originalmass;
            //}
        }
        

        // change material color
        if (button0)
        {
            material.color = Color.red;
            
        }
        else if (button1)
        {
            material.color = Color.blue;

        }
        else if (button2)
        {
            material.color = Color.green;
        }
        else if (button3)
        {
            material.color = Color.yellow;
        }
        else
        {
            material.color = Color.white;
        }

        // update damping factors
        Kv = (Kv > 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 6)) ? 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 6) : Kv;
        Kvr = (Kvr > 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 7)) ? 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 7) : Kvr;
        Kvg = (Kvr > 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 8)) ? 1.0f * myHapticManager.GetHapticDeviceInfo(hapticDevice, 8) : Kvg;
    }

    public void moveMass(bool canMove)
    {
        //massDistancceDiference = Ydistance-YmassDistance;
        massDistancceDiference = Ydistance - 4.5f;

        //Debug.Log("Diferencia masa y cuerda: "+massDistancceDiference);
        //Debug.Log("Diferencia cuerda y polea: " + massDistancceDiference);

        if ((masaDePolea.transform.position.y >= -4.6 && masaDePolea.transform.position.y <= 4.1f)&& canMove)
        {
           
            masaDePolea.transform.position = new Vector3(masaDePolea.transform.position.x, masaDePolea.transform.position.y + Mathf.Abs(Ydistance )* Time.deltaTime, masaDePolea.transform.position.z);

        }
        //Debug.Log("Distancia recorrida: "+masaDePolea.transform.position.y + Mathf.Abs(massDistancceDiference));
        
        //rbMasa.mass = (rbMasa.mass + Mathf.Abs(massDistancceDiference))*0.4f;
      

    }
    
}