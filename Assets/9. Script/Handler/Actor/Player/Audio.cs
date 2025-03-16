public enum PlayerSoundType { Attack, Damaged, Landed,
    Jump
}

namespace Player
{
    public class Audio: Actor.Audio<PlayerSoundType> { }
}
