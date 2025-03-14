using System;
using System.Collections.Generic;
using UnityEngine;

public enum ChomperSoundType { Attack, Damaged }
public class ChomperAudioHandler: AudioHandler<ChomperSoundType> { }