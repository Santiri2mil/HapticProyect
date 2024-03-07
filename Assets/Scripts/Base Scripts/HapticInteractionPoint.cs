using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapticInteractionPoint : MonoBehaviour {
    
    // establish Haptic Manager and IHIP objects
    public GameObject hapticManager;
    public GameObject IHIP;
    public Text posText;
    public Text rotText;

    // get haptic device information from the haptic manager
    private HapticManager myHapticManager;
    
    // haptic device number
    public int hapticDevice;
    // haptic device variables
    private Vector3 position;
    private Quaternion orientation;
    private bool button0;
    private bool button1;
    private bool button2;
    private bool button3;
    public float mass;
    private Material material;
    private Rigidbody rigidBody;

    // Called when the script instance is being loaded
    void Awake() {
        position = new Vector3(0, 0, 0);
        button0 = false;
        button1 = false;
        button2 = false;
        button3 = false;
        material = IHIP.GetComponent<Renderer>().material;
        rigidBody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        myHapticManager = (HapticManager)hapticManager.GetComponent(typeof(HapticManager));
	}
	
	// Update is called once per frame
	void Update () {

        // get haptic device to be used
        int hapticsFound = myHapticManager.GetHapticDevicesFound();//detecto dispositivo
        hapticDevice = (hapticDevice > -1 && hapticDevice < hapticsFound) ? hapticDevice : hapticsFound - 1;//bandera control

        // get haptic device variables
        position = myHapticManager.GetPosition(hapticDevice);
        posText.text = "Position: " + position.ToString();//Actualizar posición
        orientation = myHapticManager.GetOrientation(hapticDevice);
        rotText.text = "Rotation" + orientation.ToString();//Rotación
        button0 = myHapticManager.GetButtonState(hapticDevice, 0);
        button1 = myHapticManager.GetButtonState(hapticDevice, 1);
        button2 = myHapticManager.GetButtonState(hapticDevice, 2);
        button3 = myHapticManager.GetButtonState(hapticDevice, 3);

        // update haptic device mass
        mass = (mass > 0) ? mass : 0.0f;
        rigidBody.mass = mass;//actualizar masa

        // update positions of HIP and IHIP
        IHIP.transform.position = position;
        IHIP.transform.rotation = orientation;
        transform.position = position;
        transform.rotation = orientation;

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
    }
}

