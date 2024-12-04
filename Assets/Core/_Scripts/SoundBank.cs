using LuckiusDev.Utils;
using UnityEngine;

public class SoundBank : PersistentSingleton<SoundBank>
{
    [Header("ItemSFX")]
    [SerializeField] private AudioClip _rockSFX;
    [SerializeField] private AudioClip _spearSFX;
    [SerializeField] private AudioClip _bowSFX;
    [SerializeField] private AudioClip _crossbowSFX;
    [SerializeField] private AudioClip _armoredCrossbowSFX;
    [SerializeField] private AudioClip _bombSFX;
    [SerializeField] private AudioClip _flintlockSFX;
    [SerializeField] private AudioClip _blunderbussSFX;
    [SerializeField] private AudioClip _dynamiteSFX;
    [SerializeField] private AudioClip _revolverSFX;
    [SerializeField] private AudioClip _rifleSFX;
    [SerializeField] private AudioClip _grenadeSFX;
    [SerializeField] private AudioClip _gunSFX;
    [SerializeField] private AudioClip _smgSFX;
    [SerializeField] private AudioClip _assaultRifleSFX;
    [SerializeField] private AudioClip _sniperRifleSFX;
    [SerializeField] private AudioClip _railgunSFX;

    [SerializeField] private AudioClip _projectileHitSFX;

    [Header("UiSFX")]
    [SerializeField] private AudioClip _menuButonsSFX;
    [SerializeField] private AudioClip _buyingItemsSFX;
    [SerializeField] private AudioClip _mergingSFX;
    [SerializeField] private AudioClip _upgradeSFX;

    [Header("EnemySFX")]
    [SerializeField] private AudioClip _waveIncomingSFX;
    [SerializeField] private AudioClip _mobDeathSFX;
    [SerializeField] private AudioClip _coinSFX;

    [Header("WallSFX")]
    [SerializeField] private AudioClip _wallHitSFX;
    [SerializeField] private AudioClip _wallColapsingSFX;

    [Header("Music")]
    [SerializeField] private AudioClip _Ingame;

    public static AudioClip RockSFX => Instance._rockSFX;
    public static AudioClip SpearSFX => Instance._spearSFX;
    public static AudioClip BowSFX => Instance._bowSFX;
    public static AudioClip CrossbowSFX => Instance._crossbowSFX;
    public static AudioClip ArmoredCrossbowSFX => Instance._armoredCrossbowSFX;
    public static AudioClip BombSFX => Instance._bombSFX;
    public static AudioClip FlintlockSFX => Instance._flintlockSFX;
    public static AudioClip BlunderbussSFX => Instance._blunderbussSFX;
    public static AudioClip DynamiteSFX => Instance._dynamiteSFX;
    public static AudioClip RevolverSFX => Instance._revolverSFX;
    public static AudioClip RifleSFX => Instance._rifleSFX;
    public static AudioClip GrenadeSFX => Instance._grenadeSFX;
    public static AudioClip GunSFX => Instance._gunSFX;
    public static AudioClip SmgSFX => Instance._smgSFX;
    public static AudioClip AssaultRifleSFX => Instance._assaultRifleSFX;
    public static AudioClip SniperRifleSFX => Instance._sniperRifleSFX;
    public static AudioClip RailgunSFX => Instance._railgunSFX;
    public static AudioClip ProjectilHitSFX => Instance._projectileHitSFX;
    public static AudioClip MenuButonsSFX => Instance._menuButonsSFX;
    public static AudioClip BuyingSFX => Instance._buyingItemsSFX;
    public static AudioClip MergingSFX => Instance._mergingSFX;
    public static AudioClip UpgradeSFX => Instance._upgradeSFX;
    public static AudioClip WaveSFX => Instance._waveIncomingSFX;
    public static AudioClip MobDeathSFX => Instance._mobDeathSFX;
    public static AudioClip CoinSFX => Instance._coinSFX;
    public static AudioClip WallHitSFX => Instance._wallHitSFX;
    public static AudioClip WallColapsingSFX => Instance._wallColapsingSFX;
    public static AudioClip InGame => Instance._Ingame;
}
