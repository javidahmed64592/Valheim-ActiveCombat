using System.Collections.Generic;
using System.Reflection;

using HarmonyLib;

namespace ActiveCombat.Core;

/// <summary>Small transpiler helpers that count their matches, so a game update that moves the code fails loudly.</summary>
internal static class Transpile
{
    /// <summary>Replaces every call to <paramref name="methodName"/> with a call to <paramref name="replacement"/>.</summary>
    /// <remarks>The replacement must take the same stack values as the original call, including the instance.</remarks>
    public static IEnumerable<CodeInstruction> ReplaceCall(
        IEnumerable<CodeInstruction> instructions, string methodName, MethodInfo replacement, string patchName)
    {
        int count = 0;
        foreach (CodeInstruction instruction in instructions)
        {
            if (instruction.operand is MethodInfo method && method.Name == methodName && instruction.opcode.FlowControl == System.Reflection.Emit.FlowControl.Call)
            {
                instruction.opcode = System.Reflection.Emit.OpCodes.Call;
                instruction.operand = replacement;
                count++;
            }
            yield return instruction;
        }
        WarnIfNone(count, patchName);
    }

    /// <summary>
    /// Drops every call to <paramref name="methodName"/> (which must return nothing), popping the
    /// <paramref name="stackValues"/> values it would have taken, including the instance.
    /// </summary>
    public static IEnumerable<CodeInstruction> RemoveCall(
        IEnumerable<CodeInstruction> instructions, string methodName, int stackValues, string patchName)
    {
        int count = 0;
        foreach (CodeInstruction instruction in instructions)
        {
            if (instruction.operand is MethodInfo method && method.Name == methodName && instruction.opcode.FlowControl == System.Reflection.Emit.FlowControl.Call)
            {
                CodeInstruction first = new(System.Reflection.Emit.OpCodes.Pop);
                first.labels.AddRange(instruction.labels);
                first.blocks.AddRange(instruction.blocks);
                yield return first;
                for (int i = 1; i < stackValues; i++)
                {
                    yield return new CodeInstruction(System.Reflection.Emit.OpCodes.Pop);
                }
                count++;
                continue;
            }
            yield return instruction;
        }
        WarnIfNone(count, patchName);
    }

    /// <summary>Passes every value read from <paramref name="field"/> through <paramref name="scale"/> (a static float -> float method).</summary>
    public static IEnumerable<CodeInstruction> ScaleField(
        IEnumerable<CodeInstruction> instructions, FieldInfo field, MethodInfo scale, string patchName)
    {
        int count = 0;
        foreach (CodeInstruction instruction in instructions)
        {
            yield return instruction;
            if (instruction.LoadsField(field))
            {
                yield return new CodeInstruction(System.Reflection.Emit.OpCodes.Call, scale);
                count++;
            }
        }
        WarnIfNone(count, patchName);
    }

    private static void WarnIfNone(int count, string patchName)
    {
        if (count == 0)
        {
            ActiveCombatPlugin.Log.LogWarning($"{patchName}: found nothing to patch. The game may have changed, so this feature is off.");
        }
    }
}
