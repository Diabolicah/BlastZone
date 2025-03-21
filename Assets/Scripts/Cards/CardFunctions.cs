using UnityEngine;

public class CardFunctions : MonoBehaviour
{
    public void ActivateCard(CardConfig card, StatsManager statsManager, TankWeapons tankWeapons)
    {
        switch (card.GetTitle())
        {
            case "Health":
                ActivateHealthCard(card, statsManager);
                break;
            case "Movement Speed":
                ActivateMovementSpeedCard(card, statsManager);
                break;
            case "Damage":
                ActivateDamageCard(card, statsManager);
                break;
            case "Attack Speed":
                ActivateAttackSpeedCard(card, statsManager);
                break;
            case "Bullet Speed":
                ActivateBulletSpeedCard(card, statsManager);
                break;
            case "Health Regen":
                ActivateHealthRegenCard(card, statsManager);
                break;


            case "Bullet Weapon":
                ActivateBulletWeaponCard(card, tankWeapons);
                break;
            case "Triple Bullet Weapon":
                ActivateTripleBulletWeaponCard(card, tankWeapons);
                break;

            case "Fire Bullet Weapon":
                ActivateFireBulletWeaponCard(card, tankWeapons);
                break;
            case "Triple Fire Bullet Weapon":
                ActivateTripleFireBulletWeaponCard(card, tankWeapons);
                break;

            case "Ice Bullet Weapon":
                ActivateIceBulletWeaponCard(card, tankWeapons);
                break;
            case "Triple Ice Bullet Weapon":
                ActivateTripleIceBulletWeaponCard(card, tankWeapons);
                break;

            case "Wind Bullet Weapon":
                ActivateWindBulletWeaponCard(card, tankWeapons);
                break;
            case "Triple Wind Bullet Weapon":
                ActivateTripleWindBulletWeaponCard(card, tankWeapons);
                break;
        }
    }

    public void ActivateHealthCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.Health += 0.1f;
        statsManager.UpdateStats(currentStats);
    }

    public void ActivateMovementSpeedCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.MovementSpeed += 0.1f;
        statsManager.UpdateStats(currentStats);
    }

    public void ActivateDamageCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.Damage += 0.1f;
        statsManager.UpdateStats(currentStats);
    }

    public void ActivateAttackSpeedCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.AttackSpeed += 0.1f;
        statsManager.UpdateStats(currentStats);
    }

    public void ActivateBulletSpeedCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.BulletSpeed += 0.1f;
        statsManager.UpdateStats(currentStats);
    }

    public void ActivateHealthRegenCard(CardConfig card, StatsManager statsManager)
    {
        PlayerStatsStruct currentStats = statsManager.Stats;
        currentStats.HealthRegen += 0.1f;
        statsManager.UpdateStats(currentStats);
    }

    public void ActivateBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon bulletWeapon = new WeaponBullet(card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(bulletWeapon);
    }

    public void ActivateTripleBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon tripleBulletWeapon = new WeaponTripleBullet((TripleBulletWeaponConfig) card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(tripleBulletWeapon);
    }

    public void ActivateFireBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon bulletWeapon = new WeaponBullet(card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(bulletWeapon);
    }

    public void ActivateTripleFireBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon tripleBulletWeapon = new WeaponTripleBullet((TripleBulletWeaponConfig)card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(tripleBulletWeapon);
    }

    public void ActivateIceBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon bulletWeapon = new WeaponBullet(card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(bulletWeapon);
    }

    public void ActivateTripleIceBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon tripleBulletWeapon = new WeaponTripleBullet((TripleBulletWeaponConfig)card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(tripleBulletWeapon);
    }

    public void ActivateWindBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon bulletWeapon = new WeaponBullet(card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(bulletWeapon);
    }

    public void ActivateTripleWindBulletWeaponCard(CardConfig card, TankWeapons tankWeapons)
    {
        IWeapon tripleBulletWeapon = new WeaponTripleBullet((TripleBulletWeaponConfig)card.GetWeaponConfig(), tankWeapons._shootPoint);
        tankWeapons.WeaponIcon.sprite = card.GetImage();
        tankWeapons.setTankWeapon(tripleBulletWeapon);
    }
}
