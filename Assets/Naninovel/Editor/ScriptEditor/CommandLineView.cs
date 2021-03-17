// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Naninovel.Lexing;
using UnityEngine.UIElements;

namespace Naninovel
{
    public class CommandLineView : ScriptLineView
    {
        private struct ParameterFieldData
        {
            public LineTextField Field;
            public string Id, Value;
            public bool Nameless;
        }

        public string CommandId { get; private set; }

        private static readonly Parsing.CommandLineParser commandLineParser = new Parsing.CommandLineParser();
        private static readonly List<string> errors = new List<string>();
        private static readonly ProjectMetadata projectMeta = new ProjectMetadata();

        private readonly List<ParameterFieldData> parameterFields = new List<ParameterFieldData>();
        private readonly List<ParameterFieldData> delayedAddFields = new List<ParameterFieldData>();

        private bool hideParameters = default;

        static CommandLineView ()
        {
            IDEMetadataWindow.GenerateCommandsMetadata(projectMeta, Command.CommandTypes.Values,
                _ => (null, null, null), _ => null);
        }

        private CommandLineView (int lineIndex, VisualElement container)
            : base(lineIndex, container) { }

        public static ScriptLineView CreateDefault (int lineIndex, string commandId,
            VisualElement container, bool hideParameters)
        {
            var lineText = $"{Constants.CommandLineId}{commandId}";
            var tokens = new List<Token>();
            new Lexer().TokenizeLine(lineText, tokens);
            return CreateOrError(lineIndex, lineText, tokens, container, hideParameters);
        }

        public static ScriptLineView CreateOrError (int lineIndex, string lineText,
            IReadOnlyList<Token> tokens, VisualElement container, bool hideParameters)
        {
            errors.Clear();
            var model = commandLineParser.Parse(lineText, tokens, errors)?.Command;
            if (model is null || errors.Count > 0) return Error(errors.FirstOrDefault());

            var meta = projectMeta.commands.FirstOrDefault(c => (c.id?.EqualsFastIgnoreCase(model.Identifier) ?? false) ||
                                                                (c.alias?.EqualsFastIgnoreCase(model.Identifier) ?? false));
            if (meta is null) return Error($"Unknown command: `{model.Identifier}`");

            var nameLabel = new Label(model.Identifier.Text.FirstToLower());
            nameLabel.name = "InputLabel";
            nameLabel.AddToClassList("Inlined");

            var commandLineView = new CommandLineView(lineIndex, container);
            commandLineView.Content.Add(nameLabel);
            commandLineView.CommandId = model.Identifier;
            commandLineView.hideParameters = hideParameters;

            foreach (var paramMeta in meta.@params)
            {
                var data = new ParameterFieldData {
                    Id = string.IsNullOrEmpty(paramMeta.alias) ? paramMeta.id.FirstToLower() : paramMeta.alias,
                    Value = GetValueFor(paramMeta),
                    Nameless = paramMeta.nameless
                };
                if (commandLineView.ShouldShowParameter(data))
                    commandLineView.AddParameterField(data);
                else commandLineView.delayedAddFields.Add(data);
            }

            return commandLineView;

            ErrorLineView Error (string e) => new ErrorLineView(lineIndex, lineText, container, model?.Identifier, e);

            string GetValueFor (ProjectMetadata.ParameterMetadata m)
            {
                var param = model.Parameters.FirstOrDefault(p => p.Identifier.Text.EqualsFastIgnoreCase(m.id) ||
                                                                 p.Identifier.Text.EqualsFastIgnoreCase(m.alias));
                return param?.Value;
            }
        }

        public override string GenerateLineText ()
        {
            var result = $"{Constants.CommandLineId}{CommandId}";
            if (parameterFields.Any(f => f.Nameless))
                result += $" {EncodeValue(parameterFields.First(f => f.Nameless))}";

            foreach (var data in parameterFields)
                if (!string.IsNullOrEmpty(data.Field.label) && !string.IsNullOrWhiteSpace(data.Field.value))
                    result += $" {data.Id}:{EncodeValue(data)}";
            return result;

            string EncodeValue (ParameterFieldData data)
            {
                var value = data.Field.value;
                if (value.Any(char.IsWhiteSpace) && !value.WrappedIn("\""))
                    return $"\"{value}\"";
                return value;
            }
        }

        protected override void ApplyFocusedStyle ()
        {
            base.ApplyFocusedStyle();

            if (DragManipulator.Active) return;
            ShowUnAssignedNamedFields();
        }

        protected override void ApplyNotFocusedStyle ()
        {
            base.ApplyNotFocusedStyle();

            HideUnAssignedNamedFields();
        }

        protected override void ApplyHoveredStyle ()
        {
            base.ApplyHoveredStyle();

            if (DragManipulator.Active) return;
            ShowUnAssignedNamedFields();
        }

        protected override void ApplyNotHoveredStyle ()
        {
            base.ApplyNotHoveredStyle();

            if (FocusedLine == this) return;
            HideUnAssignedNamedFields();
        }

        private void AddParameterField (ParameterFieldData data)
        {
            data.Field = new LineTextField(data.Nameless ? "" : data.Id, DecodeValue(data.Value ?? ""));
            if (!data.Nameless) data.Field.AddToClassList("NamedParameterLabel");
            parameterFields.Add(data);
            if (ShouldShowParameter(data)) Content.Add(data.Field);

            string DecodeValue (string value)
            {
                if (value.WrappedIn("\"") && value.Any(char.IsWhiteSpace))
                    return value.Substring(1, value.Length - 2);
                return value;
            }
        }

        private bool ShouldShowParameter (ParameterFieldData data)
        {
            return !hideParameters || data.Nameless || !string.IsNullOrEmpty(data.Value);
        }

        private void ShowUnAssignedNamedFields ()
        {
            if (!hideParameters) return;

            // Add un-assigned fields in case they weren't added on init.
            if (delayedAddFields.Count > 0)
            {
                foreach (var data in delayedAddFields)
                    AddParameterField(data);
                delayedAddFields.Clear();
            }

            foreach (var data in parameterFields)
                if (!Content.Contains(data.Field))
                    Content.Add(data.Field);
        }

        private void HideUnAssignedNamedFields ()
        {
            if (!hideParameters) return;

            foreach (var data in parameterFields)
                if (!string.IsNullOrEmpty(data.Field.label)
                    && string.IsNullOrWhiteSpace(data.Field.value)
                    && Content.Contains(data.Field))
                    Content.Remove(data.Field);
        }
    }
}
