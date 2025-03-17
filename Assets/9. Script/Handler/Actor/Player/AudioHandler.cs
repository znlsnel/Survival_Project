public enum PlayerSoundType { Attack, Damaged, Landed,
    Jump
}

namespace Player
{
    public class AudioHandler: Actor.Audio<PlayerSoundType> { }
}
