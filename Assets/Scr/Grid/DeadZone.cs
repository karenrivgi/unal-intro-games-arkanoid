using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
   //Detecta si la pelota colisionó con el BottomWall
    private void OnCollisionEnter2D(Collision2D other)
   {
      if (other.transform.TryGetComponent<Ball>(out Ball ball))
      {
         ArkanoidEvent.OnBallReachDeadZoneEvent?.Invoke(ball);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if(other.transform.TryGetComponent<PowerUp>(out PowerUp powerup))
      {
         if (other.gameObject != null)
         {
            Destroy(other.gameObject);
         }
      }
   }
}
