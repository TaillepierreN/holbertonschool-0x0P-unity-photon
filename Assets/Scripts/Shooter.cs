using Photon.Pun;
using UnityEngine;

public class Shooter : MonoBehaviourPun
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("Shoot", RpcTarget.All);
        }
    }

    [PunRPC]
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().linearVelocity = transform.forward * 10f;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.shooterActorNumber = photonView.Owner.ActorNumber;

        Destroy(bullet, 3f);
    }
}
