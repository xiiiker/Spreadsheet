// <summary>
//   <para>
//     This code is provided to start your assignment.  It was written
//     by Profs Joe, Danny, Jim, and Travis.  You should keep this attribution
//     at the top of your code where you have your header comment, along
//     with any other required information.
//   </para>
//   <para>
//     You should remove/add/adjust comments in your file as appropriate
//     to represent your work and any changes you make.
//   </para>
// </summary>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace Formula;

using System.Text.RegularExpressions;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one or more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    private string token;
    private List<string> tokens;
    private string canonicalExpression;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non-Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        canonicalExpression = formula; 
        tokens = GetTokens(formula);
        token = new string(string.Empty); 
        int leftParenthesesCount = 0;
        int rightParenthesesCount = 0;

        if (tokens.Count == 0)
        {
            throw new FormulaFormatException("Expression Violates the One Token Rule");
        }
        
        for (int index = 0; index < tokens.Count; index++)
        {
            string currentToken = tokens[index];
            string previousToken = new string("");
            token = currentToken; 

            if (index == 0)
            {
                previousToken = tokens[index];
            }
            else
            {
                previousToken = tokens[index - 1];
            }
            
            bool isCurrVar = IsVar(currentToken);
            bool isPrevVar = IsVar(previousToken);
            bool isCurrNum = double.TryParse(currentToken, out double a);
            bool isPrevNum = double.TryParse(previousToken, out double b);
            bool isCurrOper = currentToken.Equals("+") || currentToken.Equals("-") || currentToken.Equals("*") ||
                                     currentToken.Equals("/");
            bool isPrevOper = previousToken.Equals("+") || previousToken.Equals("-") ||
                                      previousToken.Equals("*") || previousToken.Equals("/");
            bool isCurrOpenParen = currentToken.Equals("(");
            bool isPrevOpenParen = previousToken.Equals("(");
            bool isCurrCloseParen = currentToken.Equals(")");
            bool isPrevCloseParen = previousToken.Equals(")");

            if (isCurrNum)
            {
                canonicalExpression += ToString(currentToken); 
            } 
            else if (isCurrVar)
            {
                foreach (char c in currentToken)
                {
                    canonicalExpression += char.ToUpper(c);
                }
            }
            else
            {
                canonicalExpression += currentToken; 
            }
            
            bool parenOperFollowingRuleConditions = (isPrevOpenParen || isPrevOper) && (isCurrVar || isCurrNum || isCurrOpenParen);

            bool extraFollowingRuleConditions = (isPrevNum || isPrevVar || isPrevCloseParen) && (isCurrCloseParen || isCurrOper);

            bool isValidCurrentToken = isCurrVar || isCurrNum || isCurrOpenParen || isCurrCloseParen || isCurrOper;
            
            bool isValidFirstToken = IsVar(tokens[0]) || double.TryParse(tokens[0], out double g) ||
                                     tokens[0].Equals("(");
            
            bool isValidLastToken = IsVar(tokens[tokens.Count - 1]) || double.TryParse(tokens[tokens.Count - 1], out double h) ||
                                    tokens[tokens.Count - 1].Equals(")");

            if (index > 0 && !parenOperFollowingRuleConditions)
            {
                throw new FormulaFormatException("Expression Violates the Parenthesis/Operator Following Rule");
            }

            if (index > 0 && !extraFollowingRuleConditions)
            {
                throw new FormulaFormatException("Expression Violates the Extra Following Rule"); 
            }
            
            if (index == 0 && !isValidFirstToken)
            {
                throw new FormulaFormatException("Expression Violates the First Token Rule");
            }

            if (index == tokens.Count - 1 && !isValidLastToken)
            {
                throw new FormulaFormatException("Expression Violates the Last Token Rule"); 
            }

            if (!isValidCurrentToken)
            {
                throw new FormulaFormatException("Expression Violates the Valid Tokens Rule");
            }
            
            foreach (char c in currentToken)
            {
                if (c.Equals('('))
                {
                    leftParenthesesCount++;
                }

                if (c.Equals(')'))
                {
                    rightParenthesesCount++;
                    if (rightParenthesesCount > leftParenthesesCount)
                    {
                        throw new FormulaFormatException("Expression Violates the Closing Parentheses Rule");
                    }
                }
            }

        }
        
        if (leftParenthesesCount != rightParenthesesCount)
        {
            throw new FormulaFormatException("Expression Violates the Balanced Parentheses Rule");
        }
        
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
	///     Variables should be returned in canonical form, having all letters converted
	///     to uppercase.
    ///   </remarks>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should return a set containing "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should return a set containing "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables( )
    {
        HashSet<string> variableSet = new HashSet<string>(); 
        foreach (string s in tokens)
        {
            if (IsVar(s))
            {
                variableSet.Add(ToString(s)); 
            }
        }
        return variableSet;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All the variable and number tokens in the string will be normalized.
    ///     For numbers, this means that the original string token is converted to
    ///     a number using double.Parse or double.TryParse, then converted back to a
    ///     string using double.ToString.
    ///     For variables, this means all letters are uppercased.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + Y1").ToString() should return "X1+Y1"
    ///       new("x1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This method should execute in O(1) time.
    ///   </para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public string ToString(string formula)
    {
        return canonicalExpression; 
    }
    
    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar( string token )
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch( token, standaloneVarPattern );
    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens( string formula )
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach ( string s in Regex.Split( formula, pattern, RegexOptions.IgnorePatternWhitespace ) )
        {
            if ( !Regex.IsMatch( s, @"^\s*$", RegexOptions.Singleline ) )
            {
                results.Add(s);
            }
        }

        return results;
    }
}


/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException( string message )
        : base( message )
    {
        // All this does is call the base constructor. No extra code needed.
    }
}