using LuckiusDev.Utils;
using UnityEngine;

public class VFXBank : PersistentSingleton<VFXBank>
{
    [Header("Explosions")]
    [SerializeField] private GameObject _bombExplosionVFX;
    [SerializeField] private GameObject _dynamiteExplosionVFX;
    [SerializeField] private GameObject _grenadeExplosionVFX;

    [Header("Impactes")]
    [SerializeField] private GameObject _arrowImpacteVFX;
    [SerializeField] private GameObject _armoredArrowImpacteVFX;
    [SerializeField] private GameObject _spearImpacteVFX;
    [SerializeField] private GameObject _shotImpacteVFX;
    [SerializeField] private GameObject _RailImpacteVFX;
    
    [Header("shots")]
    [SerializeField] private GameObject _shootFireVFX;
    [SerializeField] private GameObject _shootRailVFX;
    [SerializeField] private GameObject _shootJetVFX;
    [SerializeField] private GameObject _shootArrowVFX;


    public static GameObject BombExplo => Instance._bombExplosionVFX ;
    public static GameObject DynamiteExplo => Instance._dynamiteExplosionVFX ;
    public static GameObject GrenadeExplo => Instance._grenadeExplosionVFX ;
    
    public static GameObject ArrowImp => Instance._arrowImpacteVFX ;
    public static GameObject ArmoredArrowImp => Instance._armoredArrowImpacteVFX ;
    public static GameObject SpearImp => Instance._spearImpacteVFX ;
    public static GameObject ShotImp => Instance._shotImpacteVFX ;
    public static GameObject RailImp => Instance._RailImpacteVFX ;
    
    public static GameObject ShootFire => Instance._shootFireVFX ;
    public static GameObject ShootRail => Instance._shootRailVFX ;
    public static GameObject ShootJet => Instance._shootJetVFX ;
    public static GameObject ShootArrow => Instance._shootArrowVFX ;
}
