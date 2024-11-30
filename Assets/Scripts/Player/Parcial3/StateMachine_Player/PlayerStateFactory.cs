
public class PlayerStateFactory
{
   PlayerStateMachine _context;

   public PlayerStateFactory(PlayerStateMachine currentContext)
   {
    _context = currentContext;
   }

   public PlayerBaseState Idle(){
    return new PlayerIdleState(_context, this);
   }
   public PlayerBaseState Walk(){
    return new PlayerWalkingState(_context, this);
   }
   public PlayerBaseState Dodge(){
    return new PlayerDodgeState(_context, this);
   }
   public PlayerBaseState Grounded(){
    return new PlayerGroundState(_context, this);
   }

   public PlayerBaseState Attack(){
    return new PlayerAttackState(_context, this);
   }
}
