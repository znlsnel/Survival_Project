public enum PlayerSoundType { Attack, Damaged, Landed }

namespace Player
{
    public class Audio: Actor.Audio<PlayerSoundType> { }
}
