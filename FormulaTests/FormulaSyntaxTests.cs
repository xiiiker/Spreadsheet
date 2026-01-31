// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> [Keefe Russell] </authors>
// <date> [1/23/26] </date>

namespace FormulaTests;

using Formula; 

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNoTokens_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( string.Empty ) );
    }

    /// <summary>
    ///   <para>
    ///     "One Token Rule" test that's testing for when only a single token is in the Formula
    ///     constructor. According to the specs, it should be valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOneToken_Valid()
    {
        _ = new Formula("2");
    }

    // --- Tests for Valid Token Rule ---
    
    /// <summary>
    ///   <para>
    ///     "Valid Token Rule" test that's testing for when an invalid token is in the expression
    ///     going into the Formula constructor. According to the specs, it should be invalid. 
    ///   </para>
    ///
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestInvalidTokens_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("24+")); 
    }

    /// <summary>
    ///   <para>
    ///     "Valid Token Rule" test that's testing for when an expression going into the Formula
    ///     constructor contains an operator and is written without spaces. According to the specs, 
    ///     it should be valid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokensSeparateCorrectly_Valid()
    {
        _ = new Formula("2+9");
    }

    /// <summary>
    ///   <para>
    ///     "Valid Token Rule" test that's testing for when an expression going into the Formula
    ///     constructor contains variables (as defined by the specs). According to the specs, it
    ///     should be valid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestVariableTokens_Valid()
    {
        _ = new Formula("Aa29 - bgSh23");
    }

    /// <summary>
    ///   <para>
    ///     "Valid Token Rule" test that's testing for when an expression going into the Formula
    ///     constructor contains invalid tokens that look to be the reverse of variable tokens.
    ///     According to the specs, it should be invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestReverseOfVariableTokens_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("24ae + 4"));
    }
    
    // --- Tests for Closing Parenthesis Rule
    
    /// <summary>
    ///   <para>
    ///     "Closing Parenthesis Rule" test that's testing for when the expression going into
    ///     the Formula constructor has an equal number of closing parentheses. (At no point does
    ///     the expression have a number of closing parentheses outweighing the opening ones).
    ///     According to the specs, it should be valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEqualNumOfClosingParentheses_Valid()
    {
        _ = new Formula("(2e5 - 3.0)"); 
    }

    /// <summary>
    ///   <para>
    ///     "Closing Parenthesis Rule" test that's testing for when the expression going
    ///     into the Formula constructor has a greater number of closing parentheses than opening
    ///     ones. According to the specs, it should be invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGreaterNumOfClosingParentheses_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(7.0 + 2))")); 
    }
    
    /// <summary>
    ///   <para>
    ///     "Closing Parenthesis Rule" test that's testing for when the expression going
    ///     into the Formula constructor has at a point but not at the end, a greater number of
    ///     closing parentheses than opening ones. According to the specs, it should be invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGreaterNumOfClosingParenthesesThenBalanced_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("(2 + 4))(") );
    }
    
    // --- Tests for Balanced Parentheses Rule
    
    /// <summary>
    ///   <para>
    ///     "Balanced Parenthesis Rule" test that's testing for when the expression going
    ///     into the Formula constructor has a balanced single pair of opening and closing
    ///     parentheses. According to the specs, it should be valid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPairOpeningAndClosingParentheses_Valid()
    {
        _ = new Formula("(5 * 5)");
    }

    /// <summary>
    ///   <para>
    ///     "Balanced Parenthesis Rule" test that's testing for when the expression going
    ///     into the Formula constructor has three balanced pairs of opening and closing
    ///     parentheses. According to the specs, it should be valid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTriplePairOpeningAndClosingParentheses_Valid()
    {
        _ = new Formula("(((9 - 3)))");
    }

    /// <summary>
    ///   <para>
    ///     "Balanced Parenthesis Rule" test that's testing for when the expression going
    ///     into the Formula constructor has a greater number of opening parentheses than
    ///     closing ones. According to the specs, it should be invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestGreaterNumOfOpeningParentheses_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("((2 / 4)")); 
    }
    
    // --- Tests for First Token Rule

    /// <summary>
    ///   <para>
    ///     Make sure a simple well-formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid( )
    {
        _ = new Formula("1+1");
    }

    /// <summary>
    ///   <para>
    ///     "First Token Rule" test that's testing for if the first token going into the
    ///     Formula constructor is a closing parenthesis. According to the specs, it should
    ///     be invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenClosingParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula(")(2+2)"));
    }
  
    // --- Tests for  Last Token Rule ---
    
    /// <summary>
    ///   <para>
    ///     "Last Token Rule" test that's testing for if the last token going into the
    ///     Formula constructor is a variable. According to the specs, it should be valid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastTokenVariable_Valid()
    {
        _ = new Formula("2*ara23"); 
    }

    /// <summary>
    ///   <para>
    ///     "Last Token Rule" test that's testing for if the last token going into the
    ///     Formula constructor is an operator. According to the specs, it should be
    ///     invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastTokenOperator_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("4 +")); 
    }
    
    // --- Tests for Parentheses/Operator Following Rule ---
    
    /// <summary>
    ///   <para>
    ///     "Parentheses/Operator Following Rule" test that's testing for when the expression
    ///     going into the Formula constructor contains a closing parenthesis following an
    ///     opening one. According to the specs, it should be invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestClosingParenthesisFollowingOpeningParenthesis_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("()"));
    }

    /// <summary>
    ///   <para>
    ///     "Parentheses/Operator Following Rule" test that's testing for when the expression
    ///     going into the Formula constructor contains an operator following an operator.
    ///     According to the specs, it should be invalid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowingOperator_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("2+*4.0"));
    }

    // --- Tests for Extra Following Rule ---
    
    /// <summary>
    ///   <para>
    ///     "Extra Following Rule" test that's testing for when the expression going
    ///     into the Formula constructor contains an operator following a variable.
    ///     According to the specs, it should be valid. 
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowingVariable_Valid()
    {
        _ = new Formula("AaA123 + 2"); 
    }
    
    /// <summary>
    ///   <para>
    ///     "Extra Following Rule" test that's testing for when the expression going
    ///     into the Formula constructor contains a number following a number. 
    ///     According to the specs, it should be invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNumberFollowingNumber_Invalid()
    {
        Assert.Throws<FormulaFormatException>(() => _ = new Formula("2 2 + 4"));
    }
}