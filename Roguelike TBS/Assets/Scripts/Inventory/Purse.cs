using UnityEngine;

public class Purse : MonoBehaviour {
    [SerializeField] int startingBalance = 0;

    int balance = 0;

    private void Awake() {
        balance = startingBalance;
    }

    public int GetBalance() { return balance; }

    public void UpdateBalance(int amount) {
        balance += amount;
    }
}
