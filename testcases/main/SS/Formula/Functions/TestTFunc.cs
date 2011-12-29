/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */

namespace NPOI.SS.Formula.functions;

using junit.framework.TestCase;

using NPOI.SS.Formula.Eval.AreaEval;
using NPOI.SS.Formula.Eval.BlankEval;
using NPOI.SS.Formula.Eval.BoolEval;
using NPOI.SS.Formula.Eval.ErrorEval;
using NPOI.SS.Formula.Eval.NumberEval;
using NPOI.SS.Formula.Eval.StringEval;
using NPOI.SS.Formula.Eval.ValueEval;

/**
 * Test cases for Excel function T()
 *
 * @author Josh Micich
 */
public class TestTFunc  {

	/**
	 * @return the result of calling function T() with the specified argument
	 */
	private static ValueEval invokeT(ValueEval arg) {
		ValueEval[] args = { arg, };
		ValueEval result = new T().Evaluate(args, -1, (short)-1);
		assertNotNull("result may never be null", result);
		return result;
	}
	/**
	 * Simulates call: T(A1)
	 * where cell A1 has the specified innerValue
	 */
	private ValueEval invokeTWithReference(ValueEval innerValue) {
		ValueEval arg = EvalFactory.CreateRefEval("$B$2", innerValue);
		return invokeT(arg);
	}

	private static void ConfirmText(String text) {
		ValueEval arg = new StringEval(text);
		ValueEval eval = invokeT(arg);
		StringEval se = (StringEval) Eval;
		Assert.AreEqual(text, se.StringValue);
	}

	public void TestTextValues() {

		ConfirmText("abc");
		ConfirmText("");
		ConfirmText(" ");
		ConfirmText("~");
		ConfirmText("123");
		ConfirmText("TRUE");
	}

	private static void ConfirmError(ValueEval arg) {
		ValueEval eval = invokeT(arg);
		Assert.IsTrue(arg == Eval);
	}

	public void TestErrorValues() {

		ConfirmError(ErrorEval.VALUE_INVALID);
		ConfirmError(ErrorEval.NA);
		ConfirmError(ErrorEval.REF_INVALID);
	}

	private static void ConfirmString(ValueEval Eval, String expected) {
		Assert.IsTrue(eval is StringEval);
		Assert.AreEqual(expected, ((StringEval)Eval).StringValue);
	}

	private static void ConfirmOther(ValueEval arg) {
		ValueEval eval = invokeT(arg);
		ConfirmString(eval, "");
	}

	public void TestOtherValues() {
		ConfirmOther(new NumberEval(2));
		ConfirmOther(BoolEval.FALSE);
		ConfirmOther(BlankEval.instance);  // can this particular case be verified?
	}

	public void TestRefValues() {
		ValueEval Eval;

		eval = invokeTWithReference(new StringEval("def"));
		ConfirmString(eval, "def");
		eval = invokeTWithReference(new StringEval(" "));
		ConfirmString(eval, " ");

		eval = invokeTWithReference(new NumberEval(2));
		ConfirmString(eval, "");
		eval = invokeTWithReference(BoolEval.TRUE);
		ConfirmString(eval, "");

		eval = invokeTWithReference(ErrorEval.NAME_INVALID);
		Assert.IsTrue(eval == ErrorEval.NAME_INVALID);
	}

	public void TestAreaArg() {
		ValueEval[] areaValues = new ValueEval[] {
			new StringEval("abc"), new StringEval("def"),
			new StringEval("ghi"), new StringEval("jkl"),
		};
		AreaEval ae = EvalFactory.CreateAreaEval("C10:D11", areaValues);

		ValueEval ve;
		ve = invokeT(ae);
		ConfirmString(ve, "abc");

		areaValues[0] = new NumberEval(5.0);
		ve = invokeT(ae);
		ConfirmString(ve, "");
	}
}
