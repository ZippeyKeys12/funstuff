using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class HumanoidPlayer : MonoBehaviour
    {
        public void OnMove(InputValue value)
        {
            var v = value.Get<Vector2>();
            
            GetComponent<Rigidbody>().AddForce(1000 * new float3(v.x, 0f, v.y) * Time.deltaTime);
        }
    }
}