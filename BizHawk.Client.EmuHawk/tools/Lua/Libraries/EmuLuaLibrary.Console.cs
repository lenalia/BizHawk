﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

using BizHawk.Client.Common;
using LuaInterface;

namespace BizHawk.Client.EmuHawk
{
	public class ConsoleLuaLibrary : LuaLibraryBase
	{
		public override string Name { get { return "console"; } }

		[LuaMethodAttributes(
			"clear",
			"clears the output box of the Lua Console window"
		)]
		public static void Clear()
		{
			GlobalWin.Tools.LuaConsole.ClearOutputWindow();
		}

		[LuaMethodAttributes(
			"getluafunctionslist",
			"returns a list of implemented functions"
		)]
		public static string GetLuaFunctionsList()
		{
			var list = new StringBuilder();
			foreach (var function in GlobalWin.Tools.LuaConsole.LuaImp.Docs.FunctionList)
			{
				list.AppendLine(function.Name);
			}

			return list.ToString();
		}

		[LuaMethodAttributes(
			"log",
			"Outputs the given object to the output box on the Lua Console dialog. Note: Can accept a LuaTable"
		)]
		public static void Log(params object[] outputs)
		{
			foreach (var output in outputs)
			{
				if (output == null)
				{
					GlobalWin.Tools.LuaConsole.WriteToOutputWindow("NULL");
				}
				else
				{
					if (output is LuaTable)
					{
						var sb = new StringBuilder();
						var lti = output as LuaTable;

						var keys = (from object key in lti.Keys select key.ToString()).ToList();
						var values = (from object value in lti.Values select value.ToString()).ToList();

						var kvps = new List<KeyValuePair<string, string>>();
						for (var i = 0; i < keys.Count; i++)
						{
							if (i < values.Count)
							{
								kvps.Add(new KeyValuePair<string, string>(keys[i], values[i]));
							}
						}

						kvps = kvps.OrderBy(x => x.Key).ToList();
						foreach (var kvp in kvps)
						{
							sb
								.Append("\"")
								.Append(kvp.Key)
								.Append("\": \"")
								.Append(kvp.Value)
								.Append("\"")
								.AppendLine();
						}

						GlobalWin.Tools.LuaConsole.WriteToOutputWindow(sb.ToString());
					}
					else
					{
						GlobalWin.Tools.LuaConsole.WriteToOutputWindow(output.ToString());
					}
				}
			}
		}

		// Single param version is used by logOutputCallback of some libraries.
		public static void Log(string output)
		{
			var outputs = new[] { output };
			Log(outputs);
		}
	}
}
