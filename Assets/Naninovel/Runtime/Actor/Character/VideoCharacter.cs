// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UniRx.Async;
using UnityEngine.Video;

namespace Naninovel
{
    /// <summary>
    /// A <see cref="ICharacterActor"/> implementation using <see cref="VideoClip"/> to represent the actor.
    /// </summary>
    [ActorResources(typeof(VideoClip), true)]
    public class VideoCharacter : VideoActor<CharacterMetadata>, ICharacterActor
    {
        public CharacterLookDirection LookDirection
        {
            get => TransitionalRenderer.GetLookDirection(ActorMetadata.BakedLookDirection);
            set => TransitionalRenderer.SetLookDirection(value, ActorMetadata.BakedLookDirection);
        }

        public VideoCharacter (string id, CharacterMetadata metadata)
            : base(id, metadata) { }

        public UniTask ChangeLookDirectionAsync (CharacterLookDirection lookDirection, float duration,
            EasingType easingType = default, CancellationToken cancellationToken = default)
        {
            return TransitionalRenderer.ChangeLookDirectionAsync(lookDirection,
                ActorMetadata.BakedLookDirection, duration, easingType, cancellationToken);
        }
    }
}
