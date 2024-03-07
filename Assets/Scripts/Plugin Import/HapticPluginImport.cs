using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HapticPluginImport : MonoBehaviour {

    // Constructor
    [DllImport("C++ DLL for Unity")]
    public static extern IntPtr CreateHapticDevices();

    // Destructor
    [DllImport("C++ DLL for Unity")]
    public static extern void DeleteHapticDevices(IntPtr hapticPlugin);

    // get haptic device information
    [DllImport("C++ DLL for Unity")]
    public static extern int GetHapticsDetected(IntPtr hapticPlugin);

    // get haptic device information
    [DllImport("C++ DLL for Unity")]
    public static extern double GetHapticsDeviceInfo(IntPtr hapticPlugin, int numHapDev, int var);

    [DllImport("C++ DLL for Unity")]
    public static extern Vector3 GetHapticsPositions(IntPtr hapticPlugin, int numHapDev);

    [DllImport("C++ DLL for Unity")]
    public static extern Quaternion GetHapticsOrientations(IntPtr hapticPlugin, int numHapDev);

    [DllImport("C++ DLL for Unity")]
    public static extern double GetHapticsGripperAngle(IntPtr hapticPlugin, int numHapDev);

    [DllImport("C++ DLL for Unity")]
    public static extern Vector3 GetHapticsLinearVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport("C++ DLL for Unity")]
    public static extern Vector3 GetHapticsAngularVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport("C++ DLL for Unity")]
    public static extern double GetHapticsGripperAngularVelocity(IntPtr hapticPlugin, int numHapDev);

    [DllImport("C++ DLL for Unity")]
    public static extern bool GetHapticsButtons(IntPtr hapticPlugin, int numHapDev, int button);

    // set haptic device forcefeedback
    [DllImport("C++ DLL for Unity")]
    public static extern void SetHapticsForce(IntPtr hapticPlugin, int numHapDev, Vector3 sentForce);

    [DllImport("C++ DLL for Unity")]
    public static extern void SetHapticsTorque(IntPtr hapticPlugin, int numHapDev, Vector3 sentTorque);

    [DllImport("C++ DLL for Unity")]
    public static extern void SetHapticsGripperForce(IntPtr hapticPlugin, int numHapDev, double sentGripperForce);

    // main haptics rendering loop
    [DllImport("C++ DLL for Unity")]
    public static extern void UpdateHapticDevices(IntPtr hapticPlugin, int numHapDev);
}
