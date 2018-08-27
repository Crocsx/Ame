using UnityEngine;

public class GyroscopeManager : MonoBehaviour
{
    // TODO remove unused and less optimized
    public delegate void UpdateGyroDataQuat(Quaternion gyroAttitude);
    public event UpdateGyroDataQuat OnGyroUpdate;

    public delegate void UpdateGyroDataAngle(float angleDegree);
    public event UpdateGyroDataAngle OnGyroUpdateZAngle;

    void Start()
    {
        Input.gyro.enabled = true;
	}
	
	void Update()
    {
        bool isLandScapeLeft = Screen.orientation == ScreenOrientation.LandscapeLeft;

        Quaternion gyroQuat = new Quaternion(
            Input.gyro.attitude.x * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.y * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.z * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.w * (isLandScapeLeft ? 1 : -1)
        );
        if (OnGyroUpdate != null)
            OnGyroUpdate(gyroQuat);

        if (OnGyroUpdateZAngle != null)
            OnGyroUpdateZAngle(Input.gyro.attitude.eulerAngles.z);
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Orientation: " + Screen.orientation);
        GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);

        bool isLandScapeLeft = Screen.orientation == ScreenOrientation.LandscapeLeft;
        Quaternion gyroQuat = new Quaternion(
            Input.gyro.attitude.x * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.y * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.z * (isLandScapeLeft ? 1 : -1),
            Input.gyro.attitude.w * (isLandScapeLeft ? 1 : -1)
        );
        GUILayout.Label("gyroQuat: " + gyroQuat);

        GUILayout.Label("input.gyro.attitude.eulerAngles.z: " + Input.gyro.attitude.eulerAngles.z);
    }
}
