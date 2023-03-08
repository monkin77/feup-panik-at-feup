public static class Utils {
    // GameObject Tags
    public static string BAKER_TAG = "Baker";
    public static string BULLET_TAG = "Bullet";
    public static string ENEMY_TAG = "Enemy";
    public static string OBSTACLE_TAG = "Obstacle";
    public static string WALL_TAG = "Wall";
    public static string BAKER_UP_ANIMATION = "WalkUp";
    public static string BAKER_DOWN_ANIMATION = "WalkDown";
    public static string BAKER_WALK_HORIZONTAL = "WalkHorizontal";

    public static string createAmmoText(int ammoCount) {
        return $"x{ammoCount}";
    }

    public static string createScoreText(int score) {
        return $"Score: {score}";
    }
}   