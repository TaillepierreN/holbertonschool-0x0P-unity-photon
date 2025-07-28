using System.Collections;
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
        if (nameText != null && photonView.Owner.NickName != null)
            nameText.text = photonView.Owner.NickName;
    }

    [PunRPC]
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{photonView.Owner.NickName} took {amount} damage, HP now {currentHealth}");

        if (healthBar != null)
        {
            StartCoroutine(UpdateHealthBar(currentHealth));
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator UpdateHealthBar(float target)
    {
        float start = healthBar.value;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            healthBar.value = Mathf.Lerp(start, target, t);
            yield return null;
        }
	}

	private void Die()
    {
        if (photonView.IsMine)
        {
            Debug.Log("You died. Leaving room...");
            PhotonNetwork.LeaveRoom();
        }
    }
}
