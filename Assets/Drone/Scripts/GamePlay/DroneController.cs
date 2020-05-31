using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Drone.Scripts.GamePlay
{
    [RequireComponent(typeof(InputManager))]
    public class DroneController : RigidBodyManager
    {
        #region Variables

        [Header("Control Property")] 
        [SerializeField] private float minMaxPitch = 30;
        [SerializeField] private float minMaxRoll = 30;
        [SerializeField] private float yawPower = 4;
        [SerializeField] private float lerpSpeed = 2;
        private InputManager input;
        private List<IEngine> _engines = new List<IEngine>();

        private float _finalPitch;
        private float _finalRoll;
        private float _finalYaw;
        private float yaw;
        
        #endregion

        #region Main Methods

        // Start is called before the first frame update
        void Start()
        {
            input = GetComponent<InputManager>();
            _engines = GetComponentsInChildren<IEngine>().ToList();
        }

        #endregion

        #region CustomMethods

        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
        }

        protected  virtual void HandleControls()
        {
            float pitch = input.Cyclic.y * minMaxPitch; // ranged from - minMaxPitch to +minMaxPitch
            float roll = -input.Cyclic.x * minMaxRoll;
            yaw += input.Pedals * yawPower;

            _finalPitch = Mathf.Lerp(_finalPitch, pitch, lerpSpeed * Time.deltaTime);
            _finalRoll = Mathf.Lerp(_finalRoll, roll, lerpSpeed * Time.deltaTime);
            _finalYaw = Mathf.Lerp(_finalYaw, yaw, lerpSpeed * Time.deltaTime);
                
            Quaternion rot = Quaternion.Euler(_finalPitch,_finalYaw,_finalRoll);
            _rb.MoveRotation(rot);
        }

        protected virtual void HandleEngines()
        {
            foreach (IEngine engine in _engines)
            {
                engine.UpdateEngine(_rb,input);
            }
        }
        #endregion
       
    }
}
