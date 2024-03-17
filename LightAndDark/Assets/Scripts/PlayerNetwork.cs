using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> netState = new(writePerm: NetworkVariableWritePermission.Owner);

    private Vector2 vel;

    private float rotVel;

    [SerializeField] private float cheapInterpolationTime = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
            };
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position, netState.Value.Position, ref vel, cheapInterpolationTime);
        }
    }


    struct PlayerNetworkData : INetworkSerializable
    {
        private float x, y;

        internal Vector2 Position
        {
            get => new Vector2(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }
        

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref y);
        }
    }

}