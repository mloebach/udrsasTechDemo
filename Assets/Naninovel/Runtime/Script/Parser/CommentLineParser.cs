// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All Rights Reserved.

using Naninovel.Parsing;

namespace Naninovel
{
    public class CommentLineParser : ScriptLineParser<CommentScriptLine, CommentLine, Parsing.CommentLineParser>
    {
        protected override CommentScriptLine Parse (CommentLine lineModel)
        {
            return new CommentScriptLine(lineModel.CommentText, LineIndex, LineHash);
        }
    }
}
