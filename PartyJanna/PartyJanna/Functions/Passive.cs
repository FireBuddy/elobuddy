using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class Passive
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "Passive";

            /*private static void CheckDamage(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs Args)
        {
            if (sender.Distance(Multi._Eu) >= 1100 || !sender.IsEnemy) return;
            if (Args.Target == null)
            {
                if (Args.End.Distance(Multi._Eu) <= Args.SData.LineWidth)
                {
                    Console.Write("Receiving damage");
                }
            }
            else if (Args.Target.IsMe)
            {
                Console.Write("Receiving damage");
            }
        }*/
    }
    }
}
