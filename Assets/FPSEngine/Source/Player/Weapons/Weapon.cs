using System.Collections;
using DumbInjector;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    [Header("Ammunition Editor friendly values")]
    [SerializeField] private int ammoAmount = 200;
    [SerializeField] private int ammoClipSize = 12;
    [SerializeField] private float reloadTime = 1;
    
    [Header("Damage Editor friendly values")]
    [SerializeField] private int damage = 20;

    [Header("Shake Editor friendly values")]
    [SerializeField] private float shakeDuration = 0.08f;
    [SerializeField] private float shakeMagnitude = 4;

    [SerializeField] private int ammoLeft;
    [SerializeField] private int ammoClipLeft;
    [Inject] protected CameraShaker _cameraShaker;
    [Inject] protected WeaponShake _weaponShaker;
    [Inject] protected DynamicCrosshair _crosshair;
    
    private bool isReloading = false;
    
    

    public int AmmoAmount { get => ammoAmount; set => ammoAmount = value; }
    public int AmmoClipSize { get => ammoClipSize; set => ammoClipSize = value; }
    public int Damage { get => damage; set => damage = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }

    public float ShakeDuration { get => shakeDuration; set => shakeDuration = value; }
    public float ShakeMagnitude { get => shakeMagnitude; set => shakeMagnitude = value; }
    public int AmmoLeft { get => ammoLeft; set => ammoLeft = value; }
    public int AmmoClipLeft { get => ammoClipLeft; set => ammoClipLeft = value; }
    public bool IsReloading { get => isReloading; set => isReloading = value; }
}