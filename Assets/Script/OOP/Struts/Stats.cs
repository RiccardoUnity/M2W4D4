using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int atk;
    public int def;
    public int res;
    public int spd;
    public int crt;
    public int aim;
    public int eva;

    //Costruttore di Stats che assegna tutti i valori
    public Stats (int atk, int def, int res, int spd, int crt, int aim, int eva)
    {
        this.atk = SetValue(atk, 5, 11);
        this.def = SetValue(def, 2, 6);
        this.res = SetValue(res, 2, 6);
        this.spd = SetValue(spd, 0, 101);
        this.crt = SetValue(crt, 0, 101);
        this.aim = SetValue(aim, 50, 81);
        this.eva = SetValue(eva, 0, 21);
    }

    public static Stats Sum(Stats statsHero, Stats statsWeapon)
    {
        return new Stats
        (
            statsHero.atk + statsWeapon.atk, 
            statsHero.def + statsWeapon.def,
            statsHero.res + statsWeapon.res,
            statsHero.spd + statsWeapon.spd,
            statsHero.crt + statsWeapon.crt,
            statsHero.aim + statsWeapon.aim,
            statsHero.eva + statsWeapon.eva
        );
    }

    //Da usare quando si inizializza fuori dell'Ispector
    private static int SetValue(int value, int min, int max) => (value < 0) ? Random.Range(min, max) : value;

    //Da usare quando si vuole convalidare i dati che vengono dall'Inspector
    public static Stats ControlValue(Stats stats)
    {
        return new Stats
        (
            stats.atk = Mathf.Abs(stats.atk),
            stats.def = Mathf.Abs(stats.def),
            stats.res = Mathf.Abs(stats.res),
            stats.spd = Mathf.Abs(stats.spd),
            stats.crt = Mathf.Abs(stats.crt),
            stats.aim = Mathf.Abs(stats.aim),
            stats.eva = Mathf.Abs(stats.eva)
        );
    }
}
