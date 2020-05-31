using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drone.Scripts.GamePlay
{
    [RequireComponent(typeof(BoxCollider))]
    public class DroneEngine : MonoBehaviour,IEngine
    {
        #region Variables

        [Header("Engine Properties")] 
        [SerializeField] private float maxPower = 4;

        [Header("Propeller Properties")] 
        [SerializeField] private Transform propeller;
        [SerializeField] private float maxpropRotSpeed;
        #endregion

        #region Interface Methods
        public void InitEngine()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateEngine(Rigidbody rb, InputManager inputs)
        {
            Vector3 upVector = transform.up;
            upVector.x = 0;
            upVector.z = 0;
            float diff = 1 - upVector.magnitude;
            float finalDiff = diff * Physics.gravity.magnitude;
            
            Vector3 engineForce = Vector3.zero;
            engineForce = transform.up * ((rb.mass*Physics.gravity.magnitude + finalDiff ) + (inputs.Throttle * maxPower))/4;
            rb.AddForce(engineForce,ForceMode.Force);
            HandlePropellers();
        }

        private void HandlePropellers()
        {
            float propRotSpeed = 0;
            if (!propeller)
            {
                return;
            }
            if (Physics.Raycast(transform.position, Vector3.down, 0.1f,LayerMask.GetMask("Ground")))
            {
                propRotSpeed = Mathf.Lerp(propRotSpeed, 0, 4 * Time.deltaTime);
            }
            else
            {
                propRotSpeed = Mathf.Lerp(propRotSpeed, maxpropRotSpeed, 10* Time.deltaTime);
            }
            propeller.Rotate(Vector3.forward,propRotSpeed);
        }
        
        

        #endregion

    }

}
