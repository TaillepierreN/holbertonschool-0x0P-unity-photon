using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;
    public TMP_Text nameText;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        if (photonView.Owner.NickName != null)
            nameText.text = photonView.Owner.NickName;
    }

    [PunRPC]
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (photonView.IsMine && healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }
}
