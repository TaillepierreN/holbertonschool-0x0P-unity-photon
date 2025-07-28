using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();

        if (targetView != null && !targetView.IsMine)
        {
            targetView.RPC("TakeDamage", RpcTarget.All, damage);
        }

        Destroy(gameObject);
    }
}
