using UnityEngine;

[System.Serializable]
public class Weapon
{
    public enum DAMAGE_TYPE
    {
        PHYSICAL = 0,
        MAGICAL = 1
    }

    [SerializeField] private string name;
    [SerializeField] private DAMAGE_TYPE dmgType;
    [SerializeField] private ELEMENT elem;
    [SerializeField] private Stats bonusStats;

    //Costruttore di Weapon che assegna tutti i valori
    public Weapon(string name, DAMAGE_TYPE dmgType, ELEMENT elem, Stats bonusStats)
    {
        SetName(name);
        SetDmgType(dmgType);
        SetElem(elem);
        SetWeaponStats(bonusStats);
    }

    //Per ogni variabile setto un Getter pubblico per passare il dato, un Setter privato per usarlo solo nel costruttore
    public string GetName() => name;
    private void SetName(string name) => this.name = (string.IsNullOrWhiteSpace(name)) ? "VoidWeapon" : name;

    public DAMAGE_TYPE GetDmgType() => dmgType;
    private void SetDmgType(DAMAGE_TYPE dmgType) => this.dmgType = dmgType;

    public ELEMENT GetElem() => elem;
    private void SetElem(ELEMENT elem) => this.elem = elem;

    public Stats GetWeaponStats() => bonusStats;
    private void SetWeaponStats(Stats bonusStats) => this.bonusStats = bonusStats;

    //Funzione per la consistenza dei valori
    public void ControlValue()
    {
        SetName(name);
        bonusStats = Stats.ControlValue(bonusStats);
    }
}
