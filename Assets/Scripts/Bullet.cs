using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public int shooterActorNumber;

    private void OnTriggerEnter(Collider other)
    {
        PhotonView targetView = other.GetComponent<PhotonView>();
        if (targetView != null && targetView.Owner.ActorNumber == shooterActorNumber)
            return;

        if (other.CompareTag("Player"))
        {
            if (targetView != null && targetView.IsMine == false)
            {
                targetView.RPC("TakeDamage", RpcTarget.All, damage);
            }
        }

        Destroy(gameObject);
    }
}
