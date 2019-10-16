using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class HumanoidPlayer : MonoBehaviour
    {
        void OnUpdate()
        {
            GetComponent<Rigidbody>().AddForce(1000 * new float3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * Time.deltaTime);
        }
    }
}