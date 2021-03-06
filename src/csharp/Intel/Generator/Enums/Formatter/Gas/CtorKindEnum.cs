/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Linq;

namespace Generator.Enums.Formatter.Gas {
	enum CtorKind {
		Previous,
		Normal_1,
		Normal_2a,
		Normal_2b,
		Normal_2c,
		Normal_3,
		AamAad,
		asz,
		bnd2_2,
		bnd2_3,
		DeclareData,
		er_2,
		er_4,
		far,
		imul,
		maskmovq,
		movabs,
		nop,
		OpSize,
		OpSize2_bnd,
		OpSize3,
		os_A,
		os_B,
		os_bnd,
		os_jcc,
		os_loop,
		os_mem,
		os_mem_reg16,
		os_mem2,
		os2_3,
		os2_4,
		os2_bnd,
		pblendvb,
		pclmulqdq,
		pops,
		Reg16,
		sae,
		sae_pops,
		ST_STi,
		STi_ST,
		STi_ST2,
		STIG_1a,
		STIG_1b,
		xbegin,
	}

	static class CtorKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			typeof(CtorKind).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CtorKind)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(nameof(CtorKind), TypeIds.GasCtorKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
