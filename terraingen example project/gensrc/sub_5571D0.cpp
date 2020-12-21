/*
    Copyright (c) Mathias Kaerlev 2013-2014.

    This file is part of cuwo.

    cuwo is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    cuwo is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with cuwo.  If not, see <http://www.gnu.org/licenses/>.
*/

#include "main.h"
#include "functions.h"
#include "sections.h"
#include <iostream>

void sub_5571D0()
{
    cpu.push_dword(cpu.reg[EBP]); // [5571D0] push ebp
    cpu.reg[EBP] = cpu.reg[ESP]; // [5571D1] mov ebp,esp
    cpu.push_dword(cpu.reg[ECX]); // [5571D3] push ecx
    mem.write_word(cpu.reg[EBP]+(-0x4), 65535U); // [5571D4] mov word [ebp-0x4],0xffff
    cpu.get_reg16(AX) = mem.read_word(cpu.reg[EBP]+(-0x4)); // [5571DA] mov ax,[ebp-0x4]
    cpu.get_reg8(CL) = cpu.get_reg8(CL) | 255U; // [5571DE] or cl,0xff
    mem.write_word(mem.translate(data_section+8864), cpu.get_reg16(AX)); // [5571E1] mov [0x5842a0],ax
    mem.write_byte(mem.translate(data_section+8866), cpu.get_reg8(CL)); // [5571E7] mov [0x5842a2],cl
    mem.write_byte(mem.translate(data_section+8867), 0U); // [5571ED] mov byte [0x5842a3],0x0
    cpu.reg[ESP] = cpu.reg[EBP]; // [5571F4] mov esp,ebp
    cpu.pop_dword(&cpu.reg[EBP]); // [5571F6] pop ebp
    cpu.reg[ESP] = cpu.reg[ESP]+4; // [5571F7] ret
    return; // [5571F7] ret
}
