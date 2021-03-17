// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UniRx.Async;
using UnityEngine;

namespace Naninovel
{
    /// <summary>
    /// A <see cref="ICharacterActor"/> implementation using <see cref="GenericCharacterBehaviour"/> to represent the actor.
    /// </summary>
    /// <remarks>
    /// Resource prefab should have a <see cref="GenericCharacterBehaviour"/> component attached to the root object.
    /// Appearance and other property changes are routed via the events of <see cref="GenericCharacterBehaviour"/> component.
    /// </remarks>
    [ActorResources(typeof(GenericCharacterBehaviour), false)]
    public class GenericCharacter : GenericActor<GenericCharacterBehaviour, CharacterMetadata>, ICharacterActor, Commands.LipSync.IReceiver
    {
        public CharacterLookDirection LookDirection { get => lookDirection; set => SetLookDirection(value); }

        private CharacterLipSyncer lipSyncer;
        private CharacterLookDirection lookDirection;

        public GenericCharacter (string id, CharacterMetadata metadata)
            : base(id, metadata) { }

        public override async UniTask InitializeAsync ()
        {
            await base.InitializeAsync();

            lipSyncer = new CharacterLipSyncer(Id, Behaviour.NotifyIsSpeakingChanged);
        }
        
        public override void Dispose ()
        {
            base.Dispose();

            lipSyncer?.Dispose();
        }

        public async UniTask ChangeLookDirectionAsync (CharacterLookDirection lookDirection, float duration, 
            EasingType easingType = default, CancellationToken cancellationToken = default)
        {
            this.lookDirection = lookDirection;

            if (Behaviour.TransformByLookDirection)
            {
                var rotation = LookDirectionToRotation(lookDirection);
                await ChangeRotationAsync(rotation, duration, easingType, cancellationToken);
            }
        }

        public void AllowLipSync (bool active) => lipSyncer.SyncAllowed = active;

        protected virtual void SetLookDirection (CharacterLookDirection lookDirection)
        {
            this.lookDirection = lookDirection;

            Behaviour.NotifyLookDirectionChanged(lookDirection);

            if (Behaviour.TransformByLookDirection)
            {
                var rotation = LookDirectionToRotation(lookDirection);
                SetBehaviourRotation(rotation);
            }
        }

        protected virtual Quaternion LookDirectionToRotation (CharacterLookDirection lookDirection)
        {
            var yAngle = 0f;
            switch (lookDirection)
            {
                case CharacterLookDirection.Center:
                    yAngle = 0;
                    break;
                case CharacterLookDirection.Left:
                    yAngle = Behaviour.LookDeltaAngle;
                    break;
                case CharacterLookDirection.Right:
                    yAngle = -Behaviour.LookDeltaAngle;
                    break;
            }

            var currentRotation = Rotation.eulerAngles;
            return Quaternion.Euler(currentRotation.x, yAngle, currentRotation.z);
        }
    }
}
