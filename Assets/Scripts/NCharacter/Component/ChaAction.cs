using UnityEngine;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.WorldSearcher;
using Assets.Scripts.NWorld;
using Assets.Scripts.NEvent;

namespace Assets.Scripts.NCharacter
{
    [RequireComponent(typeof(Communicator))]
    class ChaAction : MonoBehaviour
    {
        Communicator m_Communicator;

        private void Awake()
        {
            m_Communicator = GetComponent<Communicator>();
        }

        private void OnEnable()
        {
            m_Communicator.SubsribeEvent(E_Cha_TranslateRequest_XZ.ID, HandleMovement_XZ,enPriority.Lowest);
            m_Communicator.SubsribeEvent(E_Cha_YawRequest.ID, HandleYaw, enPriority.Lowest);
            m_Communicator.SubsribeEvent(E_Cha_RotateRequest.ID, HandleRotation, enPriority.Lowest);
            m_Communicator.SubsribeEvent(E_Cha_TranslateRequest_Y.ID, HandleMovement_Y, enPriority.Lowest);
            m_Communicator.SubsribeEvent(E_Cha_TranslateOrder.ID, HandleTranslationOrder, enPriority.Lowest);
        }

        private void OnDisable()
        {
            m_Communicator.UnSubscribe(E_Cha_TranslateRequest_XZ.ID, HandleMovement_XZ);
            m_Communicator.UnSubscribe(E_Cha_YawRequest.ID, HandleYaw);
            m_Communicator.UnSubscribe(E_Cha_RotateRequest.ID, HandleRotation);
            m_Communicator.UnSubscribe(E_Cha_TranslateRequest_Y.ID, HandleMovement_Y);
            m_Communicator.UnSubscribe(E_Cha_TranslateOrder.ID, HandleTranslationOrder);
        }
        void HandleMovement_XZ(IEvent _event)
        {
            Vector3 trans = (_event as E_Cha_TranslateRequest_XZ).Translation;
            transform.position += trans;

            m_Communicator.Publish(new E_Cha_HasMoved());
        }

        void HandleMovement_Y(IEvent _event)
        {
            float Y = _event.Cast<E_Cha_TranslateRequest_Y>().Velocity;
            transform.position =
                new Vector3(transform.position.x, transform.position.y + Y, transform.position.z);
        }

        void HandleYaw(IEvent _event)
        {
            float Y = (_event as E_Cha_YawRequest).Value;
            transform.Rotate(Vector3.up,Y);
        }

        void HandleRotation(IEvent _event)
        {
            Vector3 rotation = (_event as E_Cha_RotateRequest).Rotation;
            transform.Rotate(rotation);
        }

        void HandleTranslationOrder(IEvent _event)
        {
            transform.position += _event.Cast<E_Cha_TranslateOrder>().Translation;

            m_Communicator.Publish(new E_Cha_HasMoved());
        }
    }
}
