 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HIP_EF : MonoBehaviour
{
    // establish Haptic Manager and IHIP objects
    public GameObject hapticManager;
    public GameObject IHIP;

    //User Interface
    public Text px, py, pz, angleResult, peso, hy, d1;
    public Slider massaObjeto;

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
        
        distance2 = 8.5f;
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

        rbMasa.mass = massaObjeto.value;//SLIDER DE MASA!!!!!!!!!!
        peso.text = (rbMasa.mass*9,8).ToString();
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
        py.text = "Y: "+Mathf.Round( Ydistance);
        px.text = "X: " + IHIP.transform.localPosition.x;
        pz.text = "Z: " + IHIP.transform.localPosition.z;

        YmassDistance = masaDePolea.transform.position.y;//Ubicación de la masa
        ////Calculos
        //distance1 = YmassDistance;
        //distance2 = 9.25f;
        //hypotenuse = Mathf.Sqrt((distance1 * distance1) + (distance2 * distance2));
        //Debug.Log("hipotenusa: " + hypotenuse);
        //senF = distance1 / hypotenuse;
        //teta = Mathf.Asin(senF);//Angulo polea masa
        //Debug.Log("teta: " + teta * Mathf.Rad2Deg);

        if (previousPopsition > Ydistance&& position.y > -2 && position.y < 9)//Bajamos la masa si movemos el haptico hacia arriba
        {
            rbMasa.useGravity = false;
            IHIP.transform.position = new Vector3(0, position.y, 0);//Mover indicador en los limites
            transform.position = new Vector3(0, position.y, 0);//Mantener solo en Y
            if (masaDePolea.transform.position.y < 8)
                masaDePolea.transform.position = new Vector3(masaDePolea.transform.position.x, masaDePolea.transform.position.y + Mathf.Abs(Ydistance) * Time.deltaTime, masaDePolea.transform.position.z);
            else if (masaDePolea.transform.position.y >= 8)
                masaDePolea.transform.position = new Vector3(masaDePolea.transform.position.x, 8, 0);

            cambioPocision = true;
        }
        else if(previousPopsition<Ydistance)
        {
            cambioPocision = false;
            masaDePolea.transform.position = new Vector3(masaDePolea.transform.position.x, masaDePolea.transform.position.y - Mathf.Abs(Ydistance) * Time.deltaTime, masaDePolea.transform.position.z);
            rbMasa.useGravity = true;
        }
        else
        {
            rbMasa.useGravity = false;
        }
        previousPopsition = Ydistance;

        //CALCULOS FISICOS
        //massDistancceDiference = Ydistance - 4.5f;
        distance1 = Mathf.Abs(9.3f-YmassDistance);
        d1.text = distance1.ToString();
        //.Log("Distancia del triangulo: " + distance1);
        hypotenuse = Mathf.Sqrt((distance1 * distance1) + (distance2 * distance2));
        hy.text = hypotenuse.ToString();
        senF = distance1 / hypotenuse;
        teta = Mathf.Asin(distance1 / hypotenuse) *Mathf.Rad2Deg;//Angulo polea masa
        angleResult.text = teta.ToString();

        // update position
        if (position.y>-2&&position.y<9)//Limite de haptico
        {

            //moveMass(cambioPocision);
            IHIP.transform.position = new Vector3(0, position.y, 0);//Mover indicador en los limites
            transform.position = new Vector3(0, position.y, 0);//Mantener solo en Y

            //Debug.Log("teta: " + teta * Mathf.Rad2Deg);
            cambioPocision = false;

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

        
        //pruebas "gravedad"



        //Debug.Log("Diferencia masa y cuerda: "+massDistancceDiference);
        //Debug.Log("Diferencia cuerda y polea: " + massDistancceDiference);

        if ((masaDePolea.transform.position.y >= -4.6 && masaDePolea.transform.position.y <= 4.1f)&& canMove)
        {
           
            masaDePolea.transform.position = new Vector3(masaDePolea.transform.position.x, masaDePolea.transform.position.y + Mathf.Abs(Ydistance )* Time.deltaTime, masaDePolea.transform.position.z);
            rbMasa.useGravity = false;

        }
        //Debug.Log("Distancia recorrida: "+masaDePolea.transform.position.y + Mathf.Abs(massDistancceDiference));
        
        //rbMasa.mass = (rbMasa.mass + Mathf.Abs(massDistancceDiference))*0.4f;
      

    }
    
}