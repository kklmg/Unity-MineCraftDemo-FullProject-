using UnityEngine;

using Assets.Scripts.NEvent;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Communicator))]
    class ChaSuspend : EventMono
    {
        protected override void AAwake()
        {
        }

        private void OnEnable()
        {
            this.SubscribeEvent(E_Cha_Suspend.ID, HandleSuspend, enPriority.level_2);
        }

        protected override void AOnDisable()
        {
        }

        private void HandleSuspend(IEvent _event)
        {
            E_Cha_Suspend ESuspend = _event.Cast<E_Cha_Suspend>();
            this.PublishEvent(new E_Cha_TranslateRequest_Y(ESuspend.Force * 10 * Time.deltaTime));
        }
    }
}


