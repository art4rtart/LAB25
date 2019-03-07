using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponSway : MonoBehaviour
{

    public float swayAmount = 0.02f;
    public float smoothAmount = 6f;
    public float maxAmount = 0.06f;

    private Vector3 originalPosition;

    // Use this for initialization
    private void Start()
    {
        originalPosition = transform.localPosition;
     
    }

    // Update is called once per frame
    private void Update()
    {
        float positionX = -Input.GetAxis("Mouse X") * swayAmount;
        float positionY = -Input.GetAxis("Mouse Y") * swayAmount;
        float rotationX = -Input.GetAxis("Mouse Y") * swayAmount;
        float rotationY = -Input.GetAxis("Mouse X") * swayAmount * 2f;

        Mathf.Clamp(positionX, -maxAmount, maxAmount);
        Mathf.Clamp(positionY, -maxAmount, maxAmount);
        Mathf.Clamp(rotationX, -maxAmount, maxAmount);
        Mathf.Clamp(rotationY, -maxAmount, maxAmount);

        Vector3 swayPosition = new Vector3(positionX, positionY, 0);
        Quaternion swayRotation = new Quaternion(rotationX, rotationY, 0, 1);

        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + swayPosition, Time.deltaTime * smoothAmount);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, swayRotation, Time.deltaTime * smoothAmount);
    }
}
