using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public float damage;
    public float fireRate;
    public int totalBullets;
    public float reloadTime;
    public int cartidgeSize;
    public GunType gunType;

}

public enum GunType
{
    Automatic,
    SemiAutomatic,
}
