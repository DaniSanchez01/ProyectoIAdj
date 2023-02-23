using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    public struct Collision {
        public Vector3 position;
        public Vector3 normal;


    }

    public static Collision getCollision(Vector3 position, Vector3 moveAmount) {
        Collision answer = new Collision();
        Ray rayo= new Ray(position, moveAmount);
        RaycastHit hitInfo;
        if (Physics.Raycast(rayo, out hitInfo))
            {
                if (hitInfo.distance <= moveAmount.magnitude) {
                    answer.position = hitInfo.point;
                    answer.normal = hitInfo.normal;
                    return answer;
                }
            }
        return answer;
    }
}
