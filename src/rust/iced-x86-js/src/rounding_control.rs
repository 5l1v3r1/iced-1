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
/// Rounding control
#[wasm_bindgen]
#[derive(Copy, Clone)]
pub enum RoundingControl {
	/// No rounding mode
	None,
	/// Round to nearest (even)
	RoundToNearest,
	/// Round down (toward -inf)
	RoundDown,
	/// Round up (toward +inf)
	RoundUp,
	/// Round toward zero (truncate)
	RoundTowardZero,
}
// GENERATOR-END: Enum

#[allow(dead_code)]
pub(crate) fn rounding_control_to_iced(value: RoundingControl) -> iced_x86::RoundingControl {
	// Safe, the enums are exactly identical
	unsafe { std::mem::transmute(value as u8) }
}

#[allow(dead_code)]
pub(crate) fn iced_to_rounding_control(value: iced_x86::RoundingControl) -> RoundingControl {
	// Safe, the enums are exactly identical
	unsafe { std::mem::transmute(value as u8) }
}
