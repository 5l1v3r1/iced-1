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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	sealed class UsedRegisterEqualityComparer : IEqualityComparer<UsedRegister> {
		public static readonly UsedRegisterEqualityComparer Instance = new UsedRegisterEqualityComparer();
		UsedRegisterEqualityComparer() { }

		public bool Equals(UsedRegister x, UsedRegister y) =>
			x.Register == y.Register && x.Access == y.Access;

		public int GetHashCode(UsedRegister obj) =>
			(int)obj.Register ^ (int)obj.Access;
	}

	sealed class UsedMemoryEqualityComparer : IEqualityComparer<UsedMemory> {
		public static readonly UsedMemoryEqualityComparer Instance = new UsedMemoryEqualityComparer();
		UsedMemoryEqualityComparer() { }

		public bool Equals(UsedMemory x, UsedMemory y) =>
			x.Segment == y.Segment &&
			x.Base == y.Base &&
			x.Index == y.Index &&
			x.Scale == y.Scale &&
			x.Displacement == y.Displacement &&
			x.MemorySize == y.MemorySize &&
			x.Access == y.Access;

		public int GetHashCode(UsedMemory obj) {
			int hc = 0;
			hc ^= (int)obj.Segment;
			hc ^= (int)obj.Base << 8;
			hc ^= (int)obj.Index << 16;
			hc ^= obj.Scale << 28;
			hc ^= obj.Displacement.GetHashCode();
			hc ^= (int)obj.MemorySize << 12;
			hc ^= (int)obj.Access << 24;
			return hc;
		}
	}

	public abstract class InstructionInfoTest {
		protected void TestInstructionInfo(int bitness, string hexBytes, Code code, DecoderOptions options, int lineNo, InstructionInfoTestCase testCase) {
			var codeBytes = HexUtils.ToByteArray(hexBytes);
			Instruction instr;
			if (testCase.IsSpecial) {
				if (bitness == 16 && code == Code.Popw_CS && hexBytes == "0F") {
					instr = default;
					instr.Code = Code.Popw_CS;
					instr.Op0Kind = OpKind.Register;
					instr.Op0Register = Register.CS;
					instr.CodeSize = CodeSize.Code16;
					instr.Length = 1;
				}
				else if (code >= Code.DeclareByte) {
					instr = default;
					instr.Code = code;
					instr.DeclareDataCount = 1;
					Assert.Equal(64, bitness);
					instr.CodeSize = CodeSize.Code64;
					switch (code) {
					case Code.DeclareByte:
						Assert.Equal("66", hexBytes);
						instr.SetDeclareByteValue(0, 0x66);
						break;
					case Code.DeclareWord:
						Assert.Equal("6644", hexBytes);
						instr.SetDeclareWordValue(0, 0x4466);
						break;
					case Code.DeclareDword:
						Assert.Equal("664422EE", hexBytes);
						instr.SetDeclareDwordValue(0, 0xEE224466);
						break;
					case Code.DeclareQword:
						Assert.Equal("664422EE12345678", hexBytes);
						instr.SetDeclareQwordValue(0, 0x78563412EE224466);
						break;
					default: throw new InvalidOperationException();
					}
				}
				else {
					var decoder = CreateDecoder(bitness, codeBytes, options);
					instr = decoder.Decode();
					if (codeBytes.Length > 1 && codeBytes[0] == 0x9B && instr.Length == 1) {
						instr = decoder.Decode();
						switch (instr.Code) {
						case Code.Fnstenv_m14byte: instr.Code = Code.Fstenv_m14byte; break;
						case Code.Fnstenv_m28byte: instr.Code = Code.Fstenv_m28byte; break;
						case Code.Fnstcw_m2byte: instr.Code = Code.Fstcw_m2byte; break;
						case Code.Fneni: instr.Code = Code.Feni; break;
						case Code.Fndisi: instr.Code = Code.Fdisi; break;
						case Code.Fnclex: instr.Code = Code.Fclex; break;
						case Code.Fninit: instr.Code = Code.Finit; break;
						case Code.Fnsetpm: instr.Code = Code.Fsetpm; break;
						case Code.Fnsave_m94byte: instr.Code = Code.Fsave_m94byte; break;
						case Code.Fnsave_m108byte: instr.Code = Code.Fsave_m108byte; break;
						case Code.Fnstsw_m2byte: instr.Code = Code.Fstsw_m2byte; break;
						case Code.Fnstsw_AX: instr.Code = Code.Fstsw_AX; break;
						default: throw new InvalidOperationException();
						}
					}
					else
						throw new InvalidOperationException();
				}
			}
			else {
				var decoder = CreateDecoder(bitness, codeBytes, options);
				instr = decoder.Decode();
			}
			Assert.Equal(code, instr.Code);

			Assert.Equal(testCase.StackPointerIncrement, instr.StackPointerIncrement);

			var info = new InstructionInfoFactory().GetInfo(instr);
			Assert.Equal(testCase.Encoding, info.Encoding);
			Assert.Equal(testCase.CpuidFeatures, info.CpuidFeatures);
			Assert.Equal(testCase.RflagsRead, info.RflagsRead);
			Assert.Equal(testCase.RflagsUndefined, info.RflagsUndefined);
			Assert.Equal(testCase.RflagsWritten, info.RflagsWritten);
			Assert.Equal(testCase.RflagsCleared, info.RflagsCleared);
			Assert.Equal(testCase.RflagsSet, info.RflagsSet);
			Assert.Equal(testCase.IsPrivileged, info.IsPrivileged);
			Assert.Equal(testCase.IsProtectedMode, info.IsProtectedMode);
			Assert.Equal(testCase.IsStackInstruction, info.IsStackInstruction);
			Assert.Equal(testCase.IsSaveRestoreInstruction, info.IsSaveRestoreInstruction);
			Assert.Equal(testCase.FlowControl, info.FlowControl);
			Assert.Equal(testCase.Op0Access, info.Op0Access);
			Assert.Equal(testCase.Op1Access, info.Op1Access);
			Assert.Equal(testCase.Op2Access, info.Op2Access);
			Assert.Equal(testCase.Op3Access, info.Op3Access);
			Assert.Equal(testCase.Op4Access, info.Op4Access);
			Assert.Equal(
				new HashSet<UsedMemory>(testCase.UsedMemory, UsedMemoryEqualityComparer.Instance),
				new HashSet<UsedMemory>(info.GetUsedMemory(), UsedMemoryEqualityComparer.Instance));
			Assert.Equal(
				new HashSet<UsedRegister>(GetUsedRegisters(testCase.UsedRegisters), UsedRegisterEqualityComparer.Instance),
				new HashSet<UsedRegister>(GetUsedRegisters(info.GetUsedRegisters()), UsedRegisterEqualityComparer.Instance));

			Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
			Debug.Assert(instr.OpCount <= IcedConstants.MaxOpCount);
			for (int i = 0; i < instr.OpCount; i++) {
				switch (i) {
				case 0:
					Assert.Equal(testCase.Op0Access, info.GetOpAccess(i));
					break;

				case 1:
					Assert.Equal(testCase.Op1Access, info.GetOpAccess(i));
					break;

				case 2:
					Assert.Equal(testCase.Op2Access, info.GetOpAccess(i));
					break;

				case 3:
					Assert.Equal(testCase.Op3Access, info.GetOpAccess(i));
					break;

				case 4:
					Assert.Equal(testCase.Op4Access, info.GetOpAccess(i));
					break;

				default:
					throw new InvalidOperationException();
				}
			}
			for (int i = instr.OpCount; i < IcedConstants.MaxOpCount; i++)
				Assert.Equal(OpAccess.None, info.GetOpAccess(i));

			Assert.Equal(RflagsBits.None, info.RflagsWritten & (info.RflagsCleared | info.RflagsSet | info.RflagsUndefined));
			Assert.Equal(RflagsBits.None, info.RflagsCleared & (info.RflagsWritten | info.RflagsSet | info.RflagsUndefined));
			Assert.Equal(RflagsBits.None, info.RflagsSet & (info.RflagsWritten | info.RflagsCleared | info.RflagsUndefined));
			Assert.Equal(RflagsBits.None, info.RflagsUndefined & (info.RflagsWritten | info.RflagsCleared | info.RflagsSet));
			Assert.Equal(info.RflagsWritten | info.RflagsCleared | info.RflagsSet | info.RflagsUndefined, info.RflagsModified);

			var info2 = new InstructionInfoFactory().GetInfo(instr, InstructionInfoOptions.None);
			CheckEqual(ref info, ref info2, hasRegs2: true, hasMem2: true);
			info2 = new InstructionInfoFactory().GetInfo(instr, InstructionInfoOptions.NoMemoryUsage);
			CheckEqual(ref info, ref info2, hasRegs2: true, hasMem2: false);
			info2 = new InstructionInfoFactory().GetInfo(instr, InstructionInfoOptions.NoRegisterUsage);
			CheckEqual(ref info, ref info2, hasRegs2: false, hasMem2: true);
			info2 = new InstructionInfoFactory().GetInfo(instr, InstructionInfoOptions.NoRegisterUsage | InstructionInfoOptions.NoMemoryUsage);
			CheckEqual(ref info, ref info2, hasRegs2: false, hasMem2: false);

			Assert.Equal(info.Encoding, instr.Code.Encoding());
#if !NO_ENCODER
			Assert.Equal(code.ToOpCode().Encoding, instr.Code.Encoding());
#endif
			var cf = instr.Code.CpuidFeatures();
			if (cf.Length == 1 && cf[0] == CpuidFeature.AVX && instr.Op1Kind == OpKind.Register && (code == Code.VEX_Vbroadcastss_xmm_xmmm32 || code == Code.VEX_Vbroadcastss_ymm_xmmm32 || code == Code.VEX_Vbroadcastsd_ymm_xmmm64))
				cf = new[] { CpuidFeature.AVX2 };
			Assert.Equal(info.CpuidFeatures, cf);
			Assert.Equal(info.FlowControl, instr.Code.FlowControl());
			Assert.Equal(info.IsProtectedMode, instr.Code.IsProtectedMode());
			Assert.Equal(info.IsPrivileged, instr.Code.IsPrivileged());
			Assert.Equal(info.IsStackInstruction, instr.Code.IsStackInstruction());
			Assert.Equal(info.IsSaveRestoreInstruction, instr.Code.IsSaveRestoreInstruction());

			Assert.Equal(info.Encoding, instr.Encoding);
			Assert.Equal(info.CpuidFeatures, instr.CpuidFeatures);
			Assert.Equal(info.FlowControl, instr.FlowControl);
			Assert.Equal(info.IsProtectedMode, instr.IsProtectedMode);
			Assert.Equal(info.IsPrivileged, instr.IsPrivileged);
			Assert.Equal(info.IsStackInstruction, instr.IsStackInstruction);
			Assert.Equal(info.IsSaveRestoreInstruction, instr.IsSaveRestoreInstruction);
			Assert.Equal(info.RflagsRead, instr.RflagsRead);
			Assert.Equal(info.RflagsWritten, instr.RflagsWritten);
			Assert.Equal(info.RflagsCleared, instr.RflagsCleared);
			Assert.Equal(info.RflagsSet, instr.RflagsSet);
			Assert.Equal(info.RflagsUndefined, instr.RflagsUndefined);
			Assert.Equal(info.RflagsModified, instr.RflagsModified);
		}

		void CheckEqual(ref InstructionInfo info1, ref InstructionInfo info2, bool hasRegs2, bool hasMem2) {
			if (hasRegs2)
				Assert.Equal(info1.GetUsedRegisters(), info2.GetUsedRegisters(), UsedRegisterEqualityComparer.Instance);
			else
				Assert.Empty(info2.GetUsedRegisters());
			if (hasMem2)
				Assert.Equal(info1.GetUsedMemory(), info2.GetUsedMemory(), UsedMemoryEqualityComparer.Instance);
			else
				Assert.Empty(info2.GetUsedMemory());
			Assert.Equal(info1.IsProtectedMode, info2.IsProtectedMode);
			Assert.Equal(info1.IsPrivileged, info2.IsPrivileged);
			Assert.Equal(info1.IsStackInstruction, info2.IsStackInstruction);
			Assert.Equal(info1.IsSaveRestoreInstruction, info2.IsSaveRestoreInstruction);
			Assert.Equal(info1.Encoding, info2.Encoding);
			Assert.Equal(info1.CpuidFeatures, info2.CpuidFeatures);
			Assert.Equal(info1.FlowControl, info2.FlowControl);
			Assert.Equal(info1.Op0Access, info2.Op0Access);
			Assert.Equal(info1.Op1Access, info2.Op1Access);
			Assert.Equal(info1.Op2Access, info2.Op2Access);
			Assert.Equal(info1.Op3Access, info2.Op3Access);
			Assert.Equal(info1.Op4Access, info2.Op4Access);
			Assert.Equal(info1.RflagsRead, info2.RflagsRead);
			Assert.Equal(info1.RflagsWritten, info2.RflagsWritten);
			Assert.Equal(info1.RflagsCleared, info2.RflagsCleared);
			Assert.Equal(info1.RflagsSet, info2.RflagsSet);
			Assert.Equal(info1.RflagsUndefined, info2.RflagsUndefined);
			Assert.Equal(info1.RflagsModified, info2.RflagsModified);
		}

		IEnumerable<UsedRegister> GetUsedRegisters(IEnumerable<UsedRegister> usedRegisterIterator) {
			var read = new List<Register>();
			var write = new List<Register>();
			var condRead = new List<Register>();
			var condWrite = new List<Register>();

			foreach (var info in usedRegisterIterator) {
				switch (info.Access) {
				case OpAccess.Read:
					read.Add(info.Register);
					break;

				case OpAccess.CondRead:
					condRead.Add(info.Register);
					break;

				case OpAccess.Write:
					write.Add(info.Register);
					break;

				case OpAccess.CondWrite:
					condWrite.Add(info.Register);
					break;

				case OpAccess.ReadWrite:
					read.Add(info.Register);
					write.Add(info.Register);
					break;

				case OpAccess.ReadCondWrite:
					read.Add(info.Register);
					condWrite.Add(info.Register);
					break;

				case OpAccess.None:
				case OpAccess.NoMemAccess:
				default:
					throw new InvalidOperationException();
				}
			}

			foreach (var reg in GetRegisters(read))
				yield return new UsedRegister(reg, OpAccess.Read);
			foreach (var reg in GetRegisters(write))
				yield return new UsedRegister(reg, OpAccess.Write);
			foreach (var reg in GetRegisters(condRead))
				yield return new UsedRegister(reg, OpAccess.CondRead);
			foreach (var reg in GetRegisters(condWrite))
				yield return new UsedRegister(reg, OpAccess.CondWrite);
		}

		IEnumerable<Register> GetRegisters(List<Register> regs) {
			if (regs.Count <= 1)
				return regs;

			regs.Sort(RegisterSorter);

			var hash = new HashSet<Register>();
			int index;
			foreach (var reg in regs) {
				if (Register.EAX <= reg && reg <= Register.R15D) {
					index = reg - Register.EAX;
					if (hash.Contains(Register.RAX + index))
						continue;
				}
				else if (Register.AX <= reg && reg <= Register.R15W) {
					index = reg - Register.AX;
					if (hash.Contains(Register.RAX + index))
						continue;
					if (hash.Contains(Register.EAX + index))
						continue;
				}
				else if (Register.AL <= reg && reg <= Register.R15L) {
					index = reg - Register.AL;
					if (Register.AH <= reg && reg <= Register.BH)
						index -= 4;
					if (hash.Contains(Register.RAX + index))
						continue;
					if (hash.Contains(Register.EAX + index))
						continue;
					if (hash.Contains(Register.AX + index))
						continue;
				}
				else if (Register.YMM0 <= reg && reg <= IcedConstants.YMM_last) {
					index = reg - Register.YMM0;
					if (hash.Contains(Register.ZMM0 + index))
						continue;
				}
				else if (Register.XMM0 <= reg && reg <= IcedConstants.XMM_last) {
					index = reg - Register.XMM0;
					if (hash.Contains(Register.ZMM0 + index))
						continue;
					if (hash.Contains(Register.YMM0 + index))
						continue;
				}
				hash.Add(reg);
			}

			foreach (var info in lowRegs) {
				if (hash.Contains(info.rl) && hash.Contains(info.rh)) {
					hash.Remove(info.rl);
					hash.Remove(info.rh);
					hash.Add(info.rx);
				}
			}

			return hash;
		}
		static readonly (Register rl, Register rh, Register rx)[] lowRegs = new(Register rl, Register rh, Register rx)[4] {
			(Register.AL, Register.AH, Register.AX),
			(Register.CL, Register.CH, Register.CX),
			(Register.DL, Register.DH, Register.DX),
			(Register.BL, Register.BH, Register.BX),
		};

		static int RegisterSorter(Register x, Register y) {
			int c = GetRegisterGroupOrder(x) - GetRegisterGroupOrder(y);
			if (c != 0)
				return c;
			return x - y;
		}

		static int GetRegisterGroupOrder(Register reg) {
			if (Register.RAX <= reg && reg <= Register.R15)
				return 0;
			if (Register.EAX <= reg && reg <= Register.R15D)
				return 1;
			if (Register.AX <= reg && reg <= Register.R15W)
				return 2;
			if (Register.AL <= reg && reg <= Register.R15L)
				return 3;

			if (Register.ZMM0 <= reg && reg <= IcedConstants.ZMM_last)
				return 4;
			if (Register.YMM0 <= reg && reg <= IcedConstants.YMM_last)
				return 5;
			if (Register.XMM0 <= reg && reg <= IcedConstants.XMM_last)
				return 6;

			return -1;
		}

		Decoder CreateDecoder(int codeSize, byte[] codeBytes, DecoderOptions options) {
			var codeReader = new ByteArrayCodeReader(codeBytes);
			var decoder = Decoder.Create(codeSize, codeReader, options);

			switch (codeSize) {
			case 16:
				decoder.IP = DecoderConstants.DEFAULT_IP16;
				break;

			case 32:
				decoder.IP = DecoderConstants.DEFAULT_IP32;
				break;

			case 64:
				decoder.IP = DecoderConstants.DEFAULT_IP64;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(codeSize));
			}

			Assert.Equal(codeSize, decoder.Bitness);
			return decoder;
		}

		static protected IEnumerable<object[]> GetTestCases(int bitness) =>
			InstructionInfoTestReader.GetTestCases(bitness, bitness);
	}
}
#endif
