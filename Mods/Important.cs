using StupidTemplate.Menu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StupidTemplate.Mods
{
    public class Important
    {
        private static Vector3 oldLocalPosition;
        private static bool isClicking;

        public static void PCButtonClick()
        {
            if (Mouse.current == null)
                return;
            
            if (GorillaTagger.Instance == null ||
                GorillaTagger.Instance.rightHandTriggerCollider == null)
                return;

            var hand = GorillaTagger.Instance.rightHandTriggerCollider.transform;
            var follow = hand.GetComponent<TransformFollow>();

            if (follow == null)
                return;

            if (!Mouse.current.leftButton.isPressed)
            {
                if (isClicking)
                    RestoreHand(hand, follow);

                return;
            }

            if (!isClicking)
            {
                isClicking = true;
                oldLocalPosition = hand.localPosition;
                follow.enabled = false;
            }
            
            Ray ray = Main.TPC.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 512f, Main.NoInvisLayerMask()))
            {
                hand.position = hit.point;
            }
        }

        private static void RestoreHand(Transform hand, TransformFollow follow)
        {
            hand.localPosition = oldLocalPosition;
            follow.enabled = true;
            isClicking = false;
        }
        
        public static void DisablePCButtonClick()
        {
            if (GorillaTagger.Instance == null) return;

            var hand = GorillaTagger.Instance.rightHandTriggerCollider?.transform;
            if (hand == null) return;

            var follow = hand.GetComponent<TransformFollow>();
            if (follow == null) return;

            RestoreHand(hand, follow);
        }
    }
}