using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace lab1
{
    public class Token
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Lexeme { get; set; }
        public string Position { get; set; }
    }

    public class Lexer
    {
        public List<Token> Analyze(string input)
        {
            var tokens = new List<Token>();
            var lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            bool insideComment = false;
            string commentType = "";
            int startLine = 0, startCol = 0;
            string commentContent = "";

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                int j = 0;
                while (j < line.Length)
                {
                    if (!insideComment)
                    {
                        if (line[j] == '{')
                        {
                            insideComment = true;
                            commentType = "{}";
                            startLine = i + 1;
                            startCol = j + 1;
                            tokens.Add(new Token { Code = "1", Type = "Начало комментария", Lexeme = "{", Position = $"Строка {startLine}, символ {startCol}" });
                            j++;
                        }
                        else if (j < line.Length - 1 && line[j] == '(' && line[j + 1] == '*')
                        {
                            insideComment = true;
                            commentType = "(* *)";
                            startLine = i + 1;
                            startCol = j + 1;
                            tokens.Add(new Token { Code = "1", Type = "Начало комментария", Lexeme = "(*", Position = $"Строка {startLine}, символ {startCol}" });
                            j += 2;
                        }
                        else
                        {
                            j++;
                        }
                    }
                    else
                    {
                        if (commentType == "{}" && line[j] == '}')
                        {
                            tokens.Add(new Token { Code = "2", Type = "Тело комментария", Lexeme = commentContent, Position = $"Строка {startLine}, символ {startCol + 1}" });
                            tokens.Add(new Token { Code = "3", Type = "Конец комментария", Lexeme = "}", Position = $"Строка {i + 1}, символ {j + 1}" });
                            commentContent = "";
                            insideComment = false;
                            j++;
                        }
                        else if (commentType == "(* *)" && j < line.Length - 1 && line[j] == '*' && line[j + 1] == ')')
                        {
                            tokens.Add(new Token { Code = "2", Type = "Тело комментария", Lexeme = commentContent, Position = $"Строка {startLine}, символ {startCol + 2}" });
                            tokens.Add(new Token { Code = "3", Type = "Конец комментария", Lexeme = "*)", Position = $"Строка {i + 1}, символ {j + 1}" });
                            commentContent = "";
                            insideComment = false;
                            j += 2;
                        }
                        else
                        {
                            commentContent += line[j];
                            j++;
                        }
                    }
                }
                if (insideComment) commentContent += "\n";
            }

            return tokens;
        }
    }
}
