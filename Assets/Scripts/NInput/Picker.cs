using UnityEngine;

using Assets.Scripts.NCharacter;
using Assets.Scripts.NWorld;
using Assets.Scripts.NInput;
using Assets.Scripts.NEvent;
using Assets.Scripts.NData;
using Assets.Scripts.NGameSystem;
using Assets.Scripts.NGlobal.ServiceLocator;
using Assets.Scripts.NGlobal.Singleton;


namespace Assets.Scripts.NUI
{
    public enum EnPickerMode {eNone, eSet, ePut, eDestroy }

    public class Picker : MonoBehaviour
    {
        public GameObject Prefab_PutCursor;
        public GameObject Prefab_PickCursor;

        private GameObject Ins_PutCursor;
        private GameObject Ins_PickCursor;

        [SerializeField]
        [Range(6,32)]
        private int m_PickingDistance = 16;

        [SerializeField]
        [Range(0.25f, 1.0f)]
        private float m_RayCheckInterval = 1f;

        private bool m_bIsPickValid = false;
        private bool m_bIsPutValid = false;

        [SerializeField]
        private BlockLocation m_PickLoc;  //the location of block is picked

        [SerializeField]
        private BlockLocation m_PutLoc;  //the location that block will be putted

        Ray m_Ray;

        public EnPickerMode PickerMode { set; get; }
        public IBlock SelectedBlock { set; get; }
        public void SetPickingDistance(int Distance)
        {
            if (Distance < 6)
            {
                m_PickingDistance = 6;
            }
            else if (Distance > 32)
            {
                m_PickingDistance = 32;
            }
            else
            {
                m_PickingDistance = Distance;
            }
        }

        private IWorld m_World;
        private IController m_Control;
        private ChaPlayer m_Player;


        private void Awake()
        {
            //Cursor.lockState = CursorLockMode.Locked;

            Ins_PutCursor = Instantiate(Prefab_PutCursor);
            Ins_PickCursor = Instantiate(Prefab_PickCursor);

            PickerMode = EnPickerMode.ePut;
            SelectedBlock = null;
        }
        private void Start()
        {
            m_World = Locator<IWorld>.GetService();
            m_Control = Locator<IController>.GetService();
            m_Player = MonoSingleton<GameSystem>.Instance.
                PlayerMngIns.PlayerIns.GetComponent<ChaPlayer>();
        }

        private void Update()
        {
            if (PickerMode == EnPickerMode.eNone) return;
            //if (m_bHasPlayerSpawned == false) return;

            //check weather mouse has moved
            if (m_Control.HasCursorMoved())
            {
                Check();
            }

            if (m_Control.CursorDown())
            {
                switch (PickerMode)
                {
                    case EnPickerMode.eSet:
                        {
                            if (SelectedBlock != null)
                                _SetBlock(SelectedBlock);
                        }
                        break;
                    case EnPickerMode.ePut:
                        _PutBlock(SelectedBlock);
                        break;
                    case EnPickerMode.eDestroy:
                        _SetBlock(null);
                        break;
                    default:
                        break;
                }
            }

            if (m_Control.Back())
            {
                UndoBlockModify();
            }
        }

        public void UndoBlockModify()
        {
            Locator<IEventHelper>.GetService().Publish(new E_Block_Recover());
        }

        public void Check()
        {
            _FigureOutPickPutLocation();

            if (m_bIsPickValid && (PickerMode == EnPickerMode.eSet || PickerMode == EnPickerMode.eDestroy))
            {
                Ins_PickCursor.SetActive(true);
                Ins_PickCursor.transform.position = m_PickLoc.Bound.center;
            }
            else
            {
                Ins_PickCursor.SetActive(false);
            }

            if (m_bIsPutValid && PickerMode == EnPickerMode.ePut)
            {
                Ins_PutCursor.SetActive(true);
                Ins_PutCursor.transform.position = m_PutLoc.Bound.center;
            }
            else
            {
                Ins_PutCursor.SetActive(false);
            }

        }

        private void _SetBlock(IBlock block)
        {
            if (m_bIsPickValid)
            {
                if (block == null)
                {
                    //publish event : set block  request
                    Locator<IEventHelper>.GetService().Publish(new E_Block_Modify(ref m_PickLoc, 0));

                    //Debug.Log("pb des");
                }
                else
                {
                    //publish event : set block  request
                    Locator<IEventHelper>.GetService().Publish(new E_Block_Modify(ref m_PickLoc, block.ID));

                    //Debug.Log("pbs set");
                }
                
            }
        }
        private void _PutBlock(IBlock block)
        {
            //publish event : put block  request
            if (m_bIsPutValid && block!=null)
            {
                Locator<IEventHelper>.GetService().Publish(new E_Block_Modify(ref m_PutLoc, block.ID));
                m_bIsPutValid = false;
                Check();
            }
        }
        private void _DestroyBlock()
        {
            _SetBlock(null);
        }


        private void _FigureOutPickPutLocation()
        {
            m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            _CheckIfPickIsValid();

            if (m_bIsPickValid)
            {
                if (_CheckRayHitSide(out Vector3Int HitSide))
                {
                    m_PutLoc = m_PickLoc.Offset_Blk(HitSide, m_World);

                    m_bIsPutValid = m_PutLoc.IsPutable(m_World) && !m_Player.IntersectWithBound(m_PickLoc.Bound);
                }
                else m_bIsPutValid = false;
            }
            else m_bIsPutValid = false;

        }

        //Check if Block had been Picked
        private void _CheckIfPickIsValid()
        {
            float distance = m_PickingDistance;
            Vector3 CheckLoc = m_Ray.origin;

            do
            {
                //Get Cur location
                CheckLoc += m_Ray.direction * m_RayCheckInterval;

                //Get Location Data
                m_PickLoc.Update(CheckLoc, m_World);

                //Check if Block is Picked
                if (m_PickLoc.IsBlockExists())
                {
                    m_bIsPickValid = true;
                    return;
                }

                //Move to Next Location
                distance -= m_RayCheckInterval;
            }
            while (distance > 0);

            m_bIsPickValid = false;
        }

        //Check which side of bound(box) hitted by ray
        private bool _CheckRayHitSide(out Vector3Int Out_Offset)
        {
            Out_Offset = Vector3Int.zero;

            if (m_PickLoc.Bound.IntersectRay(m_Ray, out float distance))
            {
                 Vector3 test_HitPoint = m_Ray.origin + distance * m_Ray.direction;
                 Vector3 Dir = test_HitPoint - m_PickLoc.Bound.center;

                float absx = Mathf.Abs(Dir.x);
                float absy = Mathf.Abs(Dir.y);
                float absz = Mathf.Abs(Dir.z);

                if (absx > absy && absx > absz)
                {
                    Out_Offset = Dir.x > 0 ? Vector3Int.right : Vector3Int.left;
                    return true;
                }
                else if (absy > absx && absy > absz)
                {
                    Out_Offset = Dir.y > 0 ? Vector3Int.up : Vector3Int.down;
                    return true;
                }
                else if (absz > absx && absz > absy)
                {
                    Out_Offset = Dir.z > 0 ? new Vector3Int(0, 0, 1) : new Vector3Int(0, 0, -1);
                    return true;
                }
                else return false;
            }
            else return false; 
        }

        private bool Handle_PlayerSpawn()
        {
            //m_bHasPlayerSpawned = true;

            return true;
        }
    }
}
