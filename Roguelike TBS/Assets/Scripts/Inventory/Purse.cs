using UnityEngine;

public class Purse : MonoBehaviour {
    [SerializeField] int startingBalance = 0;

    private int balance = 0;
    private float moneyMultiplier = 1f;

    private void Awake() {
        balance = startingBalance;
    }

    public int GetBalance() { return balance; }

    public void SetMoneyMultiplier(float multiplier) { moneyMultiplier = multiplier; }

    public void UpdateBalance(int amount) {
        if (Mathf.Sign(amount) == -1) {
            balance += amount;
        } else {
            balance += Mathf.RoundToInt(amount * moneyMultiplier);
        }
    }
}
