using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Weavers
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        private MethodReference _nowMethod;
        private MethodReference _toLongTimeStringMethod;
        private TypeReference _dateTimeType;
        private MethodReference _concatMethod;
        private MethodReference _writLineMethod;

        public void Execute()
        {
            InitialiseReferences();
            foreach (var method in GetMethods())
                Inject(method);
        }

        private void InitialiseReferences()
        {
            _dateTimeType = ModuleDefinition.Import(typeof(DateTime));
            var dateTimeDefinition = _dateTimeType.Resolve();
            _nowMethod = ModuleDefinition.Import(dateTimeDefinition.Methods.First(x => x.Name == "get_Now"));
            _toLongTimeStringMethod = ModuleDefinition.Import(dateTimeDefinition.Methods.First(x => x.Name == "ToLongTimeString"));
            var stringType = ModuleDefinition.Import(typeof(string)).Resolve();
            _concatMethod = ModuleDefinition.Import(stringType.Methods.First(x => x.Name == "Concat" && x.Parameters.Count == 2));
            var consoleType = ModuleDefinition.Import(typeof(Console)).Resolve();
            _writLineMethod = ModuleDefinition.Import(consoleType.Methods.First(x => x.Name == "WriteLine" && x.Parameters.Count == 1 && x.Parameters[0].ParameterType.Name == "String"));
        }

        private void Inject(MethodDefinition method)
        {
            var instructions = method.Body.Instructions;
            var variableDefinition = new VariableDefinition(_dateTimeType);
            method.Body.Variables.Add(variableDefinition);
            instructions.Insert(0, Instruction.Create(OpCodes.Call, _nowMethod));
            instructions.Insert(1, Instruction.Create(OpCodes.Stloc_0));
            instructions.Insert(2, Instruction.Create(OpCodes.Ldloca_S, variableDefinition));
            instructions.Insert(3, Instruction.Create(OpCodes.Call, _toLongTimeStringMethod));
            instructions.Insert(4, Instruction.Create(OpCodes.Ldstr, " Teste " + method.Name));
            instructions.Insert(5, Instruction.Create(OpCodes.Call, _concatMethod));
            instructions.Insert(6, Instruction.Create(OpCodes.Call, _writLineMethod));
        }

        private IEnumerable<MethodDefinition> GetMethods()
        {
            return ModuleDefinition.Types.SelectMany(x => x.Methods.Where(ContainsAttribute));
        }

        private static bool ContainsAttribute(MethodDefinition method)
        {
            return method.CustomAttributes.Any(x => x.Constructor.DeclaringType.FullName.EndsWith("LogMethodEntryAttribute"));
        }
    }
}