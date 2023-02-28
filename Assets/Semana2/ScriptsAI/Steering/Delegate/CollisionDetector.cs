using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    public struct Collision {
        public Vector3 position;
        public Vector3 normal;


    }

    public static Collision getCollision(Vector3 position, Vector3 bigote) {
        Collision answer = new Collision();
        answer.position = Vector3.zero;
        answer.normal = Vector3.zero;
        Ray rayo= new Ray(position, bigote);
        RaycastHit hitInfo;
        if (Physics.Raycast(rayo, out hitInfo))
            {
                if (hitInfo.distance <= bigote.magnitude) {
                    answer.position = hitInfo.point;
                    answer.normal = hitInfo.normal;
                    if (answer.normal.y != 0f) answer.normal.y =0f;
                    if (answer.position.y != 0f) answer.position.y =0f;
                    return answer;     
                }
            }
        return answer;
    }
}
