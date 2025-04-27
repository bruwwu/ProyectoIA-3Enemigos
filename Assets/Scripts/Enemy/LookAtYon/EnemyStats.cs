[System.Serializable]
public class EnemyStats
{
    public float maxHP;
    public float bulletSpeed;
    public float rotationSpeed;
    public float rotationAngle;
    public float coneDistance;
    public float difficultyValue;

    public EnemyStats() {}

    public EnemyStats(EnemyStats other)
    {
        maxHP = other.maxHP;
        bulletSpeed = other.bulletSpeed;
        rotationSpeed = other.rotationSpeed;
        rotationAngle = other.rotationAngle;
        coneDistance = other.coneDistance;
        difficultyValue = other.difficultyValue;
    }

    public string PrintStats()
    {
        return $"HP: {maxHP}, BulletSpeed: {bulletSpeed}, RotationSpeed: {rotationSpeed}, RotationAngle: {rotationAngle}, ConeDistance: {coneDistance}";
    }
}