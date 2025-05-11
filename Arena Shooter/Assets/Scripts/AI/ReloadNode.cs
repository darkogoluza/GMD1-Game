using AI.Core;

namespace AI
{
   public class ReloadNode : IBehaviorNode
   {
       private readonly IWeapon _weapon;
   
       public ReloadNode(IWeapon weapon)
       {
           _weapon = weapon;
       }
   
       public NodeStatus Tick()
       {
           _weapon.Reload();
   
           return NodeStatus.Success;
       }
   } 
}
