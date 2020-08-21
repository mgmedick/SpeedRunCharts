namespace SpeedRunApp.Model.Data
{
    public class VariableValueMapping
    {
        public string VariableValueID { get; set; }
        public string VariableID { get; set; }

        //embeds
        public Variable Variable { get; set; }

        //public VariableValue() { }

        /*
        public static VariableValue CreateCustomValue(SpeedrunComClient client, string variableId, string customValue)
        {
            var value = new VariableValue();

            value.VariableID = variableId;

            value.variable = new Lazy<Variable>(() => client.Variables.GetVariable(value.VariableID));
            value.value = new Lazy<string>(() => customValue);

            return value;
        }

        public static VariableValue ParseValueDescriptor(SpeedrunComClient client, KeyValuePair<string, dynamic> valueElement)
        {
            var value = new VariableValue();

            value.VariableID = valueElement.Key;
            value.ID = valueElement.Value as string;

            //Parse Links

            value.variable = new Lazy<Variable>(() => client.Variables.GetVariable(value.VariableID));
            value.value = new Lazy<string>(() => value.Variable.Values.FirstOrDefault(x => x.ID == value.ID).Value);

            return value;
        }

        public static VariableValue ParseIDPair(SpeedrunComClient client, Variable variable, KeyValuePair<string, dynamic> valueElement)
        {
            var value = new VariableValue();

            value.VariableID = variable.ID;
            value.ID = valueElement.Key as string;

            //Parse Links

            value.variable = new Lazy<Variable>(() => variable);

            var valueName = valueElement.Value as string;
            value.value = new Lazy<string>(() => valueName);

            return value;
        }
        */

        //public override int GetHashCode()
        //{
        //    return (ID ?? string.Empty).GetHashCode();
        //}

        //public override bool Equals(object obj)
        //{
        //    var other = obj as VariableValue;

        //    if (other == null)
        //        return false;

        //    return ID == other.ID;
        //}

        //public override string ToString()
        //{
        //    return Value;
        //}
    }
}
