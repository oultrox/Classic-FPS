using System.Collections;
using System.Collections.Generic;
using DumbInjector;
using UnityEngine;

public class PlayerInstaller : MonoBehaviour, IDependencyProvider
{
    [SerializeField] PlayerHealth playerHealth;

    [Provide]
    public  IHasHealth ProvidePlayerHealth()
    {
        return playerHealth;
    }
}
