using UnityEngine;
using Mirror;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public float lerpSpeed = 1.0f;
        private Vector3 offset;

        private Vector3 targetPos;
        GameObject localPlayerObj;

        private void Update()
        {
            localPlayerObj = NetworkClient.localPlayer?.gameObject;
            if (localPlayerObj == null)
                return;
            targetPos = NetworkClient.localPlayer.gameObject.transform.position;
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                lerpSpeed * Time.deltaTime
            );
        }
    }
}
