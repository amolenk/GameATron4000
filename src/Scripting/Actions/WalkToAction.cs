// namespace GameATron4000.Scripting
// {
//     public class WalkToAction
//     {
//         public WalkTo TryParse(string input, Lua lua)
//         {
//             var walkToPattern = @"^(walk to)\s(?<arg1>.*?)$";
//             match = Regex.Match(input, walkToPattern, RegexOptions.IgnoreCase);
//             if (match.Success)
//             {
//                 var arg = GetTable(LuaConstants.Tables.Name, match.Groups["arg1"].Value,
//                     LuaConstants.Tables.Types.Actor, LuaConstants.Tables.Types.Object);

//                 return ("walk_to", arg, new object[] { arg });
//             }
//         }

//         public override void Execute(ActivityFactory activityFactory)
//         {
//             LuaObject object = LuaObject.FromTable(table);
//             // TODO Get target position
//             // Add WalkTo command
//         }

//         // VerbAction
//     }
// }