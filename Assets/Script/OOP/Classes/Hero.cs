using UnityEngine;

[System.Serializable]
public class Hero
{
    [SerializeField] private string name;
    [SerializeField] private int hp;
    [SerializeField] private Stats baseStats;
    [SerializeField] private ELEMENT resistance;
    [SerializeField] private ELEMENT weakness;
    [SerializeField] private Weapon weapon;

    //Usata per i controlli allo Start
    private bool isStart = true;   

    //Costruttore di Hero che assegna tutti i valori
    public Hero(string name, int hp, Stats baseStats, ELEMENT resistence, ELEMENT weakness, Weapon weapon)
    {
        SetName(name);
        SetHp(hp);
        SetHeroStats(baseStats);
        SetResistence(resistence);
        SetWeakness(weakness);
        SetWeapon(weapon);
    }

    public string GetName() => name;
    private void SetName(string name) => this.name = (string.IsNullOrWhiteSpace(name)) ? "VoidHero" : name;

    public int GetHp() => hp;
    private void SetHp(int hp)
    {
        //Eseguito solo una volta al controllo in Start nel caso venga usato nell'Inspector o nel costruttore
        if (isStart)
        {
            if (hp < 0)
            {
                this.hp = Mathf.Abs(hp);
            }
            else if (hp == 0)
            {
                this.hp = Random.Range(50, 101);
            }
            else
            {
                this.hp = hp;
            }
        }
        //Eseguito tutte le altre volte
        else
        {
            this.hp = Mathf.Max(0, hp);
        }
    }

    public Stats GetHeroStats() => baseStats;
    private void SetHeroStats(Stats stats) => baseStats = stats;

    public ELEMENT GetResistance() => resistance;
    private void SetResistence(ELEMENT resistence) => this.resistance = resistence;

    public ELEMENT GetWeakness() => weakness;
    private void SetWeakness(ELEMENT weakness) => this.weakness = weakness;

    public Weapon GetWeapon() => weapon;
    private void SetWeapon(Weapon weapon) => this.weapon = weapon;

    public void AddHp(int amount) => SetHp(hp + amount);

    public void TakeDamage(int damage) => AddHp(-damage);

    public bool IsAlive() => hp > 0;

    //Funzione per la consistenza dei valori
    public void ControlValue()
    {
        isStart = true;
        SetName(name);
        SetHp(hp);
        isStart = false;
        baseStats = Stats.ControlValue(baseStats);

        weapon.ControlValue();
    }
}
