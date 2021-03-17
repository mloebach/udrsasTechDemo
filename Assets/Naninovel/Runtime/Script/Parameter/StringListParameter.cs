// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Naninovel
{
    /// <summary>
    /// Represents a serializable <see cref="Command"/> parameter with a collection of <see cref="NullableString"/> values.
    /// </summary>
    [Serializable]
    public class StringListParameter : ParameterList<NullableString>
    {
        public static implicit operator StringListParameter (List<string> value) => new StringListParameter { Value = value?.Select(v => new NullableString { Value = v }).ToList() };
        public static implicit operator List<string> (StringListParameter param) => param?.Value?.Select(v => v?.Value).ToList();
        public static implicit operator string[] (StringListParameter param) => param?.Value?.Select(v => v?.Value).ToArray();

        protected override NullableString ParseItemValueText (string valueText, out string errors) => ParseStringText(valueText, out errors);
    }
}