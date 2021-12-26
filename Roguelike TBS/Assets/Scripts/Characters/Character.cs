using System;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour, IModifierProvider {
    public static Character instance = null;

    [Header("Level Up Bonus Variables")]
    [SerializeField] protected LevelUpBonuses levelUpBonuses = null;
    [SerializeField] protected GameObject abilityParticles = null;

    [Header("Level Up UI")]
    [SerializeField] protected LevelUpBonusMenu levelUpMenu = null;
    [SerializeField] protected Sprite activeAbilityIcon = null;

    protected List<int> choiceIndexes = new List<int>();
    protected BaseStats baseStats;
    protected RoomManager currentRoom = null;
    
    protected ClassAbility[] randomBonuses = new ClassAbility[3];
    protected List<ClassAbility> selectedAbilities = new List<ClassAbility>();
    protected int levelOfBonuses = 0;

    // Passive Stat increases
    protected Dictionary<Stat, int> passiveModifyAdditions = new Dictionary<Stat, int>();
    protected Dictionary<Stat, int> passiveModifyPercentages = new Dictionary<Stat, int>();

    // Active Ability variables
    protected Dictionary<Stat, int> activeAbilityModifyPercentages = new Dictionary<Stat, int>();
    protected float cooldownTimer = 0;
    protected GameObject characterAbilityIcon;
    protected AbilityState abilityState = AbilityState.Ready;

    public event Action<GameObject> onRoomClear;
    public virtual event Action onFeatAdded;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        baseStats = GetComponent<BaseStats>();
        if (baseStats != null) {
            baseStats.onLevelUp += ChooseLevelUpModifier;
        }
        characterAbilityIcon = GameObject.Find("Character Ability Icon");

        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            passiveModifyPercentages[stat] = 0;
            passiveModifyAdditions[stat] = 0;
        }
    }

    private void Start() {
        if (activeAbilityIcon) {
                var weaponAbilityIcon = GameObject.Find("Character Ability Icon");
                weaponAbilityIcon.GetComponent<Image>().sprite = activeAbilityIcon;
                weaponAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        }
    }

    public AbilityState GetAbilityState() { return abilityState; }

    public virtual void ActiveAbility() {
        // Overrided in the specific character classes
    }

    public virtual void CallOnAbilityKill() {
        // Overrided in the specific character classes
    }

    public void ChooseLevelUpModifier() {
        if (baseStats.GetLevel() % 2 == 1) { return; }

        levelOfBonuses = (baseStats.GetLevel() / 2) - 1;

        randomBonuses = levelUpBonuses.GetLevelUpBonusesByLevel(levelOfBonuses);
        
        Time.timeScale = 0;

        levelUpMenu.ToggleBodyActive();
        do {
            var index = UnityEngine.Random.Range(0, randomBonuses.Length);
            if (!choiceIndexes.Contains(index)) {
                choiceIndexes.Add(index);
            }
            levelUpMenu.SetAbilityDescription(choiceIndexes.Count-1, randomBonuses[index].GetAbilityDescription());
        } while (choiceIndexes.Count < 3);
    }

    public void ChooseBonusButton(int index) {
        HandleSelectedClassAbility(randomBonuses[choiceIndexes[index]]);
        levelUpMenu.ToggleBodyActive();
        Time.timeScale = 1;
        choiceIndexes = new List<int>();
    }

    protected abstract void HandleSelectedClassAbility(ClassAbility ability);

    public IEnumerable<int> GetAdditiveModifiers(Stat stat) {
        yield return passiveModifyAdditions[stat];
    }

    public virtual IEnumerable<int> GetMultiplicativeModifiers(Stat stat) {
        // Overrided in character class
        yield return 0;
    }

    public void ModifyAbilityBonusPercentage(Stat stat, int percent) {
        activeAbilityModifyPercentages[stat] += percent;
    }

    public void ModifyPassiveBonusAddition(Stat stat, int increase) {
        passiveModifyAdditions[stat] += increase;
    }

    public void ModifyPassiveBonusPercentage(Stat stat, int percent) {
        passiveModifyPercentages[stat] += percent;
    }

    public void SetCurrentRoom(RoomManager roomManager) { currentRoom = roomManager; }
    
    public RoomManager GetCurrentRoom() { return currentRoom; }
    public List<ClassAbility> GetSelectedAbilities() { return selectedAbilities; }

    public void TriggerOnRoomClear() {
        if (onRoomClear != null) { onRoomClear(gameObject); }
    }
}
