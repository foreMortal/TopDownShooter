using UnityEngine;

public class PlayerBonus : BonusParent
{
    [SerializeField] private float time = 10f;
    private float timer;
    public float Time {  get { return time; } }

    public void SelfUpdate(float deltaTime)
    {
        timer += deltaTime;
        if(timer >= time)
        {
            RemoveBonus(playerManager);
            timer = 0f;
        }
    }
}
