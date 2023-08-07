using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BoardCell Type Definition")]
public class BoardCellTypeDefinition : BaseEntitiyTypeDefinition
{
    [BHeader("BoardCell Settings")]
    [SerializeField] protected ParticleSystem psParticle;
    [SerializeField] protected GameObject spinObject;
    [SerializeField] protected ShineEffect shine;


    public ParticleSystem PsParticle => psParticle;
    public GameObject SpinObject => spinObject;
    public ShineEffect ShineObject => shine;
}
