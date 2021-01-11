namespace ProceduralDungeon
{
    public interface IDegradable
    {
        int Duration {get;}
        void DecrementDuration();
    }
}