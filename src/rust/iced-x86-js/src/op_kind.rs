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

use wasm_bindgen::prelude::*;

// GENERATOR-BEGIN: Enum
// ⚠️This was generated by GENERATOR!🦹‍♂️
/// Instruction operand kind
#[wasm_bindgen]
#[derive(Copy, Clone)]
#[allow(non_camel_case_types)]
pub enum OpKind {
	/// A register ([`Register`]).
	///
	/// This operand kind uses [`Instruction.op0Register`], [`Instruction.op1Register`], [`Instruction.op2Register`], [`Instruction.op3Register`] or [`Instruction.op4Register`] depending on operand number. See also [`Instruction.opRegister()`].
	///
	/// [`Register`]: enum.Register.html
	/// [`Instruction.op0Register`]: struct.Instruction.html#method.op0Register
	/// [`Instruction.op1Register`]: struct.Instruction.html#method.op1Register
	/// [`Instruction.op2Register`]: struct.Instruction.html#method.op2Register
	/// [`Instruction.op3Register`]: struct.Instruction.html#method.op3Register
	/// [`Instruction.op4Register`]: struct.Instruction.html#method.op4Register
	/// [`Instruction.opRegister()`]: struct.Instruction.html#method.opRegister
	Register,
	/// Near 16-bit branch. This operand kind uses [`Instruction.nearBranch16`]
	///
	/// [`Instruction.nearBranch16`]: struct.Instruction.html#method.nearBranch16
	NearBranch16,
	/// Near 32-bit branch. This operand kind uses [`Instruction.nearBranch32`]
	///
	/// [`Instruction.nearBranch32`]: struct.Instruction.html#method.nearBranch32
	NearBranch32,
	/// Near 64-bit branch. This operand kind uses [`Instruction.nearBranch64`]
	///
	/// [`Instruction.nearBranch64`]: struct.Instruction.html#method.nearBranch64
	NearBranch64,
	/// Far 16-bit branch. This operand kind uses [`Instruction.farBranch16`] and [`Instruction.farBranchSelector`]
	///
	/// [`Instruction.farBranch16`]: struct.Instruction.html#method.farBranch16
	/// [`Instruction.farBranchSelector`]: struct.Instruction.html#method.farBranchSelector
	FarBranch16,
	/// Far 32-bit branch. This operand kind uses [`Instruction.farBranch32`] and [`Instruction.farBranchSelector`]
	///
	/// [`Instruction.farBranch32`]: struct.Instruction.html#method.farBranch32
	/// [`Instruction.farBranchSelector`]: struct.Instruction.html#method.farBranchSelector
	FarBranch32,
	/// 8-bit constant. This operand kind uses [`Instruction.immediate8`]
	///
	/// [`Instruction.immediate8`]: struct.Instruction.html#method.immediate8
	Immediate8,
	/// 8-bit constant used by the `ENTER`, `EXTRQ`, `INSERTQ` instructions. This operand kind uses [`Instruction.immediate8_2nd`]
	///
	/// [`Instruction.immediate8_2nd`]: struct.Instruction.html#method.immediate8_2nd
	Immediate8_2nd,
	/// 16-bit constant. This operand kind uses [`Instruction.immediate16`]
	///
	/// [`Instruction.immediate16`]: struct.Instruction.html#method.immediate16
	Immediate16,
	/// 32-bit constant. This operand kind uses [`Instruction.immediate32`]
	///
	/// [`Instruction.immediate32`]: struct.Instruction.html#method.immediate32
	Immediate32,
	/// 64-bit constant. This operand kind uses [`Instruction.immediate64`]
	///
	/// [`Instruction.immediate64`]: struct.Instruction.html#method.immediate64
	Immediate64,
	/// An 8-bit value sign extended to 16 bits. This operand kind uses [`Instruction.immediate8to16`]
	///
	/// [`Instruction.immediate8to16`]: struct.Instruction.html#method.immediate8to16
	Immediate8to16,
	/// An 8-bit value sign extended to 32 bits. This operand kind uses [`Instruction.immediate8to32`]
	///
	/// [`Instruction.immediate8to32`]: struct.Instruction.html#method.immediate8to32
	Immediate8to32,
	/// An 8-bit value sign extended to 64 bits. This operand kind uses [`Instruction.immediate8to64`]
	///
	/// [`Instruction.immediate8to64`]: struct.Instruction.html#method.immediate8to64
	Immediate8to64,
	/// A 32-bit value sign extended to 64 bits. This operand kind uses [`Instruction.immediate32to64`]
	///
	/// [`Instruction.immediate32to64`]: struct.Instruction.html#method.immediate32to64
	Immediate32to64,
	/// `seg:[SI]`. This operand kind uses [`Instruction.memorySize`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	MemorySegSI,
	/// `seg:[ESI]`. This operand kind uses [`Instruction.memorySize`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	MemorySegESI,
	/// `seg:[RSI]`. This operand kind uses [`Instruction.memorySize`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	MemorySegRSI,
	/// `seg:[DI]`. This operand kind uses [`Instruction.memorySize`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	MemorySegDI,
	/// `seg:[EDI]`. This operand kind uses [`Instruction.memorySize`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	MemorySegEDI,
	/// `seg:[RDI]`. This operand kind uses [`Instruction.memorySize`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	MemorySegRDI,
	/// `ES:[DI]`. This operand kind uses [`Instruction.memorySize`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	MemoryESDI,
	/// `ES:[EDI]`. This operand kind uses [`Instruction.memorySize`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	MemoryESEDI,
	/// `ES:[RDI]`. This operand kind uses [`Instruction.memorySize`]
	///
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	MemoryESRDI,
	/// 64-bit offset `[xxxxxxxxxxxxxxxx]`. This operand kind uses [`Instruction.memoryAddress64`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`], [`Instruction.memorySize`]
	///
	/// [`Instruction.memoryAddress64`]: struct.Instruction.html#method.memoryAddress64
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	Memory64,
	/// Memory operand.
	///
	/// This operand kind uses [`Instruction.memoryDisplSize`], [`Instruction.memorySize`], [`Instruction.memoryIndexScale`], [`Instruction.memoryDisplacement`], [`Instruction.memoryBase`], [`Instruction.memoryIndex`], [`Instruction.memorySegment`], [`Instruction.segmentPrefix`]
	///
	/// [`Instruction.memoryDisplSize`]: struct.Instruction.html#method.memoryDisplSize
	/// [`Instruction.memorySize`]: struct.Instruction.html#method.memorySize
	/// [`Instruction.memoryIndexScale`]: struct.Instruction.html#method.memoryIndexScale
	/// [`Instruction.memoryDisplacement`]: struct.Instruction.html#method.memoryDisplacement
	/// [`Instruction.memoryBase`]: struct.Instruction.html#method.memoryBase
	/// [`Instruction.memoryIndex`]: struct.Instruction.html#method.memoryIndex
	/// [`Instruction.memorySegment`]: struct.Instruction.html#method.memorySegment
	/// [`Instruction.segmentPrefix`]: struct.Instruction.html#method.segmentPrefix
	Memory,
}
// GENERATOR-END: Enum

#[allow(dead_code)]
pub(crate) fn op_kind_to_iced(value: OpKind) -> iced_x86::OpKind {
	// Safe, the enums are exactly identical
	unsafe { std::mem::transmute(value as u8) }
}

#[allow(dead_code)]
pub(crate) fn iced_to_op_kind(value: iced_x86::OpKind) -> OpKind {
	// Safe, the enums are exactly identical
	unsafe { std::mem::transmute(value as u8) }
}
