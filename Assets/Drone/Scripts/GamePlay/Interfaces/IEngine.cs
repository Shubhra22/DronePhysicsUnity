using UnityEngine;

namespace Drone.Scripts.GamePlay
{
    public interface IEngine
    {
        void InitEngine();
        void UpdateEngine(Rigidbody rb, InputManager inputs);
    }

}
